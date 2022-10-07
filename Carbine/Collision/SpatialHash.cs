using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using Carbine.Maps;

namespace Carbine.Collision
{
    internal class SpatialHash
    {
        internal const int CELL_SIZE = 256;
        internal const int INITIAL_BUCKET_SIZE = 4;
        internal const int MAX_BUCKET_SIZE = 512;
        private readonly ICollidable[][] buckets;
        private VertexArray debugGridVerts;
        private readonly int heightInCells;
        private readonly int heightInPixels;
        private readonly bool[] touches;
        private readonly int widthInCells;
        private readonly int widthInPixels;

        /// <summary>
        /// Creates a new spatial hash
        /// </summary>
        /// <param name="width">The width of the space that this spatial hash handles collision in</param>
        /// <param name="height">The height of the space that this spatial hash handles collision in</param>
        public SpatialHash(int width, int height)
        {
            widthInPixels = width;
            heightInPixels = height;
            widthInCells = (widthInPixels - 1) / 256 + 1;
            heightInCells = (heightInPixels - 1) / 256 + 1;
            int num = widthInCells * heightInCells;
            buckets = new ICollidable[num][];
            touches = new bool[num];
            InitializeDebugGrid();
        }

        private void InitializeDebugGrid()
        {
            uint vertexCount = (uint)((widthInCells + heightInCells + 2) * 2);
            debugGridVerts = new VertexArray(PrimitiveType.Lines, vertexCount);
            int num = widthInCells * 256;
            int num2 = heightInCells * 256;
            uint num3 = 0U;
            while (num3 <= (ulong)widthInCells)
            {
                debugGridVerts[num3 * 2U] = new Vertex(new Vector2f(num3 * 256U, 0f), Color.Blue);
                debugGridVerts[num3 * 2U + 1U] = new Vertex(new Vector2f(num3 * 256U, num2), Color.Blue);
                num3 += 1U;
            }

            uint num4 = (uint)((widthInCells + 1) * 2);
            uint num5 = 0U;
            while (num5 <= (ulong)heightInCells)
            {
                debugGridVerts[num4 + num5 * 2U] = new Vertex(new Vector2f(0f, num5 * 256U), Color.Blue);
                debugGridVerts[num4 + num5 * 2U + 1U] = new Vertex(new Vector2f(num, num5 * 256U), Color.Blue);
                num5 += 1U;
            }
        }

        private void ClearTouches()
        {
            Array.Clear(touches, 0, touches.Length);
        }
        private int GetPositionHash(int x, int y)
        {
            int num = x / 256;
            int num2 = y / 256;
            return num + num2 * widthInCells;
        }
        private void BucketInsert(int hash, ICollidable collidable)
        {
            int num = -1;
            ICollidable[] array = buckets[hash];
            if (array == null)
            {
                buckets[hash] = new ICollidable[4];
                array = buckets[hash];
            }

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == collidable)
                {
                    return;
                }

                if (num < 0 && array[i] == null)
                {
                    num = i;
                }
            }

            if (num >= 0)
            {
                array[num] = collidable;
                return;
            }

            int num2 = array.Length;
            if (num2 * 2 <= 512)
            {
                Array.Resize(ref array, num2 * 2);
                array[num2] = collidable;
                buckets[hash] = array;
                return;
            }

            string message = string.Format("Cannot to insert more than {0} collidables into a single bucket.", 512);
            throw new InvalidOperationException(message);
        }
        private void BucketRemove(int hash, ICollidable collidable, bool log)
        {
            ICollidable[] array = buckets[hash];
            if (array != null)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i] == collidable)
                    {
                        if (log)
                        {
                            Console.WriteLine($"{collidable} removed!");
                        }

                        array[i] = null;
            
                        return;
                    }
                }
            }

        }

        public void Insert(ICollidable collidable)
        {
            ClearTouches();
            AABB aabb = collidable.AABB;
            int num = ((int)aabb.Size.X - 1) / 256 + 1;
            int num2 = ((int)aabb.Size.Y - 1) / 256 + 1;
            for (int i = 0; i <= num2; i++)
            {
                int y = i == num2
                    ? (int)(collidable.Position.Y + aabb.Position.Y) + (int)aabb.Size.Y
                    : (int)(collidable.Position.Y + aabb.Position.Y) + 256 * i;
                for (int j = 0; j <= num; j++)
                {
                    int x = j == num
                        ? (int)(collidable.Position.X + aabb.Position.X) + (int)aabb.Size.X
                        : (int)(collidable.Position.X + aabb.Position.X) + 256 * j;
                    int positionHash = GetPositionHash(x, y);
                    if (positionHash >= 0 && positionHash < buckets.Length && !touches[positionHash])
                    {
                        touches[positionHash] = true;
                        BucketInsert(positionHash, collidable);
                    }
                }
            }
        }


        public void Update(ICollidable collidable, Vector2f oldPosition, Vector2f newPosition)
        {
            ClearTouches();
            AABB aabb = collidable.AABB;
            int num = ((int)aabb.Size.X - 1) / 256 + 1;
            int num2 = ((int)aabb.Size.Y - 1) / 256 + 1;



            for (int i = 0; i <= num2; i++)
            {
                int y = i == num2
                    ? (int)(oldPosition.Y + aabb.Position.Y) + (int)aabb.Size.Y
                    : (int)(oldPosition.Y + aabb.Position.Y) + 256 * i;

                int y2 = i == num2
                    ? (int)(newPosition.Y + aabb.Position.Y) + (int)aabb.Size.Y
                    : (int)(newPosition.Y + aabb.Position.Y) + 256 * i;

                for (int j = 0; j <= num; j++)
                {
                    int x = j == num
                        ? (int)(oldPosition.X + aabb.Position.X) + (int)aabb.Size.X
                        : (int)(oldPosition.X + aabb.Position.X) + 256 * j;

                    int x2 = j == num
                        ? (int)(newPosition.X + aabb.Position.X) + (int)aabb.Size.X
                        : (int)(newPosition.X + aabb.Position.X) + 256 * j;

                    int positionHash = GetPositionHash(x, y);
                    int positionHash2 = GetPositionHash(x2, y2);
                    bool flag = positionHash >= 0 && positionHash < buckets.Length;
                    bool flag2 = positionHash2 >= 0 && positionHash2 < buckets.Length;
                    if (flag && !touches[positionHash] || flag2 && !touches[positionHash2])
                    {
                        if (flag && positionHash != positionHash2)
                        {
                            BucketRemove(positionHash, collidable, false);
                        }

                        if (flag2 && positionHash != positionHash2)
                        {
                            BucketInsert(positionHash2, collidable);
                        }

                        if (flag)
                        {
                            touches[positionHash] = true;
                        }

                        if (flag2)
                        {
                            touches[positionHash2] = true;
                        }
                    }
                }
            }
        }

        public bool CheckPosition(Vector2f position)
        {
            foreach (ICollidable[] collider0 in buckets)
            {
                foreach (ICollidable collidable in collider0)
                {
                    if (position.X >= collidable.Position.X + collidable.AABB.Position.X && position.X < collidable.Position.X + collidable.AABB.Position.X + collidable.AABB.Size.X && position.Y >= collidable.Position.Y + collidable.AABB.Position.Y && position.Y < collidable.Position.Y + collidable.AABB.Position.Y + collidable.AABB.Size.Y)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            return true;

        }

        public void Remove(ICollidable collidable)
        {
            ClearTouches();
            AABB aabb = collidable.AABB;
            int num = ((int)aabb.Size.X - 1) / 256 + 1;
            int num2 = ((int)aabb.Size.Y - 1) / 256 + 1;
            //Console.WriteLine($"removing collider {collidable}");
            for (int i = 0; i <= num2; i++)
            {
                int y = i == num2
                    ? (int)(collidable.Position.Y + aabb.Position.Y) + (int)aabb.Size.Y
                    : (int)(collidable.Position.Y + aabb.Position.Y) + 256 * i;
                for (int j = 0; j <= num; j++)
                {
                    int x = j == num
                        ? (int)(collidable.Position.X + aabb.Position.X) + (int)aabb.Size.X
                        : (int)(collidable.Position.X + aabb.Position.X) + 256 * j;
                    int positionHash = GetPositionHash(x, y);
                    if (positionHash >= 0 && positionHash < buckets.Length && !touches[positionHash])
                    {
                        touches[positionHash] = true;
                        BucketRemove(positionHash, collidable, false);
                    }
                }
            }
        }

        public void Query(Vector2f point, Stack<ICollidable> resultStack)
        {
            int positionHash = GetPositionHash((int)point.X, (int)point.Y);
            if (positionHash < 0 || positionHash >= buckets.Length || touches[positionHash])
            {
                return;
            }

            ICollidable[] array = buckets[positionHash];
            if (array != null)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    if (array[i] != null)
                    {
                        resultStack.Push(array[i]);
                    }
                }
            }
        }

        public void Query(ICollidable collidable, Stack<ICollidable> resultStack)
        {
            Query(collidable, new Vector2f(0f, 0f), resultStack);
        }

        public void Query(ICollidable collidable, Vector2f offset, Stack<ICollidable> resultStack)
        {
            ClearTouches();
            Vector2f vector2f = collidable.Position + offset;
            AABB aabb = collidable.AABB;
            int num = ((int)aabb.Size.X - 1) / 256 + 1;
            int num2 = ((int)aabb.Size.Y - 1) / 256 + 1;
            for (int i = 0; i <= num2; i++)
            {
                int y = i == num2
                    ? (int)(vector2f.Y + aabb.Position.Y) + (int)aabb.Size.Y
                    : (int)(vector2f.Y + aabb.Position.Y) + 256 * i;
                for (int j = 0; j <= num; j++)
                {
                    int x = j == num
                        ? (int)(vector2f.X + aabb.Position.X) + (int)aabb.Size.X
                        : (int)(vector2f.X + aabb.Position.X) + 256 * j;
                    int positionHash = GetPositionHash(x, y);
                    if (positionHash >= 0 && positionHash < buckets.Length && !touches[positionHash])
                    {
                        touches[positionHash] = true;
                        ICollidable[] array = buckets[positionHash];
                        if (array != null)
                        {
                            for (int k = 0; k < array.Length; k++)
                            {
                                if (array[k] != null && array[k] != collidable)
                                {
                                    resultStack.Push(array[k]);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void Clear()
        {
            for (int i = 0; i < buckets.Length; i++)
            {
                ICollidable[] array = buckets[i];
                if (array != null)
                {
                    for (int j = 0; j < array.Length; j++)
                    {
                        array[j] = null;
                    }
                }
            }
        }

        public void DebugDraw(RenderTarget target)
        {
            RenderStates states = new RenderStates(BlendMode.Alpha, Transform.Identity, null, null);
            for (int i = 0; i < buckets.Length; i++)
            {
                ICollidable[] array = buckets[i];
                if (array != null)
                {
                    foreach (ICollidable collidable in array)
                    {
                        if (collidable != null && collidable.DebugVerts != null)
                        {

                            states.Transform = Transform.Identity;
                            states.Transform.Translate(collidable.Position);
                            target.Draw(collidable.DebugVerts, states);
                        }
                    }
                }
            }

            target.Draw(debugGridVerts);
        }
    }
}