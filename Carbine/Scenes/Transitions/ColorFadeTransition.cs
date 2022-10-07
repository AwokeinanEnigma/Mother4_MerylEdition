using SFML.Graphics;
using SFML.System;
using System;
using Carbine.Graphics;

namespace Carbine.Scenes.Transitions
{
    public class ColorFadeTransition : ITransition
    {
        public bool IsComplete
        {
            get
            {
                return this.isComplete;
            }
        }

        public float Progress
        {
            get
            {
                return this.progress;
            }
        }

        public bool ShowNewScene
        {
            get
            {
                return this.progress > 0.5f;
            }
        }

        public bool Blocking { get; set; }

        public ColorFadeTransition(float duration, Color color)
        {
            float num = 60f * duration;
            this.speed = 1f / num;
            this.isComplete = false;
            this.progress = 0f;
            this.target = Engine.FrameBuffer;
            float num2 = 160f;
            float num3 = 90f;
            this.verts = new Vertex[4];
            this.verts[0] = new Vertex(new Vector2f(-num2, -num3), color);
            this.verts[1] = new Vertex(new Vector2f(num2, -num3), color);
            this.verts[2] = new Vertex(new Vector2f(num2, num3), color);
            this.verts[3] = new Vertex(new Vector2f(-num2, num3), color);
            Transform transform = new Transform(1f, 0f, ViewManager.Instance.FinalCenter.X, 0f, 1f, ViewManager.Instance.FinalCenter.Y, 0f, 0f, 1f);
            this.renderStates = new RenderStates(transform);
        }

        public void Update()
        {
            this.progress += this.speed;
            this.isComplete = (this.progress > 1f);
            byte b = (byte)(255.0 * (Math.Cos(this.progress * 2f * 3.141592653589793 + 3.141592653589793) / 2.0 + 0.5));
            b /= 25;
            b *= 25;
            this.verts[0].Color.A = b;
            this.verts[1].Color.A = b;
            this.verts[2].Color.A = b;
            this.verts[3].Color.A = b;
        }

        public void Draw()
        {
            this.renderStates.Transform = new Transform(1f, 0f, ViewManager.Instance.FinalCenter.X, 0f, 1f, ViewManager.Instance.FinalCenter.Y, 0f, 0f, 1f);
            this.target.Draw(this.verts, PrimitiveType.Quads, this.renderStates);
        }

        public void Reset()
        {
            this.isComplete = false;
            this.progress = 0f;
            this.verts[0].Color.A = 0;
            this.verts[1].Color.A = 0;
            this.verts[2].Color.A = 0;
            this.verts[3].Color.A = 0;
        }

        private const int STEPS = 10;

        private float speed;

        private bool isComplete;

        private float progress;

        private RenderTarget target;

        private Vertex[] verts;

        private RenderStates renderStates;
    }
}
