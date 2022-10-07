using System;
using Carbine.Audio;
using Carbine.Graphics;
using Carbine.GUI;
using Carbine.Input;
using Mother4.Data;
using SFML.System;

namespace Mother4.GUI
{
	internal class QuestionBox : TextBox
	{
		public event QuestionBox.SelectionHandler OnSelection;

		public QuestionBox(RenderPipeline pipeline, int colorIndex) : base(pipeline, colorIndex)
		{
			this.selectionArrow = new IndexedColorGraphic(Paths.GRAPHICS + "realcursor.dat", "right", QuestionBox.OPTION1_POSITION, 2147450880);
			this.option1 = new TextRegion(default(Vector2f), 2147450880, Fonts.Main, "Option 1");
			this.option2 = new TextRegion(default(Vector2f), 2147450880, Fonts.Main, "Option 2");
			this.optionsLineOffset = 0;
			this.arrowXpos = new float[2];
			this.arrowYpos = new float[2];
			InputManager.Instance.AxisPressed += this.AxisPressed;
			this.moveSound = AudioManager.Instance.Use(Paths.SFXMENU + "cursorx.wav", AudioType.Sound);
			this.selectSound = AudioManager.Instance.Use(Paths.SFXMENU + "confirm.wav", AudioType.Sound);
		}

		protected override void TypewriterComplete()
		{
			base.TypewriterComplete();
			this.ShowOptions();
			this.pipeline.Remove(this.advanceArrow);
		}

		protected override void ButtonPressed(InputManager sender, Button b)
		{
			if (this.textboxWaitForPlayer && b == Button.A)
			{
				this.textboxWaitForPlayer = false;
				if (!this.typewriterDone)
				{
					this.typewriterBox.ContinueFromWait();
					if (!this.hideAdvanceArrow)
					{
						this.arrowVisible = false;
						this.pipeline.Remove(this.advanceArrow);
						return;
					}
				}
				else if (this.typewriterDone && this.OnSelection != null)
				{
					this.selectSound.Play();
					this.OnSelection(this.selection);
				}
			}
		}

		private void AxisPressed(InputManager sender, Vector2f axis)
		{
			if (this.optionsVisible)
			{
				this.selection = ((this.selection == 1) ? 0 : 1);
				this.selectionArrow.Position = new Vector2f(this.arrowXpos[this.selection], this.selectionArrow.Position.Y);
				this.moveSound.Play();
			}
		}

		public void Reset(string text, string namestring, string option1, string option2, bool suppressSlideIn, bool suppressSlideOut)
		{
			base.Reset(text, namestring, suppressSlideIn, suppressSlideOut);
			this.optionsLineOffset = this.typewriterBox.DisplayLines;
			this.option1.Reset(option1, 0, option1.Length);
			this.option2.Reset(option2, 0, option2.Length);
		}

		protected override void Recenter()
		{
			base.Recenter();
			Vector2f finalCenter = ViewManager.Instance.FinalCenter;
			Vector2f v = finalCenter - ViewManager.Instance.View.Size / 2f;
			Vector2f v2 = new Vector2f(0f, (float)(14 * this.optionsLineOffset));
			this.option1.Position = v + QuestionBox.OPTION1_POSITION + v2;
			this.option2.Position = v + QuestionBox.OPTION2_POSITION + v2;
			this.arrowXpos[0] = this.option1.Position.X - 1f;
			this.arrowXpos[1] = this.option2.Position.X - 1f;
			this.arrowYpos[0] = this.option1.Position.Y + (float)Fonts.Main.WHeight - this.selectionArrow.Size.Y / 2f;
			this.arrowYpos[1] = this.option2.Position.Y + (float)Fonts.Main.WHeight - this.selectionArrow.Size.Y / 2f;
			this.selectionArrow.Position = new Vector2f(this.arrowXpos[this.selection], this.arrowYpos[this.selection]);
		}

		private void ShowOptions()
		{
			if (!this.optionsVisible)
			{
				this.pipeline.Add(this.option1);
				this.pipeline.Add(this.option2);
				this.pipeline.Add(this.selectionArrow);
				this.optionsVisible = true;
			}
		}

		public override void Show()
		{
			this.selection = 0;
			if (!this.visible && this.optionsVisible)
			{
				this.pipeline.Add(this.option1);
				this.pipeline.Add(this.option2);
				this.pipeline.Add(this.selectionArrow);
			}
			base.Show();
		}

		public override void Hide()
		{
			if (this.visible && this.optionsVisible)
			{
				this.pipeline.Remove(this.option1);
				this.pipeline.Remove(this.option2);
				this.pipeline.Remove(this.selectionArrow);
				this.optionsVisible = false;
			}
			base.Hide();
		}

		public override void Update()
		{
			base.Update();
		}

		protected override void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					this.selectionArrow.Dispose();
				}
				AudioManager.Instance.Unuse(this.moveSound);
				AudioManager.Instance.Unuse(this.selectSound);
				InputManager.Instance.ButtonPressed -= this.ButtonPressed;
				base.Dispose(disposing);
			}
		}

		private static Vector2f OPTION1_POSITION = new Vector2f(TextBox.TEXT_POSITION.X + 26f, TextBox.TEXT_POSITION.Y);

		private static Vector2f OPTION2_POSITION = new Vector2f(TextBox.TEXT_POSITION.X + 26f + (float)((int)(TextBox.BOX_SIZE.X / 3f)), TextBox.TEXT_POSITION.Y);

		private Graphic selectionArrow;

		private TextRegion option1;

		private TextRegion option2;

		private float[] arrowXpos;

		private float[] arrowYpos;

		private int selection;

		private bool optionsVisible;

		private int optionsLineOffset;

		private CarbineSound moveSound;

		private CarbineSound selectSound;

		public delegate void SelectionHandler(int choice);
	}
}
