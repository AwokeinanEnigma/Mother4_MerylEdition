using System;
using Carbine.Graphics;
using Mother4.Data;
using SFML.System;

namespace Mother4.Battle.UI
{
	internal class OdometerRoller : IDisposable
	{
		public event OdometerRoller.RollCompleteHandler OnRollComplete;

		public event OdometerRoller.RollOverHandler OnRollover;

		public Vector2f Position
		{
			get
			{
				return this.roller.Position;
			}
			set
			{
				this.roller.Position = value;
			}
		}

		public int Number
		{
			get
			{
				return this.targetFrame / 9;
			}
			set
			{
				this.targetFrame = value * 9;
			}
		}

		public int CurrentNumber
		{
			get
			{
				return this.frame / 9;
			}
		}

		public OdometerRoller(RenderPipeline pipeline, int initialNumber, Vector2f position, int depth)
		{
			this.pipeline = pipeline;
			this.frame = initialNumber * 9;
			this.targetFrame = this.frame;
			this.roller = new IndexedColorGraphic(Paths.GRAPHICS + "odometer.dat", "odometer", position, depth);
			this.roller.SpeedModifier = 0f;
			this.roller.Frame = (float)(this.frame % 90);
			pipeline.Add(this.roller);
		}

		~OdometerRoller()
		{
			this.Dispose(false);
		}

		public void Hide()
		{
			if (!this.hidden)
			{
				this.roller.Visible = false;
				this.hidden = true;
			}
		}

		public void Show()
		{
			if (this.hidden)
			{
				this.roller.Visible = true;
				this.hidden = false;
			}
		}

		public void DoEntireRoll()
		{
			this.rolling = true;
			this.targetStepFrame = -1;
		}

        private int deadCount;
		public void StepRoll()
        {
            this.rolling = true;
			int num = Math.Max(-1, Math.Min(1, this.targetFrame - this.frame));
			this.targetStepFrame = this.frame + 9 * num;
		}

		public void Update()
		{
			if (this.rolling)
			{
				int num = Math.Max(-1, Math.Min(1, this.targetFrame - this.frame));
				this.frame += num;
				this.roller.Frame = (float)(this.frame % 90);
				if ((num > 0 && this.frame % 90 == 82) || (num < 0 && this.frame % 90 == 89))
				{
					this.rollCount += num;
					if (this.OnRollover != null)
					{
						this.OnRollover();
					}
				}
				if (this.OnRollComplete != null && this.frame == this.targetFrame)
				{
					this.OnRollComplete(frame, hidden, targetFrame);
					this.rolling = false;
				}
				if (this.frame == this.targetStepFrame)
				{
					this.rolling = false;
				}
			}
		}

		public void ForceNumber(int number)
		{
			this.roller.Frame = (float)(number * 9);
			this.targetFrame = (int)this.roller.Frame;
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					this.pipeline.Remove(this.roller);
					this.roller.Dispose();
				}
				this.disposed = true;
			}
		}

		private const int FRAMES_PER_NUMBER = 9;

		private const int TOTAL_FRAMES = 90;

		private bool disposed;

		private RenderPipeline pipeline;

		private Graphic roller;

		private int frame;

		private int targetFrame;

		private int targetStepFrame;

		private int rollCount;

		public bool rolling;

		private bool hidden;

		public delegate void RollCompleteHandler(int frame, bool hidden, int targetFrame);

		public delegate void RollOverHandler();
	}
}
