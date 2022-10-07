using System;
using System.Collections.Generic;
using Carbine.Actors;
using Carbine.Graphics;
using Carbine.GUI;
using Carbine.Input;
using Mother4.Data;
using Mother4.GUI;
using Mother4.Scripts.Text;
using SFML.System;

namespace Mother4.Battle.UI
{
	internal class BattleTextbox : Actor
	{
		public event BattleTextbox.CompletionHandler OnTextboxComplete;

		public event BattleTextbox.TextTriggerHandler OnTextTrigger;

		public bool Visible
		{
			get
			{
				return this.visible;
			}
		}

		public BattleTextbox(RenderPipeline pipeline, int colorIndex)
		{
			this.pipeline = pipeline;
			this.typewriterbox = new TypewriterBox(pipeline, BattleTextbox.TEXT_POSITION, BattleTextbox.TEXT_SIZE, 2147450880, Button.A, false, new TextBlock(new List<TextLine>()));
			this.typewriterbox.UseBeeps = false;
			this.window = new WindowBox(Settings.WindowStyle, Settings.WindowFlavor, BattleTextbox.BOX_POSITION, BattleTextbox.BOX_SIZE, 2147450879);
			pipeline.Add(this.window);
			this.arrow = new IndexedColorGraphic(Paths.GRAPHICS + "realcursor.dat", "down", BattleTextbox.BUTTON_POSITION, 2147450880);
			this.arrow.Visible = false;
			pipeline.Add(this.arrow);
			this.visible = false;
			this.showArrow = false;
			this.typewriterbox.OnTypewriterComplete += this.TypewriterComplete;
			this.typewriterbox.OnTextTrigger += this.TextTrigger;
			this.typewriterbox.OnTextWait += this.TextWait;
			InputManager.Instance.ButtonPressed += this.ButtonPressed;
		}

		public void ChangeStyle(WindowBox.Style style) {
			window.FrameStyle = style;
		}

		private void TypewriterComplete()
		{
			this.typewriterFinished = true;
			if (this.useButton)
			{
				this.waitForPlayer = true;
				this.showArrow = true;
				this.arrow.Visible = true;
				return;
			}
			this.waitForTimer = true;
			this.timer = 0;
		}

		private void TextTrigger(TextTrigger trigger)
		{
			if (this.OnTextTrigger != null)
			{
				this.OnTextTrigger(trigger);
			}
		}

		private void TextWait()
		{
			if (this.useButton)
			{
				this.waitForPlayer = true;
				this.showArrow = true;
				this.arrow.Visible = true;
				return;
			}
			this.continueOnTimer = true;
		}

		private void ButtonPressed(InputManager sender, Button b)
		{
			if (this.waitForPlayer && b == Button.A)
			{
				if (!this.typewriterFinished)
				{
					this.typewriterbox.ContinueFromWait();
					this.showArrow = false;
					this.arrow.Visible = false;
					return;
				}
				if (this.OnTextboxComplete != null)
				{
					this.waitForPlayer = false;
					this.OnTextboxComplete();
				}
			}
		}

		public void Reset(string text, bool useButton)
		{
			this.typewriterbox.Reset(TextProcessor.Process(Fonts.Main, text, (int)BattleTextbox.TEXT_SIZE.X));
			this.waitForPlayer = false;
			this.showArrow = false;
			this.waitForTimer = false;
			this.typewriterFinished = false;
			this.timer = 0;
			this.useButton = useButton;
		}

		public void Show()
		{
			if (!this.visible)
			{
				this.visible = true;
				this.window.Visible = true;
				this.typewriterbox.Show();
				if (this.showArrow)
				{
					this.arrow.Visible = true;
				}
			}
		}

		public void Hide()
		{
			if (this.visible)
			{
				this.visible = false;
				this.window.Visible = false;
				this.typewriterbox.Hide();
				if (this.showArrow)
				{
					this.arrow.Visible = false;
				}
			}
		}

		public override void Update()
		{
			base.Update();
			if (this.visible)
			{
				this.typewriterbox.Input();
				this.typewriterbox.Update();
				if (this.waitForTimer && !this.waitForPlayer && !this.continueOnTimer)
				{
					if (this.timer < 45)
					{
						this.timer++;
						return;
					}
					this.waitForTimer = false;
					this.timer = 0;
					if (this.OnTextboxComplete != null)
					{
						this.OnTextboxComplete();
						return;
					}
				}
				else if (this.continueOnTimer)
				{
					if (this.timer < 45)
					{
						this.timer++;
						return;
					}
					this.continueOnTimer = false;
					this.timer = 0;
					this.typewriterbox.ContinueFromWait();
				}
			}
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			this.arrow.Dispose();
			this.window.Dispose();
			this.typewriterbox.OnTypewriterComplete -= this.TypewriterComplete;
			this.typewriterbox.OnTextTrigger -= this.TextTrigger;
			this.typewriterbox.OnTextWait -= this.TextWait;
			InputManager.Instance.ButtonPressed -= this.ButtonPressed;
			this.typewriterbox.Dispose();
		}

		private const Button ADVANCE_BUTTON = Button.A;

		protected const int DEPTH = 2147450880;

		protected const int MESSAGE_WAIT = 45;

		protected static Vector2f BOX_SIZE = new Vector2f(248f, 43f);

		protected static Vector2f BOX_POSITION = new Vector2f((float)(160L - (long)((int)(BattleTextbox.BOX_SIZE.X / 2f))), 0f);

		protected static Vector2f TEXT_POSITION = new Vector2f(BattleTextbox.BOX_POSITION.X, BattleTextbox.BOX_POSITION.Y + 8f);

		protected static Vector2f TEXT_SIZE = new Vector2f(BattleTextbox.BOX_SIZE.X - 20f, BattleTextbox.BOX_SIZE.Y - 8f);

		protected static Vector2f NAMETAG_POSITION = new Vector2f(BattleTextbox.BOX_POSITION.X + 3f, BattleTextbox.BOX_POSITION.Y - 14f);

		protected static Vector2f NAMETEXT_POSITION = new Vector2f(BattleTextbox.NAMETAG_POSITION.X + 5f, BattleTextbox.NAMETAG_POSITION.Y - 1f);

		protected static Vector2f BUTTON_POSITION = new Vector2f(BattleTextbox.BOX_POSITION.X + BattleTextbox.BOX_SIZE.X - 14f, BattleTextbox.BOX_POSITION.Y + BattleTextbox.BOX_SIZE.Y - 8f);

		private RenderPipeline pipeline;

		private TypewriterBox typewriterbox;

		private WindowBox window;

		private Graphic arrow;

		private bool visible;

		private bool showArrow;

		private bool waitForTimer;

		private bool waitForPlayer;

		private bool useButton;

		private bool typewriterFinished;

		private bool continueOnTimer;

		private int timer;

		public delegate void CompletionHandler();

		public delegate void TextTriggerHandler(TextTrigger trigger);
	}
}
