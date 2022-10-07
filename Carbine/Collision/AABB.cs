using SFML.Graphics;
using SFML.System;

namespace Carbine.Collision
{
    public struct AABB
    {
        public AABB(Vector2f position, Vector2f size)
        {
            this.Position = position;
            this.Size = size;
            this.IsPlayer = false;
            this.OnlyPlayer = false;
            this.floatRect = new FloatRect(this.Position.X, this.Position.Y, this.Size.X, this.Size.Y);
        }

        public AABB(Vector2f position, Vector2f size, bool isPlayer, bool onlyPlayer)
        {
            this.Position = position;
            this.Size = size;
            this.IsPlayer = isPlayer;
            this.OnlyPlayer = onlyPlayer;
            this.floatRect = new FloatRect(this.Position.X, this.Position.Y, this.Size.X, this.Size.Y);
        }

        public FloatRect GetFloatRect()
        {
            return this.floatRect;
        }

        private FloatRect floatRect;

        public readonly Vector2f Position;

        public readonly Vector2f Size;

        public readonly bool IsPlayer;

        public readonly bool OnlyPlayer;
    }
}
