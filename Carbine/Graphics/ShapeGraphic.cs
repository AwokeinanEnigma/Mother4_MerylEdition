using SFML.Graphics;
using SFML.System;

namespace Carbine.Graphics
{
    /// <summary>
    /// A graphic that has a specified shape
    /// </summary>
    public class ShapeGraphic : Renderable
    {
		/// <summary>
		/// The shape of the graphic
		/// </summary>
		public Shape Shape
        {
            get
            {
                return this.shape;
            }
        }

        public override Vector2f Position
        {
            get
            {
                return this.shape.Position;
            }
            set
            {
                this.shape.Position = value;
            }
        }

        public override Vector2f Origin
        {
            get
            {
                return this.shape.Origin;
            }
            set
            {
                this.shape.Origin = value;
            }
        }

        public Color FillColor
        {
            get
            {
                return this.shape.FillColor;
            }
            set
            {
                this.shape.FillColor = value;
            }
        }

        public Color OutlineColor
        {
            get
            {
                return this.shape.OutlineColor;
            }
            set
            {
                this.shape.OutlineColor = value;
            }
        }

		/// <summary>
		/// Creates a new shaped graphic 
		/// </summary>
		/// <param name="shape">The shape of this Shapegraphic</param>
		/// <param name="position">The position of the ShapeGraphic</param>
		/// <param name="origin">The origin of the ShapeGraphic</param>
		/// <param name="size">The size of the ShapeGraphoc</param>
		/// <param name="depth">The depth of the ShapeGraphic</param>
		public ShapeGraphic(Shape shape, Vector2f position, Vector2f origin, Vector2f size, int depth)
        {
            this.size = size;
            this.depth = depth;
            this.shape = shape;
            this.shape.Position = position;
            this.shape.Origin = origin;
        }

        public override void Draw(RenderTarget target)
        {
            target.Draw(this.shape);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        private Shape shape;
    }
}
