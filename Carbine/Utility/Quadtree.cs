using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;

namespace Carbine.Utility
{
    internal class Quadtree<T> where T : class
    {
        public IComparer<T> Comparer { get; set; }

        public Quadtree(int level, Rectangle bounds)
        {
            this.level = level;
            this.bounds = bounds;
            this.objects = new List<T>();
            this.nodes = new Quadtree<T>[4];
        }

        protected Quadtree()
        {
        }

        public virtual void Clear()
        {
            this.objects.Clear();
            for (int i = 0; i < this.nodes.Length; i++)
            {
                if (this.nodes[i] != null)
                {
                    this.nodes[i].Clear();
                    this.nodes[i] = null;
                }
            }
        }

        protected virtual void Split()
        {
            int num = (int)(this.bounds.Width / 2f);
            int num2 = (int)(this.bounds.Height / 2f);
            int num3 = (int)this.bounds.X;
            int num4 = (int)this.bounds.Y;
            int num5 = this.level + 1;
            this.nodes[0] = new Quadtree<T>(num5, new Rectangle(num3 + num, num4, num, num2));
            this.nodes[1] = new Quadtree<T>(num5, new Rectangle(num3, num4, num, num2));
            this.nodes[2] = new Quadtree<T>(num5, new Rectangle(num3, num4 + num2, num, num2));
            this.nodes[3] = new Quadtree<T>(num5, new Rectangle(num3 + num, num4 + num2, num, num2));
        }

        protected virtual int FindIndex(T obj)
        {
            throw new NotImplementedException("FindIndex must be overridden for this type.");
        }

        protected virtual int FindIndex(Vector2f point)
        {
            int result = -1;
            float num = this.bounds.X + this.bounds.Width / 2f;
            float num2 = this.bounds.Y + this.bounds.Height / 2f;
            bool flag = point.X < num2;
            bool flag2 = point.Y >= num2;
            bool flag3 = point.X < num;
            bool flag4 = point.X >= num;
            if (flag3)
            {
                if (flag)
                {
                    result = 1;
                }
                else if (flag2)
                {
                    result = 2;
                }
            }
            else if (flag4)
            {
                if (flag)
                {
                    result = 0;
                }
                else if (flag2)
                {
                    result = 3;
                }
            }
            return result;
        }

        public virtual void Insert(T obj)
        {
            if (this.nodes[0] != null)
            {
                int num = this.FindIndex(obj);
                if (num != -1)
                {
                    this.nodes[num].Insert(obj);
                    return;
                }
            }
            this.objects.Add(obj);
            if (this.objects.Count > 10 && this.level < 5)
            {
                if (this.nodes[0] == null)
                {
                    this.Split();
                }
                int i = 0;
                while (i < this.objects.Count)
                {
                    int num2 = this.FindIndex(this.objects[i]);
                    if (num2 != -1)
                    {
                        this.nodes[num2].Insert(this.objects[i]);
                        this.objects.RemoveAt(i);
                    }
                    else
                    {
                        i++;
                    }
                }
            }
            if (this.Comparer != null)
            {
                this.objects.Sort(this.Comparer);
            }
        }

        public virtual void Remove(T obj)
        {
            if (this.nodes[0] != null)
            {
                int num = this.FindIndex(obj);
                if (num != -1)
                {
                    this.nodes[num].Remove(obj);
                }
            }
            this.objects.Remove(obj);
        }

        public virtual List<T> Retrieve(T obj)
        {
            List<T> returnList = new List<T>();
            return this.Retrieve(returnList, obj);
        }

        protected List<T> Retrieve(List<T> returnList, T obj)
        {
            int num = this.FindIndex(obj);
            if (this.nodes[0] != null)
            {
                if (num != -1)
                {
                    this.nodes[num].Retrieve(returnList, obj);
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        this.nodes[i].Retrieve(returnList, obj);
                    }
                }
            }
            returnList.AddRange(this.objects);
            return returnList;
        }

        public virtual List<T> Retrieve(Vector2f point)
        {
            List<T> returnList = new List<T>();
            return this.Retrieve(returnList, point);
        }

        protected List<T> Retrieve(List<T> returnList, Vector2f point)
        {
            int num = this.FindIndex(point);
            if (this.nodes[0] != null)
            {
                if (num != -1)
                {
                    this.nodes[num].Retrieve(returnList, point);
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        this.nodes[i].Retrieve(returnList, point);
                    }
                }
            }
            returnList.AddRange(this.objects);
            return returnList;
        }

        public virtual void DebugDraw(RenderTarget target)
        {
            foreach (Quadtree<T> quadtree in this.nodes)
            {
                if (quadtree != null)
                {
                    quadtree.DebugDraw(target);
                }
            }
        }

        private const int MAX_OBJECTS = 10;

        private const int MAX_LEVELS = 5;

        protected int level;

        protected List<T> objects;

        protected Rectangle bounds;

        protected Quadtree<T>[] nodes;
    }
}
