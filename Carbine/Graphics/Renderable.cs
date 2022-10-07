using SFML.Graphics;
using SFML.System;
using System;

namespace Carbine.Graphics
{
    /// <summary>
    /// Class that defines anything which can be rendered within the game
    /// </summary>
    public abstract class Renderable : IDisposable
    {
        ~Renderable()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// The position of the renderable object within the game.
        /// </summary>
		public virtual Vector2f Position
        {
            get
            {
                return this.position;
            }
            set
            {
                this.position = value;
            }
        }

        /// <summary>
        /// The origin of the renderable object
        /// </summary>
		public virtual Vector2f Origin
        {
            get
            {
                return this.origin;
            }
            set
            {
                this.origin = value;
            }
        }

        /// <summary>
        /// The size of the renderable object.
        /// </summary>
		public virtual Vector2f Size
        {
            get
            {
                return this.size;
            }
            set
            {
                this.size = value;
            }
        }

        /// <summary>
        /// The depth of the renderable object.
        /// </summary>
        public virtual int Depth
        {
            get
            {
                return this.depth;
            }
            set
            {
                this.depth = value;
            }
        }

        /// <summary>
        /// Is this renderable visible?
        /// </summary>
        public virtual bool Visible
        {
            get
            {
                return this.visible;
            }
            set
            {
                this.visible = value;
            }
        }

        public abstract void Draw(RenderTarget target);

        protected virtual void Dispose(bool disposing)
        {
            this.disposed = true;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected Vector2f position;

        protected Vector2f origin;

        protected Vector2f size;

        protected int depth;

        protected bool visible = true;

        protected bool disposed;
    }
}
