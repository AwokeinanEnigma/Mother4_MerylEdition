using SFML.Graphics;
using SFML.System;
using Carbine.Utility;

namespace Carbine.Collision
{
    /// <summary>
    /// A static object that is static (unmoving)
    /// </summary>
    public class SolidStatic : ICollidable
    {
        /// <summary>
        /// Creates a new solid static object from a mesh
        /// </summary>
        /// <param name="mesh"></param>
        public SolidStatic(Mesh mesh)
        {
            Mesh = mesh;
            AABB = mesh.AABB;
            Position = new Vector2f(0f, 0f);
            Solid = true;
            VertexArray vertexArray = new VertexArray(PrimitiveType.LinesStrip, (uint)(mesh.Vertices.Count + 1));
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                vertexArray[(uint)i] = new Vertex(mesh.Vertices[i], Color.Red);
            }

            vertexArray[(uint)mesh.Vertices.Count] = new Vertex(mesh.Vertices[0], Color.Red);
            DebugVerts = vertexArray;
        }

        public Vector2f Position { get; set; }

        public Vector2f Velocity => VectorMath.ZERO_VECTOR;

        public AABB AABB { get; }

        public Mesh Mesh { get; }

        public bool Solid { get; set; }

        public VertexArray DebugVerts { get; }

        public void Collision(CollisionContext context)
        {
        }
    }
}