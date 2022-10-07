using System;
using Carbine.Actors;
using Carbine.Graphics;
using SFML.Graphics;

namespace Mother4.Battle.UI
{
	internal class Blinker : Actor
	{
		public event Blinker.CompletionHandler OnComplete;

		public Blinker(Graphic graphic, int blinks)
		{
			this.graphic = graphic;
			graphic.Color = new Color(0, 0, 0, 0);
			this.blinkCap = blinks;
		}

		public override void Update()
		{
			base.Update();
			this.graphic.Color = new Color(0, 0, 0, (byte)((Math.Sin((double)this.timer) + 1.0) / 2.0 * 255.0));
			if ((double)this.timer > 6.283185307179586 * (double)this.blinkCap)
			{
				if (this.OnComplete != null)
				{
					this.OnComplete();
					return;
				}
			}
			else
			{
				this.timer += 0.3926991f;
			}
		}

		private Graphic graphic;

		private int blinkCap;

		private float timer;

		public delegate void CompletionHandler();
	}
}
