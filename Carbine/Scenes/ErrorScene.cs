using SFML.Graphics;
using SFML.System;
using System;
using System.IO;
using Carbine.Graphics;
using Carbine.GUI;
using Carbine.Input;

namespace Carbine.Scenes
{
    public class ErrorScene : Scene
    {
        public ErrorScene(Exception ex)
        {
            StreamWriter streamWriter = new StreamWriter("error.log");
            streamWriter.WriteLine(ex);
            streamWriter.Close();
            Engine.ClearColor = Color.Blue;
            this.title = new TextRegion(new Vector2f(16f, 8f), 0, Engine.DefaultFont, "An unhandled exception has occurred.");
            this.message = new TextRegion(new Vector2f(16f, 32f), 0, Engine.DefaultFont, "Enigma  is obviously an incompetent programmer.");
            this.pressenter = new TextRegion(new Vector2f(16f, 48f), 0, Engine.DefaultFont, "Press Enter/Start to exit.");
            this.exceptionDetails = new TextRegion(new Vector2f(16f, 80f), 0, Engine.DefaultFont, string.Format("{0}\nSee error.log for more details.", ex.Message));
            this.pipeline = new RenderPipeline(Engine.FrameBuffer);
            this.pipeline.Add(this.title);
            this.pipeline.Add(this.message);
            this.pipeline.Add(this.pressenter);
            this.pipeline.Add(this.exceptionDetails);
        }

        public override void Focus()
        {
            base.Focus();
            ViewManager.Instance.FollowActor = null;
            ViewManager.Instance.Center = new Vector2f(160f, 90f);
            Engine.ClearColor = Color.Black;
        }

        public override void Update()
        {
            base.Update();
            if (InputManager.Instance.State[Button.Start] || InputManager.Instance.State[Button.A])
            {
                InputManager.Instance.State[Button.Start] = false;
                InputManager.Instance.State[Button.A] = false;
                this.pipeline.Remove(this.title);
                this.pipeline.Remove(this.message);
                this.pipeline.Remove(this.pressenter);
                this.pipeline.Remove(this.exceptionDetails);
                SceneManager.Instance.Pop();
            }
        }

        public override void Draw()
        {
            this.pipeline.Draw();
            base.Draw();
        }

        protected override void Dispose(bool disposing)
        {
            if (!this.disposed && disposing)
            {
                this.title.Dispose();
                this.message.Dispose();
                this.pressenter.Dispose();
                this.exceptionDetails.Dispose();
            }
            base.Dispose(disposing);
        }

        private RenderPipeline pipeline;

        private TextRegion title;

        private TextRegion message;

        private TextRegion pressenter;

        private TextRegion exceptionDetails;
    }
}
