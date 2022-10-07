namespace Carbine.Utility
{
    internal class Rectangle
    {
        public float X { get; set; }

        public float Y { get; set; }

        public float Width { get; set; }

        public float Height { get; set; }

        public Rectangle(float x, float y, float width, float height)
        { 
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }
    }
}
