using SFML.Graphics;
using SFML.System;
using System;
using Carbine.Utility;

namespace Carbine.Graphics
{
    public class TiledBackground : Renderable
    {
        public Vector2f Velocity
        {
            get
            {
                return this.velocity;
            }
            set
            {
                this.velocity = value;
            }
        }

        public TiledBackground(string resource, IntRect area, bool xRepeat, bool yRepeat, Vector2f velocity, int depth)
        {
            this.xRepeat = xRepeat;
            this.yRepeat = yRepeat;
            this.velocity = velocity;
            this.texture = TextureManager.Instance.Use(resource);
            int x = (int)this.texture.Image.Size.X;
            int y = (int)this.texture.Image.Size.Y;
            this.shader = new Shader(EmbeddedResources.GetStream("Carbine.Resources.pal.vert"), EmbeddedResources.GetStream("Carbine.Resources.pal.frag"));
            this.shader.SetParameter("image", this.texture.Image);
            this.shader.SetParameter("palette", this.texture.Palette);
            this.shader.SetParameter("palIndex", 0f);
            this.shader.SetParameter("palSize", this.texture.PaletteSize);
            this.shader.SetParameter("blend", Color.White);
            this.shader.SetParameter("blendMode", 1f);
            this.states = new RenderStates(BlendMode.Alpha, Transform.Identity, this.texture.Image, this.shader);
            float num = area.Width / (float)x;
            float num2 = area.Height / (float)y;
            this.xRepeatCount = (int)Math.Ceiling(num) + (xRepeat ? 1 : 0);
            this.yRepeatCount = (int)Math.Ceiling(num2) + (yRepeat ? 1 : 0);
            this.sprites = new Sprite[this.xRepeatCount, this.yRepeatCount];
            for (int i = 0; i < this.yRepeatCount; i++)
            {
                for (int j = 0; j < this.xRepeatCount; j++)
                {
                    this.sprites[j, i] = new Sprite(this.texture.Image);
                    this.sprites[j, i].Position = new Vector2f(area.Left + (xRepeat ? (-x) : 0) + x * j, area.Top + (yRepeat ? (-y) : 0) + y * i);
                }
            }
            this.area = new IntRect(area.Left - x, area.Top - y, area.Width + x, area.Height + y);
            this.size = new Vector2f(area.Width, area.Height);
            this.position = new Vector2f(0f, 0f);
            this.origin = new Vector2f(0f, 0f);
        }

        public override void Draw(RenderTarget target)
        {
            for (int i = 0; i < this.yRepeatCount; i++)
            {
                for (int j = 0; j < this.xRepeatCount; j++)
                {
                    target.Draw(this.sprites[j, i], this.states);
                    this.sprites[j, i].Position += this.velocity;
                    this.Wrap(this.sprites[j, i]);
                }
            }
        }

        private void Wrap(Sprite sprite)
        {
            if (sprite.Position.X < area.Left)
            {
                sprite.Position += new Vector2f(area.Width, 0f);
            }
            if (sprite.Position.Y < area.Top)
            {
                sprite.Position += new Vector2f(0f, area.Height);
            }
            if (sprite.Position.X > this.area.Left + this.area.Width)
            {
                sprite.Position -= new Vector2f(area.Width, 0f);
            }
            if (sprite.Position.Y > this.area.Top + this.area.Height)
            {
                sprite.Position -= new Vector2f(0f, area.Height);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    for (int i = 0; i < this.yRepeatCount; i++)
                    {
                        for (int j = 0; j < this.xRepeatCount; j++)
                        {
                            this.sprites[j, i].Dispose();
                        }
                    }
                }
                TextureManager.Instance.Unuse(this.texture);
            }
            this.disposed = true;
        }

        private IntRect area;

        protected int xRepeatCount;

        protected int yRepeatCount;

        private bool xRepeat;

        private bool yRepeat;

        private Vector2f velocity;

        protected IndexedTexture texture;

        protected Sprite[,] sprites;

        private Shader shader;

        private RenderStates states;
    }
}
