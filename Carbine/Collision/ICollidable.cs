using SFML.Graphics;
using SFML.System;

namespace Carbine.Collision
{
    public interface ICollidable
    {
        /// <summary>
        /// The position of this collider
        /// </summary>
        Vector2f Position { get; set; }
        /// <summary>
        /// The velocity of this collider
        /// </summary>
        Vector2f Velocity { get; }
        /// <summary>
        /// The AABB of this collider
        /// </summary>
        AABB AABB { get; }
        /// <summary>
        /// The mesh of this collider
        /// </summary>
        Mesh Mesh { get; }
        /// <summary>
        /// Can we walk through this collider or not?
        /// </summary>
        bool Solid { get; set; }
        /// <summary>
        /// Debug vertices to draw when debug mode is enabled.
        /// </summary>
        VertexArray DebugVerts { get; }
        /// <summary>
        /// When this collider collides with another object
        /// </summary>
        /// <param name="context">Information about the collision</param>
        void Collision(CollisionContext context);
    }
}