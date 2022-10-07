using System;
using System.Collections.Generic;
using Carbine.Graphics;
using Carbine.Input;
using Carbine.Scenes.Transitions;

namespace Carbine.Scenes
{
    public class SceneManager
    {
        public static SceneManager Instance
        {
            get
            {
                if (SceneManager.instance == null)
                {
                    SceneManager.instance = new SceneManager();
                }
                return SceneManager.instance;
            }
        }

        public ITransition Transition
        {
            get
            {
                return this.transition;
            }
            set
            {
                this.transition = value;
            }
        }

        public bool IsTransitioning
        {
            get
            {
                return this.state == SceneManager.State.Transition;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return this.isEmpty;
            }
        }

        public bool CompositeMode
        {
            get
            {
                return this.isCompositeMode;
            }
            set
            {
                this.isCompositeMode = value;
            }
        }

        private SceneManager()
        {
            this.scenes = new SceneManager.SceneStack();
            this.isEmpty = true;
            this.transition = new InstantTransition();
            this.state = SceneManager.State.Scene;
        }

        public void Push(Scene newScene)
        {
            this.Push(newScene, false);
        }

        public void Push(Scene newScene, bool swap)
        {
            if (this.scenes.Count > 0)
            {
                this.previousScene = (swap ? this.scenes.Pop() : this.scenes.Peek());
                this.popped = swap;
            }
            this.scenes.Push(newScene);
            this.SetupTransition();
            this.isEmpty = false;
        }

        public Scene Pop()
        {
            if (this.scenes.Count > 0)
            {
                Scene result = this.scenes.Pop();
                this.popped = true;
                if (this.scenes.Count > 0)
                {
                    this.scenes.Peek();
                    this.SetupTransition();
                }
                else
                {
                    this.isEmpty = true;
                }
                this.previousScene = result;
                return result;
            }
            throw new EmptySceneStackException();
        }

        private void SetupTransition()
        {
            this.transition.Reset();
            this.state = SceneManager.State.Transition;
            InputManager.Instance.Enabled = false;
        }

        public Scene Peek()
        {
            if (this.scenes.Count > 0)
            {
                return this.scenes.Peek();
            }
            throw new EmptySceneStackException();
        }

        public void Clear()
        {
            Scene scene = this.scenes.Peek();
            while (this.scenes.Count > 0)
            {
                Scene scene2 = this.scenes.Pop();
                if (scene2 == scene)
                {
                    scene2.Unfocus();
                }
                scene2.Unload();
            }
        }

        public void Update()
        {
            this.UpdateScene();
            if (this.state == SceneManager.State.Transition)
            {
                this.UpdateTransition();
            }
        }

        private void UpdateScene()
        {
            if (this.scenes.Count > 0)
            {
                Scene scene = this.scenes.Peek();
                scene.Update();
                return;
            }
            throw new EmptySceneStackException();
        }

        private void UpdateTransition()
        {
            if (!this.newSceneShown && this.transition.ShowNewScene)
            {
                if (this.previousScene != null)
                {
                    if (this.popped)
                    {
                        this.previousScene.Unfocus();
                        this.previousScene.Unload();
                        this.previousScene.Dispose();
                        this.popped = false;
                    }
                    else
                    {
                        this.previousScene.Unfocus();
                    }
                }

                Scene scene = this.scenes.Peek();

                    scene.Focus();
                    this.previousScene = null;
                    this.newSceneShown = true;
                

            }

            if (!this.transition.IsComplete)
            {
                this.transition.Update();
                if (!this.transition.Blocking && this.previousScene != null)
                {
                    this.previousScene.Update();
                }
                if (this.transition.Progress > 0.5f && !this.cleanupFlag)
                {
                    TextureManager.Instance.Purge();
                    GC.Collect();
                    this.cleanupFlag = true;
                    return;
                }
            }
            else
            {
                this.state = SceneManager.State.Scene;
                this.newSceneShown = false;
                InputManager.Instance.Enabled = true;
                this.cleanupFlag = false;
            }
        }

        public void AbortTransition()
        {
            if (this.state == SceneManager.State.Transition)
            {
                this.transition = null;
                this.state = SceneManager.State.Scene;
                if (previousScene != null)
                {
                    this.previousScene.Unfocus();
                    this.previousScene.Unload();
                    this.previousScene.Dispose();
                    this.previousScene = null;
                }
            }
        }

        private void CompositeDraw()
        {
            int count = this.scenes.Count;
            for (int i = 0; i < count - 1; i++)
            {
                if (this.scenes[i + 1].DrawBehind)
                {
                    this.scenes[i].Draw();
                }
            }
        }

        public void Draw()
        {
            if (this.scenes.Count > 0)
            {
                if (this.transition.ShowNewScene)
                {
                    if (this.isCompositeMode)
                    {
                        this.CompositeDraw();
                    }
                    Scene scene = this.scenes.Peek();
                    scene.Draw();
                }
                else if (this.previousScene != null)
                {
                    this.previousScene.Draw();
                }
                if (!this.transition.IsComplete)
                {
                    this.transition.Draw();
                }
            }
        }

        private static SceneManager instance;

        private SceneManager.State state;

        private SceneManager.SceneStack scenes;

        private Scene previousScene;

        private ITransition transition;

        private bool isEmpty;

        private bool popped;

        private bool newSceneShown;

        private bool cleanupFlag;

        private bool isCompositeMode;

        private enum State
        {
            Scene,
            Transition
        }

        private class SceneStack
        {
            public Scene this[int i]
            {
                get
                {
                    return this.list[i];
                }
            }

            public int Count
            {
                get
                {
                    return this.list.Count;
                }
            }

            public SceneStack()
            {
                this.list = new List<Scene>();
            }

            public void Clear()
            {
                this.list.Clear();
            }

            public void Push(Scene scene)
            {
                this.list.Add(scene);
            }

            public Scene Peek()
            {
                return this.Peek(0);
            }

            public Scene Peek(int i)
            {
                if (i < 0 || i >= this.list.Count)
                {
                    return null;
                }
                return this.list[this.list.Count - i - 1];
            }

            public Scene Pop()
            {
                Scene result = null;
                if (this.list.Count > 0)
                {
                    result = this.list[this.list.Count - 1];
                    this.list.RemoveAt(this.list.Count - 1);
                }
                return result;
            }

            private List<Scene> list;
        }
    }
}
