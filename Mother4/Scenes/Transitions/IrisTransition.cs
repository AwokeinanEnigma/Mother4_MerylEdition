using System;
using Carbine;
using Carbine.Graphics;
using Carbine.Scenes.Transitions;
using Mother4.GUI;
using SFML.System;

namespace Mother4.Scenes.Transitions
{
	internal class IrisTransition : ITransition
	{
		public bool IsComplete
		{
			get
			{
				return this.isComplete;
			}
		}

		public float Progress
		{
			get
			{
				return this.progress;
			}
		}

		public bool ShowNewScene
		{
			get
			{
				return this.progress > 0.5f;
			}
		}

		public bool Blocking { get; set; }

		public IrisTransition(float duration)
		{
			Vector2f origin = new Vector2f(160f, 90f);
			this.overlay = new IrisOverlay(ViewManager.Instance.FinalCenter, origin, 0f);
			float num = 60f * duration;
			this.speed = 1f / num;
			this.holdFrames = 30;
		}

		public void Update()
		{
			if (!this.isComplete)
			{
				if (this.progress < 0.5f)
				{
					float num = 1f - this.progress / 0.5f;
					this.overlay.Progress = (float)(-(float)Math.Cos((double)num * 3.141592653589793) + 1.0) / 2f;
				}
				else if (this.holdTimer < this.holdFrames)
				{
					this.holdTimer++;
				}
				else
				{
					float num2 = (this.progress - 0.5f) / 0.5f;
					this.overlay.Progress = (float)(-(float)Math.Cos((double)num2 * 3.141592653589793) + 1.0) / 2f;
				}
				if (this.holdTimer == 0 || this.holdTimer >= this.holdFrames)
				{
					if (this.progress < 1f)
					{
						this.progress += this.speed;
						return;
					}
					this.isComplete = true;
				}
			}
		}

		public void Draw()
		{
			this.overlay.Draw(Engine.FrameBuffer);
		}

		public void Reset()
		{
			this.isComplete = false;
			this.progress = 0f;
			this.overlay.Progress = 1f;
		}

		private bool isComplete;

		private float progress;

		private float speed;

		private int holdTimer;

		private int holdFrames;

		private IrisOverlay overlay;
	}
}
