using System;
using Carbine.Graphics;
using SFML.System;

namespace Mother4.GUI.Modifiers
{
	internal class GraphicBouncer : IGraphicModifier, IDisposable
	{
		public bool Done
		{
			get
			{
				return this.done;
			}
		}

		public Graphic Graphic
		{
			get
			{
				return this.graphic;
			}
		}

		public GraphicBouncer(Graphic graphic, GraphicBouncer.SpringMode mode, Vector2f amplitude, Vector2f speed, Vector2f decay)
		{
			this.graphic = graphic;
			this.position = graphic.Position;
			this.SetSpring(mode, amplitude, speed, decay);
		}

		public void SetSpring(GraphicBouncer.SpringMode mode, Vector2f amplitude, Vector2f speed, Vector2f decay)
		{
			this.springMode = mode;
			this.xSpring = 0f;
			this.xDampTarget = amplitude.X;
			this.xSpeedTarget = speed.X;
			this.xDecayTarget = decay.X;
			this.ySpring = 0f;
			this.yDampTarget = amplitude.Y;
			this.ySpeedTarget = speed.Y;
			this.yDecayTarget = decay.Y;
			this.ramping = true;
		}

		private void UpdateSpring()
		{
			if (this.ramping)
			{
				this.xDamp += (this.xDampTarget - this.xDamp) / 2f;
				this.xSpeed += (this.xSpeedTarget - this.xSpeed) / 2f;
				this.xDecay += (this.xDecayTarget - this.xDecay) / 2f;
				this.yDamp += (this.yDampTarget - this.yDamp) / 2f;
				this.ySpeed += (this.ySpeedTarget - this.ySpeed) / 2f;
				this.yDecay += (this.yDecayTarget - this.yDecay) / 2f;
				if ((int)this.xDamp == (int)this.xDampTarget && (int)this.xSpeed == (int)this.xSpeedTarget && (int)this.xDecay == (int)this.xDecayTarget && (int)this.yDamp == (int)this.yDampTarget && (int)this.ySpeed == (int)this.ySpeedTarget && (int)this.yDecay == (int)this.yDecayTarget)
				{
					this.ramping = false;
				}
			}
			else
			{
				this.xDamp = ((this.xDamp > 0.5f) ? (this.xDamp * this.xDecay) : 0f);
				this.yDamp = ((this.yDamp > 0.5f) ? (this.yDamp * this.yDecay) : 0f);
			}
			this.xSpring += this.xSpeed;
			this.ySpring += this.ySpeed;
			this.offset.X = (float)Math.Sin((double)this.xSpring) * this.xDamp;
			this.offset.Y = (float)Math.Sin((double)this.ySpring) * this.yDamp;
			if (this.springMode == GraphicBouncer.SpringMode.BounceUp)
			{
				this.offset.Y = -Math.Abs(this.offset.Y);
				return;
			}
			if (this.springMode == GraphicBouncer.SpringMode.BounceDown)
			{
				this.offset.Y = Math.Abs(this.offset.Y);
			}
		}

		public void Update()
		{
			if (!this.done)
			{
				this.UpdateSpring();
				if (this.xDamp > 0.5f || this.yDamp > 0.5f)
				{
					this.graphic.Position = this.position + this.offset;
					return;
				}
				this.done = true;
				this.graphic.Position = this.position;
			}
		}
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed;

        protected void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    
                }
                this.disposed = true;
            }
        }

		private const float DAMP_HIGHPASS = 0.5f;

		private bool done;

		private Graphic graphic;

		private GraphicBouncer.SpringMode springMode;

		private Vector2f position;

		private Vector2f offset;

		private float xSpring;

		private float ySpring;

		private float xSpeed;

		private float xSpeedTarget;

		private float ySpeed;

		private float ySpeedTarget;

		private float xDamp;

		private float xDampTarget;

		private float yDamp;

		private float yDampTarget;

		private float xDecay;

		private float xDecayTarget;

		private float yDecay;

		private float yDecayTarget;

		private bool ramping;

		public enum SpringMode
		{
			Normal,
			BounceUp,
			BounceDown
		}
	}
}
