using SFML.Graphics;
using SFML.System;
using System.Collections.Generic;
using Carbine.Utility;

namespace Carbine.Collision
{
    /// <summary>
    /// This class handles points that define a mesh. 
    /// </summary>
    public class Mesh
    {
        private AABB aabb;

        /// <summary>
        /// Creates a new mesh
        /// </summary>
        /// <param name="points">The bounds of the mesh</param>
        public Mesh(List<Vector2f> points)
        {
            AddPoints(points);
        }

        /// <summary>
        /// Creates a new mesh from a FloatRect
        /// </summary>
        /// <param name="rectangle">The bounds of the mesh </param>
        public Mesh(FloatRect rectangle)
        {
            AddRectangle(rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height);
        }
        /// <summary>
        /// Creates a new mesh from an IntRect
        /// </summary>
        /// <param name="rectangle">The bounds of the mesh </param>
        public Mesh(IntRect rectangle)
        {
            AddRectangle(rectangle.Left, rectangle.Top, rectangle.Width, rectangle.Height);
        }

        /// <summary>
        /// The vertices of the mesh
        /// </summary>
        public List<Vector2f> Vertices { get; private set; }
        /// <summary>
        /// Edges of the mesh
        /// </summary>
        public List<Vector2f> Edges { get; private set; }
        /// <summary>
        /// The normals of the mesh
        /// </summary>
        public List<Vector2f> Normals { get; private set; }

        public AABB AABB => aabb;

        /// <summary>
        /// The center of the mesh
        /// </summary>
        public Vector2f Center => new Vector2f(aabb.Size.X / 2f, aabb.Size.Y / 2f);

        private void AddPoints(List<Vector2f> points)
        {
            Vertices = new List<Vector2f>();
            Edges = new List<Vector2f>();
            Normals = new List<Vector2f>();
            for (int i = 0; i < points.Count; i++)
            {
                Vertices.Add(points[i]);
                int index = (i + 1) % points.Count;
                float x = points[index].X - points[i].X;
                float y = points[index].Y - points[i].Y;
                Vector2f vector2f = new Vector2f(x, y);
                Edges.Add(vector2f);
                Vector2f item = VectorMath.RightNormal(vector2f);
                Normals.Add(item);
            }

            aabb = GetAABB();
        }

        private void AddRectangle(float x, float y, float width, float height)
        {
            AddPoints(new List<Vector2f>
            {
                new Vector2f(x, y),
                new Vector2f(x + width, y),
                new Vector2f(x + width, y + height),
                new Vector2f(x, y + height)
            });
        }

        private AABB GetAABB()
        {
            float num = float.MinValue;
            float num2 = float.MinValue;
            float num3 = float.MaxValue;
            float num4 = float.MaxValue;
            foreach (Vector2f vector2f in Vertices)
            {
                num3 = vector2f.X < num3 ? vector2f.X : num3;
                num = vector2f.X > num ? vector2f.X : num;
                num4 = vector2f.Y < num4 ? vector2f.Y : num4;
                num2 = vector2f.Y > num2 ? vector2f.Y : num2;
            }

            return new AABB(new Vector2f(num3, num4), new Vector2f(num - num3, num2 - num4));
        }
    }
}