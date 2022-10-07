using System;
using Carbine.Graphics;
using Carbine.Utility;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Overworld
{
	internal class ParallaxBackground : TiledBackground
	{
		public Vector2f Vector
		{
			get
			{
				return this.vector;
			}
			set
			{
				this.vector = value;
			}
		}

		public ParallaxBackground(string sprite, Vector2f vector, IntRect area, int depth) : base(sprite, area, true, true, VectorMath.ZERO_VECTOR, depth)
		{
			this.vector = vector;
			this.areaPoint = new Vector2f((float)area.Left, (float)area.Top);
			this.areaDimensions = new Vector2f((float)area.Width, (float)area.Height);
			this.position = this.areaPoint;
			this.size = this.areaDimensions;
			this.w = this.areaDimensions.X - this.texture.Image.Size.X;
			this.h = this.areaDimensions.Y - this.texture.Image.Size.Y;
			this.tw = (this.texture.Image.Size.X - 320U) / 2f;
			this.th = (this.texture.Image.Size.Y - 180U) / 2f;
			this.Update();
		}

		private void Update()
		{
			float num = ViewManager.Instance.FinalCenter.X - 160f;
			float num2 = ViewManager.Instance.FinalCenter.Y - 90f;
			this.previousPosition = this.position;
			this.position.X = num + (this.areaPoint.X - num) / this.w * (this.vector.X * this.tw);
			this.position.Y = num2 + (this.areaPoint.Y - num2) / this.h * (this.vector.Y * this.th);
			for (int i = 0; i < this.yRepeatCount; i++)
			{
				for (int j = 0; j < this.xRepeatCount; j++)
				{
					this.sprites[j, i].Position += this.position - this.previousPosition;
				}
			}
		}

		public override void Draw(RenderTarget target)
		{
			this.Update();
			base.Draw(target);
		}

		protected override void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				TextureManager.Instance.Unuse(this.texture);
			}
			this.disposed = true;
		}

		private Vector2f previousPosition;

		private Vector2f vector;

		private Vector2f areaPoint;

		private Vector2f areaDimensions;

		private float w;

		private float h;

		private float tw;

		private float th;
	}
}
