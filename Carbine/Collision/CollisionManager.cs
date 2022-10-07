using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Carbine.Utility;

namespace Carbine.Collision
{
    /// <summary>
    /// Manages all collision
    /// </summary>
    public class CollisionManager
    {
        public CollisionManager(int width, int height)
        {
            this.spatialHash = new SpatialHash(width, height);
            this.resultStack = new Stack<ICollidable>(512);
            this.resultList = new List<ICollidable>(4);
            collidables = new List<ICollidable>();
        }

        /// <summary>
        /// Adds a collider
        /// </summary>
        /// <param name="collidable">Collider to add</param>
        public void Add(ICollidable collidable)
        {
            this.spatialHash.Insert(collidable);
            collidables.Add(collidable);
        }

        /// <summary>
        /// Adds a list of colliders
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collidables"></param>
        public void AddAll<T>(ICollection<T> collidables) where T : ICollidable
        {
            foreach (T t in collidables)
            {
                ICollidable collidable = t;
                this.collidables.Add(collidable);
                this.Add(collidable);

            }
        }

        /// <summary>
        /// Removes a collider
        /// </summary>
        /// <param name="collidable"></param>
        public void Remove(ICollidable collidable, bool clear)
        {
            collidables.Remove(collidable);
            this.spatialHash.Remove(collidable);
            if (clear)
            {
                Clear();
            }
        }

        public void Filter()
        {
            var itemToRemove = collidables.FindAll(r => r == null);
            itemToRemove.ForEach(x=>spatialHash.Remove(x));
            List<ICollidable> result = collidables.Except(itemToRemove).ToList();
            collidables = result;

        }

        public List<ICollidable> collidables;

        public void Update(ICollidable collidable, Vector2f oldPosition, Vector2f newPosition)
        {
            this.spatialHash.Update(collidable, oldPosition, newPosition);
        }

        public bool PlaceFree(ICollidable obj, Vector2f position)
        {
            return this.PlaceFree(obj, position, null);
        }

        public bool PlaceFree(ICollidable obj, Vector2f position, ICollidable[] collisionResults)
        {
            return this.PlaceFree(obj, position, collisionResults, null);
        }

        public bool PlaceFree(ICollidable obj, Vector2f position, ICollidable[] collisionResults, Type[] ignoreTypes)
        {
            if (collisionResults != null)
            {
                Array.Clear(collisionResults, 0, collisionResults.Length);
            }
            bool flag = false;
            Vector2f offset = obj.Position - position;
            this.resultList.Clear();
            this.spatialHash.Query(obj, offset, this.resultStack);
            int num = 0;
            while (this.resultStack.Count > 0)
            {
                ICollidable collidable = this.resultStack.Pop();
                if (this.PlaceFreeBroadPhase(obj, position, collidable))
                {
                    bool flag2 = this.CheckPositionCollision(obj, position, collidable);
                    if (flag2)
                    {
                        bool flag3 = false;
                        //Console.WriteLine($"Moving collider '{obj}' to position '{position}' ");
                        if (ignoreTypes != null)
                        {
                            for (int i = 0; i < ignoreTypes.Length; i++)
                            {
                                if (ignoreTypes[i] == collidable.GetType())
                                {
                                    flag3 = true;
                                    break;
                                }
                            }
                        }
                        if (!flag3)
                        {
                            flag = true;
                            if (collisionResults == null || num >= collisionResults.Length)
                            {
                                break;
                            }
                            collisionResults[num] = collidable;
                            num++;
                        }
                    }
                }
            }
            this.resultStack.Clear();
            return !flag;
        }

        public bool GetDoorStuck(Vector2f position)
        {
            return spatialHash.CheckPosition(position);
        }

        public IEnumerable<ICollidable> ObjectsAtPosition(Vector2f position)
        {
            this.resultList.Clear();
            this.spatialHash.Query(position, this.resultStack);
            while (this.resultStack.Count > 0)
            {
                ICollidable collidable = this.resultStack.Pop();
                if (position.X >= collidable.Position.X + collidable.AABB.Position.X && position.X < collidable.Position.X + collidable.AABB.Position.X + collidable.AABB.Size.X && position.Y >= collidable.Position.Y + collidable.AABB.Position.Y && position.Y < collidable.Position.Y + collidable.AABB.Position.Y + collidable.AABB.Size.Y)
                {
                    this.resultList.Add(collidable);
                }
            }
            return this.resultList;
        }

        private bool PlaceFreeBroadPhase(ICollidable objA, Vector2f position, ICollidable objB)
        {
            if (objA == objB)
            {
                return false;
            }
            if (objA.AABB.OnlyPlayer && !objB.AABB.IsPlayer)
            {
                return false;
            }
            if (!objA.Solid || !objB.Solid)
            {
                return false;
            }
            FloatRect floatRect = objA.AABB.GetFloatRect();
            floatRect.Left += position.X;
            floatRect.Top += position.Y;
            FloatRect floatRect2 = objB.AABB.GetFloatRect();
            floatRect2.Left += objB.Position.X;
            floatRect2.Top += objB.Position.Y;
            return floatRect.Intersects(floatRect2);
        }

        private bool CheckPositionCollision(ICollidable objA, Vector2f position, ICollidable objB)
        {
            int count = objA.Mesh.Edges.Count;
            int count2 = objB.Mesh.Edges.Count;
            for (int i = 0; i < count + count2; i++)
            {
                Vector2f vector2f;
                if (i < count)
                {
                    vector2f = objA.Mesh.Normals[i];
                }
                else
                {
                    vector2f = objB.Mesh.Normals[i - count];
                }
                vector2f = VectorMath.Normalize(vector2f);
                float minA = 0f;
                float minB = 0f;
                float maxA = 0f;
                float maxB = 0f;
                this.ProjectPolygon(vector2f, objA.Mesh, position, ref minA, ref maxA);
                this.ProjectPolygon(vector2f, objB.Mesh, objB.Position, ref minB, ref maxB);
                if (this.IntervalDistance(minA, maxA, minB, maxB) > 1f)
                {
                    return false;
                }
            }
            return true;
        }

        private float IntervalDistance(float minA, float maxA, float minB, float maxB)
        {
            if (minA < minB)
            {
                return minB - maxA;
            }
            return minA - maxB;
        }

        private void ProjectPolygon(Vector2f normal, Mesh mesh, Vector2f offset, ref float min, ref float max)
        {
            float num = VectorMath.DotProduct(normal, mesh.Vertices[0] + offset);
            min = num;
            max = num;
            for (int i = 0; i < mesh.Vertices.Count; i++)
            {
                num = VectorMath.DotProduct(mesh.Vertices[i] + offset, normal);
                if (num < min)
                {
                    min = num;
                }
                else if (num > max)
                {
                    max = num;
                }
            }
        }

        public void Clear()
        {
            this.spatialHash.Clear();
        }

        public void Draw(RenderTarget target)
        {
            this.spatialHash.DebugDraw(target);
        }

        // Token: 0x0400004B RID: 75
        private SpatialHash spatialHash;

        // Token: 0x0400004C RID: 76
        private Stack<ICollidable> resultStack;

        // Token: 0x0400004D RID: 77
        private List<ICollidable> resultList;
    }
}
