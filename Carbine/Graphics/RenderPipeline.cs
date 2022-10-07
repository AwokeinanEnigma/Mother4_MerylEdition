using SFML.Graphics;
using System;
using System.Collections.Generic;

namespace Carbine.Graphics
{
    /// <summary>
    /// A pipeline which renders what is fed to it.
    /// </summary>
    public class RenderPipeline
    {
        /// <summary>
        /// The RenderTarget to render to
        /// </summary>
        public RenderTarget Target
        {
            get
            {
                return this.target;
            }
        }
        /// <summary>
        /// Creates a new RenderPipeline from a rendertarget
        /// </summary>
        /// <param name="target">The RenderTarget to render to</param>
        public RenderPipeline(RenderTarget target)
        {
            this.target = target;
            this.renderables = new List<Renderable>();
            this.renderablesToAdd = new Stack<Renderable>();
            this.renderablesToRemove = new Stack<Renderable>();
            this.uids = new Dictionary<Renderable, int>();
            this.depthCompare = new RenderPipeline.RenderableComparer(this);
            this.viewRect = new FloatRect();
            this.rendRect = new FloatRect();
        }
        /// <summary>
        /// Adds a renderable object to the stack of objects to render
        /// </summary>
        /// <param name="renderable">The renderable to add</param>
        public void Add(Renderable renderable)
        {
            if (!this.renderables.Contains(renderable))
            {
                this.renderablesToAdd.Push(renderable);
                return;
            }
            Console.WriteLine("Tried to add renderable that already exists in the RenderPipeline.");
        }

        /// <summary>
        /// Adds a list of renderables
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="addRenderables">Renderables to add</param>
        public void AddAll<T>(IList<T> addRenderables) where T : Renderable
        {
            int count = addRenderables.Count;
            for (int i = 0; i < count; i++)
            {
                this.Add(addRenderables[i]);
            }
        }
        /// <summary>
        /// Removes a renderable 
        /// </summary>
        /// <param name="renderable">The renderable to remove.</param>
        public void Remove(Renderable renderable)
        {
            if (renderable != null)
            {
                this.renderablesToRemove.Push(renderable);
            }
        }
        public void Update(Renderable renderable)
        {
            this.needToSort = true;
        }
        private void DoAdditions()
        {
            while (this.renderablesToAdd.Count > 0)
            {
                Renderable key = this.renderablesToAdd.Pop();
                this.renderables.Add(key);
                this.uids.Add(key, this.rendCount);
                this.needToSort = true;
                ++this.rendCount;
            }
        }
        private void DoRemovals()
        {
            while (this.renderablesToRemove.Count > 0)
            {
                Renderable key = this.renderablesToRemove.Pop();
                this.renderables.Remove(key);
                this.uids.Remove(key);
            }
        }
        public void Each(Action<Renderable> forEachFunc)
        {
            int count = this.renderables.Count;
            for (int i = 0; i < count; i++)
            {
                forEachFunc(this.renderables[i]);
            }
        }
        public void Clear()
        {
            this.Clear(true);
        }
        public void Clear(bool dispose)
        {
            this.renderablesToRemove.Clear();
            if (dispose)
            {
                foreach (Renderable renderable in this.renderables)
                {
                    this.renderables[0].Dispose();
                }
            }
            this.renderables.Clear();
            if (dispose)
            {
                while (this.renderablesToAdd.Count > 0)
                {
                    this.renderablesToAdd.Pop().Dispose();
                }
            }
            this.renderablesToAdd.Clear();
        }
        public void Draw()
        {
            this.DoAdditions();
            this.DoRemovals();
            if (this.needToSort)
            {
                this.renderables.Sort((IComparer<Renderable>)this.depthCompare);
                this.needToSort = false;
            }
            View view = this.target.GetView();
            this.viewRect.Left = view.Center.X - view.Size.X / 2f;
            this.viewRect.Top = view.Center.Y - view.Size.Y / 2f;
            this.viewRect.Width = view.Size.X;
            this.viewRect.Height = view.Size.Y;
            int count = this.renderables.Count;
            for (int index = 0; index < count; ++index)
            {
                Renderable renderable = this.renderables[index];
                if (renderable.Visible)
                {
                    this.rendRect.Left = renderable.Position.X - renderable.Origin.X;
                    this.rendRect.Top = renderable.Position.Y - renderable.Origin.Y;
                    this.rendRect.Width = renderable.Size.X;
                    this.rendRect.Height = renderable.Size.Y;
                    if (this.rendRect.Intersects(this.viewRect))
                        renderable.Draw(this.target);
                }
            }
        }
        private RenderTarget target;
        private List<Renderable> renderables;
        private Stack<Renderable> renderablesToAdd;
        private Stack<Renderable> renderablesToRemove;
        private bool needToSort;
        private RenderPipeline.RenderableComparer depthCompare;
        private Dictionary<Renderable, int> uids;
        private int rendCount;
        private FloatRect viewRect;
        private FloatRect rendRect;
        private class RenderableComparer : IComparer<Renderable>
        {
            private RenderPipeline pipeline;

            public RenderableComparer(RenderPipeline pipeline)
            {
                this.pipeline = pipeline;
            }

            public int Compare(Renderable x, Renderable y)
            {
                return x.Depth != y.Depth ? x.Depth - y.Depth : this.pipeline.uids[y] - this.pipeline.uids[x];
            }
        }
    }
}
