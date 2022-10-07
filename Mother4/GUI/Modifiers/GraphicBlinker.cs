using System;
using Carbine.Graphics;

namespace Mother4.GUI.Modifiers
{
	internal class GraphicBlinker : IGraphicModifier
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

		public GraphicBlinker(Graphic graphic, int duration, int count)
		{
			this.graphic = graphic;
			this.initialVisibility = graphic.Visible;
			this.duration = duration;
			this.total = count * 2;
		}

		public void Update()
		{
			if (this.count >= this.total && this.total >= 0)
			{
				if (!this.done)
				{
					this.graphic.Visible = this.initialVisibility;
					this.done = true;
				}
				return;
			}
			if (this.timer < this.duration)
			{
				this.timer++;
				return;
			}
			this.count++;
			this.graphic.Visible = !this.graphic.Visible;
			this.timer = 0;
		}

		private Graphic graphic;

		private bool initialVisibility;

		private int count;

		private int total;

		private int timer;

		private int duration;

		private bool done;
	}
}
