using SFML.Graphics;
using SFML.System;
using Carbine.Utility;

namespace Carbine.Graphics
{
    public class IndexedColorGraphic : Graphic
    {
        public uint CurrentPalette
        {
            get
            {
                return this.currentPalette;
            }
            set
            {
                if (this.currentPalette != value)
                {
                    this.previousPalette = this.currentPalette;
                    this.currentPalette = value;
                }
            }
        }

        public override Color Color
        {
            get
            {
                return this.blend;
            }
            set
            {
                this.blend = value;
            }
        }

        public ColorBlendMode ColorBlendMode
        {
            get
            {
                return this.blendMode;
            }
            set
            {
                this.blendMode = value;
            }
        }

        public uint PreviousPalette
        {
            get
            {
                return this.previousPalette;
            }
        }

        public RenderStates RenderStates
        {
            get
            {
                return this.renderStates;
            }
        }

        public bool AnimationEnabled
        {
            get
            {
                return this.animationEnabled;
            }
            set
            {
                this.animationEnabled = value;
            }
        }

        public IndexedColorGraphic(string resource, string spriteName, Vector2f position, int depth)
        {
            this.texture = TextureManager.Instance.Use(resource);
            this.sprite = new Sprite(this.texture.Image);
            this.Position = position;
            this.sprite.Position = this.Position;
            this.Depth = depth;
            this.Rotation = 0f;
            this.scale = new Vector2f(1f, 1f);
            this.SetSprite(spriteName);
            ((IndexedTexture)this.texture).CurrentPalette = this.currentPalette;
            this.blend = Color.White;
            this.blendMode = ColorBlendMode.Multiply;
            this.renderStates = new RenderStates(BlendMode.Alpha, Transform.Identity, null, IndexedColorGraphic.INDEXED_COLOR_SHADER);
            this.animationEnabled = true;
            this.Visible = true;
        }

        public void SetSprite(string name)
        {
            this.SetSprite(name, true);
        }

        public void SetSprite(string name, bool reset)
        {
            SpriteDefinition spriteDefinition = ((IndexedTexture)this.texture).GetSpriteDefinition(name);
            if (spriteDefinition == null)
            {
                spriteDefinition = ((IndexedTexture)this.texture).GetDefaultSpriteDefinition();
            }
            this.sprite.Origin = spriteDefinition.Origin;
            this.Origin = spriteDefinition.Origin;
            this.sprite.TextureRect = new IntRect(spriteDefinition.Coords.X, spriteDefinition.Coords.Y, spriteDefinition.Bounds.X, spriteDefinition.Bounds.Y);
            this.startTextureRect = this.sprite.TextureRect;
            this.Size = new Vector2f(sprite.TextureRect.Width, sprite.TextureRect.Height);
            this.flipX = spriteDefinition.FlipX;
            this.flipY = spriteDefinition.FlipY;
            this.finalScale.X = (this.flipX ? (-this.scale.X) : this.scale.X);
            this.finalScale.Y = (this.flipY ? (-this.scale.Y) : this.scale.Y);
            this.sprite.Scale = this.finalScale;
            base.Frames = spriteDefinition.Frames;
            base.Speeds = spriteDefinition.Speeds;
            this.mode = spriteDefinition.Mode;
            if (reset)
            {
                this.frame = 0f;
                this.betaFrame = 0f;
                this.speedIndex = 0f;
                this.speedModifier = 1f;
                return;
            }
            this.frame %= Frames;
        }

        protected override void IncrementFrame()
        {
            float frameSpeed = base.GetFrameSpeed();
            switch (this.mode)
            {
                case 0:
                    this.frame = (this.frame + frameSpeed) % Frames;
                    break;
                case 1:
                    this.betaFrame = (this.betaFrame + frameSpeed) % 4f;
                    this.frame = IndexedColorGraphic.MODE_ONE_FRAMES[(int)this.betaFrame];
                    break;
            }
            this.speedIndex = (int)this.frame % this.speeds.Length;
        }

        public override void Draw(RenderTarget target)
        {
            if (!this.disposed && this.visible)
            {
                if (base.Frames > 1 && this.animationEnabled)
                {
                    base.UpdateAnimation();
                }
                this.sprite.Position = this.Position;
                this.sprite.Origin = this.Origin;
                this.sprite.Rotation = this.Rotation;
                this.finalScale.X = (this.flipX ? (-this.scale.X) : this.scale.X);
                this.finalScale.Y = (this.flipY ? (-this.scale.Y) : this.scale.Y);
                this.sprite.Scale = this.finalScale;
                ((IndexedTexture)this.texture).CurrentPalette = this.currentPalette;
                IndexedColorGraphic.INDEXED_COLOR_SHADER.SetParameter("image", this.texture.Image);
                IndexedColorGraphic.INDEXED_COLOR_SHADER.SetParameter("palette", ((IndexedTexture)this.texture).Palette);
                IndexedColorGraphic.INDEXED_COLOR_SHADER.SetParameter("palIndex", ((IndexedTexture)this.texture).CurrentPaletteFloat);
                IndexedColorGraphic.INDEXED_COLOR_SHADER.SetParameter("palSize", ((IndexedTexture)this.texture).PaletteSize);
                IndexedColorGraphic.INDEXED_COLOR_SHADER.SetParameter("blend", this.blend);
                IndexedColorGraphic.INDEXED_COLOR_SHADER.SetParameter("blendMode", (float)this.blendMode);
                if (!this.disposed)
                {
                    target.Draw(this.sprite, this.renderStates);
                }
            }
        }

        public FullColorTexture CopyToTexture()
        {
            return ((IndexedTexture)this.texture).ToFullColorTexture();
        }

        public SpriteDefinition GetSpriteDefinition(string sprite)
        {
            int hashCode = sprite.GetHashCode();
            return ((IndexedTexture)this.texture).GetSpriteDefinition(hashCode);
        }

        private static readonly int[] MODE_ONE_FRAMES = new int[]
        {
            0,
            1,
            0,
            2
        };

        private static readonly Shader INDEXED_COLOR_SHADER = new Shader(EmbeddedResources.GetStream("Carbine.Resources.pal.vert"), EmbeddedResources.GetStream("Carbine.Resources.pal.frag"));

        private RenderStates renderStates;

        private ColorBlendMode blendMode;

        private bool flipX;

        private bool flipY;

        private int mode;

        private float betaFrame;

        private uint previousPalette;

        private uint currentPalette;

        private Color blend;

        private bool animationEnabled;
    }
}
