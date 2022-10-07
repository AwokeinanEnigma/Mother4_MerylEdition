using System;

namespace Carbine.Scenes
{
    public abstract class Scene : IDisposable
    {
        public bool DrawBehind
        {
            get
            {
                return this.drawBehind;
            }
            set
            {
                this.drawBehind = value;
            }
        }

        ~Scene()
        {
            this.Dispose(false);
        }

        public virtual void Focus()
        {
        }

        public virtual void Unfocus()
        {
        }

        public virtual void Unload()
        {
        }

        public virtual void Update()
        {
        }

        public virtual void Draw()
        {
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
            }
            this.disposed = true;
        }

        protected bool disposed;

        private bool drawBehind;
    }
}
