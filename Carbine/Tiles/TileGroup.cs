using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using Carbine.Graphics;
using Carbine.Utility;

namespace Carbine.Tiles
{
    public class TileGroup : Renderable, IDisposable
    {
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

        public override Vector2f Position
        {
            get
            {
                return this.position;
            }
            set
            {
                this.position = value;
                this.ResetTransform();
            }
        }

        public override Vector2f Origin
        {
            get
            {
                return this.origin;
            }
            set
            {
                this.origin = value;
                this.ResetTransform();
            }
        }

        public IndexedTexture Tileset
        {
            get
            {
                return this.tileset;
            }
        }

        public TileGroup(IList<Tile> tiles, string resource, int depth, Vector2f position, uint palette)
        {
            this.tileset = TextureManager.Instance.Use(resource);
            this.tileset.CurrentPalette = palette;
            this.position = position;
            this.depth = depth;
            this.renderState = new RenderStates(BlendMode.Alpha, Transform.Identity, this.tileset.Image, TileGroup.TILE_GROUP_SHADER);
            this.animationEnabled = true;
            this.CreateAnimations(this.tileset.GetSpriteDefinitions());
            this.CreateVertexArray(tiles);
            this.ResetTransform();
        }

        private void ResetTransform()
        {
            Transform identity = Transform.Identity;
            identity.Translate(this.position - this.origin);
            this.renderState.Transform = identity;
        }

        public int GetTileId(Vector2f location)
        {
            Vector2f vector2f = location - this.position + this.origin;
            uint num = (uint)(vector2f.X / 8f + vector2f.Y / 8f * (this.size.X / 8f));
            Vertex vertex = this.vertices[(int)((UIntPtr)(num * 4U))];
            Vector2f texCoords = vertex.TexCoords;
            return (int)(texCoords.X / 8f + texCoords.Y / 8f * (this.tileset.Image.Size.X / 8U));
        }

        private void IDToTexCoords(uint id, out uint tx, out uint ty)
        {
            tx = id * 8U % this.tileset.Image.Size.X;
            ty = id * 8U / this.tileset.Image.Size.X * 8U;
        }

        private void CreateAnimations(ICollection<SpriteDefinition> definitions)
        {
            this.animations = new TileGroup.TileAnimation[definitions.Count];
            foreach (SpriteDefinition spriteDefinition in definitions)
            {
                int num = -1;
                int.TryParse(spriteDefinition.Name, out num);
                if (num >= 0)
                {
                    if (spriteDefinition.Data != null && spriteDefinition.Data.Length > 0)
                    {
                        int[] data = spriteDefinition.Data;
                        float speed = spriteDefinition.Speeds[0];
                        this.animations[num].Tiles = data;
                        this.animations[num].VertIndexes = new List<int>();
                        this.animations[num].Speed = speed;
                    }
                    else
                    {
                        Console.WriteLine("Tried to load tile animation data for animation {0}, but there was no tile data.", num);
                    }
                }
            }
        }

        private void AddVertIndex(Tile tile, int index)
        {
            if (tile.AnimationId > 0)
            {
                try
                {
                    int num = tile.AnimationId - 1;
                    this.animations[num].VertIndexes.Add(index);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ index } was outside range of the array! Error: {ex}");
                    int num = tile.AnimationId;
                    this.animations[num].VertIndexes.Add(index);
                }
            }
        }

        private unsafe void CreateVertexArray(IList<Tile> tiles)
        {
            this.vertices = new Vertex[tiles.Count * 4];
            uint num = 0U;
            uint num2 = 0U;
            Vector2f v = default(Vector2f);
            Vector2f v2 = default(Vector2f);
            fixed (Vertex* ptr = this.vertices)
            {
                for (int i = 0; i < tiles.Count; i++)
                {
                    Vertex* ptr2 = ptr + i * 4;
                    Tile tile = tiles[i];
                    float x = tile.Position.X;
                    float y = tile.Position.Y;
                    ptr2->Position.X = x;
                    ptr2->Position.Y = y;
                    ptr2[1].Position.X = x + 8f;
                    ptr2[1].Position.Y = y;
                    ptr2[2].Position.X = x + 8f;
                    ptr2[2].Position.Y = y + 8f;
                    ptr2[3].Position.X = x;
                    ptr2[3].Position.Y = y + 8f;
                    this.IDToTexCoords(tile.ID, out num, out num2);
                    if (!tile.FlipHorizontal && !tile.FlipVertical)
                    {
                        ptr2->TexCoords.X = num;
                        ptr2->TexCoords.Y = num2;
                        ptr2[1].TexCoords.X = num + 8U;
                        ptr2[1].TexCoords.Y = num2;
                        ptr2[2].TexCoords.X = num + 8U;
                        ptr2[2].TexCoords.Y = num2 + 8U;
                        ptr2[3].TexCoords.X = num;
                        ptr2[3].TexCoords.Y = num2 + 8U;
                    }
                    else if (tile.FlipHorizontal && !tile.FlipVertical)
                    {
                        ptr2->TexCoords.X = num + 8U;
                        ptr2->TexCoords.Y = num2;
                        ptr2[1].TexCoords.X = num;
                        ptr2[1].TexCoords.Y = num2;
                        ptr2[2].TexCoords.X = num;
                        ptr2[2].TexCoords.Y = num2 + 8U;
                        ptr2[3].TexCoords.X = num + 8U;
                        ptr2[3].TexCoords.Y = num2 + 8U;
                    }
                    else if (!tile.FlipHorizontal && tile.FlipVertical)
                    {
                        ptr2->TexCoords.X = num;
                        ptr2->TexCoords.Y = num2 + 8U;
                        ptr2[1].TexCoords.X = num + 8U;
                        ptr2[1].TexCoords.Y = num2 + 8U;
                        ptr2[2].TexCoords.X = num + 8U;
                        ptr2[2].TexCoords.Y = num2;
                        ptr2[3].TexCoords.X = num;
                        ptr2[3].TexCoords.Y = num2;
                    }
                    else
                    {
                        ptr2->TexCoords.X = num + 8U;
                        ptr2->TexCoords.Y = num2 + 8U;
                        ptr2[1].TexCoords.X = num;
                        ptr2[1].TexCoords.Y = num2 + 8U;
                        ptr2[2].TexCoords.X = num;
                        ptr2[2].TexCoords.Y = num2;
                        ptr2[3].TexCoords.X = num + 8U;
                        ptr2[3].TexCoords.Y = num2;
                    }
                    v.X = Math.Min(v.X, ptr2->Position.X);
                    v.Y = Math.Min(v.Y, ptr2->Position.Y);
                    v2.X = Math.Max(v2.X, ptr2[2].Position.X - v.X);
                    v2.Y = Math.Max(v2.Y, ptr2[2].Position.Y - v.Y);
                    this.AddVertIndex(tile, i * 4);
                }
            }
            this.size = v2 - v;
        }

        private unsafe void UpdateAnimations()
        {
            if (!this.animationEnabled)
            {
                return;
            }
            for (int i = 0; i < this.animations.Length; i++)
            {
                TileGroup.TileAnimation tileAnimation = this.animations[i];
                float num = Engine.Frame * tileAnimation.Speed;
                uint num2 = (uint)tileAnimation.Tiles[(int)num % tileAnimation.Tiles.Length];
                this.IDToTexCoords(num2 - 1U, out uint num3, out uint num4);
                fixed (Vertex* ptr = this.vertices)
                {
                    for (int j = 0; j < tileAnimation.VertIndexes.Count; j++)
                    {
                        int num5 = tileAnimation.VertIndexes[j];
                        Vertex* ptr2 = ptr + num5;
                        ptr2->TexCoords.X = num3;
                        ptr2->TexCoords.Y = num4;
                        ptr2[1].TexCoords.X = num3 + 8U;
                        ptr2[1].TexCoords.Y = num4;
                        ptr2[2].TexCoords.X = num3 + 8U;
                        ptr2[2].TexCoords.Y = num4 + 8U;
                        ptr2[3].TexCoords.X = num3;
                        ptr2[3].TexCoords.Y = num4 + 8U;
                    }
                }
            }
        }

        public override void Draw(RenderTarget target)
        {
            TileGroup.TILE_GROUP_SHADER.SetParameter("image", this.tileset.Image);
            TileGroup.TILE_GROUP_SHADER.SetParameter("palette", this.tileset.Palette);
            TileGroup.TILE_GROUP_SHADER.SetParameter("palIndex", this.tileset.CurrentPaletteFloat);
            TileGroup.TILE_GROUP_SHADER.SetParameter("palSize", this.tileset.PaletteSize);
            TileGroup.TILE_GROUP_SHADER.SetParameter("blend", Color.White);
            TileGroup.TILE_GROUP_SHADER.SetParameter("blendMode", 1f);
            this.UpdateAnimations();
            target.Draw(this.vertices, PrimitiveType.Quads, this.renderState);
        }

        protected override void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                TextureManager.Instance.Unuse(this.tileset);
            }
            this.disposed = true;
        }

        private static readonly Shader TILE_GROUP_SHADER = new Shader(EmbeddedResources.GetStream("Carbine.Resources.pal.vert"), EmbeddedResources.GetStream("Carbine.Resources.pal.frag"));

        private Vertex[] vertices;

        private IndexedTexture tileset;

        private RenderStates renderState;

        private TileGroup.TileAnimation[] animations;

        private bool animationEnabled;

        private struct TileAnimation
        {
            public int[] Tiles;

            public IList<int> VertIndexes;

            public float Speed;
        }
    }
}
