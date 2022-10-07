using SFML.Graphics;
using SFML.System;
using System;
using Carbine.Graphics;
using Carbine.Utility;

namespace Carbine.GUI
{
    public class TextRegion : Renderable
    {
        public override Vector2f Position
        {
            get
            {
                return this.position;
            }
            set
            {
                this.position = value;
                this.drawText.Position = new Vector2f(this.position.X + xCompensate, this.position.Y + yCompensate);
            }
        }

        public string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                this.text = value;
                this.dirtyText = true;
            }
        }

        public int Index
        {
            get
            {
                return this.index;
            }
            set
            {
                this.index = value;
                this.dirtyText = true;
            }
        }

        public int Length
        {
            get
            {
                return this.length;
            }
            set
            {
                int num = this.length;
                this.length = value;
                this.dirtyText = (this.length != num);
            }
        }

        public Color Color
        {
            get
            {
                return this.drawText.Color;
            }
            set
            {
                this.drawText.Color = value;
                this.dirtyColor = true;
            }
        }

        public TextRegion(Vector2f position, int depth, FontData font, string text) : this(position, depth, font, (text != null) ? text : string.Empty, 0, (text != null) ? text.Length : 0)
        {
        }

        public TextRegion(Vector2f position, int depth, FontData font, string text, int index, int length)
        {
            this.position = position;
            this.text = text;
            this.index = index;
            this.length = length;
            this.depth = depth;
            this.xCompensate = font.XCompensation;
            this.yCompensate = font.YCompensation;
            this.drawText = new Text(string.Empty, font.Font, font.Size);
            this.drawText.Position = new Vector2f(position.X + xCompensate, position.Y + yCompensate);
            this.UpdateText(index, length);
            this.shader = new Shader(EmbeddedResources.GetStream("Carbine.Resources.text.vert"), EmbeddedResources.GetStream("Carbine.Resources.text.frag"));
            this.shader.SetParameter("color", this.drawText.Color);
            this.shader.SetParameter("threshold", font.AlphaThreshold);
            this.renderStates = new RenderStates(BlendMode.Alpha, Transform.Identity, null, this.shader);
        }

        public void Reset(string text, int index, int length)
        {
            this.text = text;
            this.index = index;
            this.length = length;
            this.UpdateText(index, length);
        }

        private void UpdateText(int index, int length)
        {
            this.drawText.DisplayedString = this.text.Substring(index, length);
            FloatRect localBounds = this.drawText.GetLocalBounds();
            this.size = new Vector2f(Math.Max(1f, localBounds.Width), Math.Max(16f, localBounds.Height));
        }

        public override void Draw(RenderTarget target)
        {
            if (this.dirtyText)
            {
                this.UpdateText(this.Index, this.Length);
                this.dirtyText = false;
            }
            if (this.dirtyColor)
            {
                this.shader.SetParameter("color", this.drawText.Color);
            }
            target.Draw(this.drawText, this.renderStates);
        }

        protected override void Dispose(bool disposing)
        {
            if (!this.disposed && disposing)
            {
                this.drawText.Dispose();
            }
            this.disposed = true;
        }

        private Shader shader;

        private RenderStates renderStates;

        private Text drawText;

        private string text;

        private int index;

        private int length;

        private bool dirtyText;

        private bool dirtyColor;

        private int xCompensate;

        private int yCompensate;
    }
}
