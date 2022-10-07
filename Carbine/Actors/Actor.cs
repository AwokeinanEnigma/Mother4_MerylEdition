using SFML.System;
using System;

namespace Carbine.Actors
{
    /// <summary>
    ///     An object within a scene capable of movement
    /// </summary>
    public abstract class Actor : IDisposable
    {
        /// <summary>
        ///     Has this actor been disposed?
        /// </summary>
        protected bool disposed;

        /// <summary>
        ///     The protected position of the actor
        /// </summary>
        protected Vector2f position;

        /// <summary>
        ///     The protected velocity of the actor
        /// </summary>
        protected Vector2f velocity;

        /// <summary>
        ///     The protected z Offset of the actor
        /// </summary>
        protected float zOffset;

        /// <summary>
        ///     The position of this object within the game space.
        /// </summary>
        public virtual Vector2f Position
        {
            get => position;
            set => position = value;
        }

        /// <summary>
        ///     How fast this actor is moving
        /// </summary>
        public virtual Vector2f Velocity => velocity;

        /// <summary>
        ///     The z offset
        /// </summary>
        public virtual float ZOffset
        {
            get => zOffset;
            set => zOffset = value;
        }

        /// <summary>
        ///     Disposes the object and frees it from memory
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Kills the class
        /// </summary>
        ~Actor()
        {
            Dispose(false);
        }


        public virtual void Input()
        {
        }

        /// <summary>
        ///     Called every frame to move the object by its velocity
        /// </summary>
        public virtual void Update()
        {
            position += velocity;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
            }

            disposed = true;
        }
    }
}