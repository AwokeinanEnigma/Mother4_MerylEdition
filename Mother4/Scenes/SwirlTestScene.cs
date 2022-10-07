using System;
using Carbine;
using Carbine.Input;
using Mother4.Overworld;
using SFML.Graphics;

namespace Mother4.Scenes
{
	internal class SwirlTestScene : StandardScene
	{
		public SwirlTestScene()
		{
			this.swirl = new BattleSwirlOverlay(BattleSwirlOverlay.Style.Green, 0, 0.01f);
			this.swirl.OnAnimationComplete += this.AnimationComplete;
			this.swirl.Visible = true;
			this.pipeline.Add(this.swirl);
		}

		private void AnimationComplete(BattleSwirlOverlay anim)
		{
		}

		private void ButtonPressed(InputManager sender, Button b)
		{
			if (b == Button.A)
			{
				this.swirl.Reset();
				this.swirl.Visible = true;
			}
		}

		public override void Focus()
		{
			base.Focus();
			InputManager.Instance.ButtonPressed += this.ButtonPressed;
			Engine.ClearColor = Color.Magenta;
		}

		public override void Update()
		{
			base.Update();
		}

		public override void Unfocus()
		{
			base.Unfocus();
			InputManager.Instance.ButtonPressed -= this.ButtonPressed;
		}

		protected override void Dispose(bool disposing)
		{
			if (!this.disposed && disposing)
			{
				this.swirl.Dispose();
			}
			base.Dispose(disposing);
		}

		private BattleSwirlOverlay swirl;
	}
}
