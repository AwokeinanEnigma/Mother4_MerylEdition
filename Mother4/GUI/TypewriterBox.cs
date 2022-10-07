using System;
using Carbine.Actors;
using Carbine.Audio;
using Carbine.Graphics;
using Carbine.GUI;
using Carbine.Input;
using Carbine.Utility;
using Mother4.Data;
using Mother4.Scripts.Text;
using SFML.System;

namespace Mother4.GUI
{
	internal class TypewriterBox : Actor
	{
		public event TypewriterBox.TypewriterCompleteHandler OnTypewriterComplete;

		public event TypewriterBox.WaitCommandHandler OnTextWait;

		public event TypewriterBox.TriggerCommandHandler OnTextTrigger;

		public bool UseBeeps
		{
			get
			{
				return this.useBeeps;
			}
			set
			{
				this.useBeeps = value;
			}
		}

		public int DisplayLines
		{
			get
			{
				return Math.Min(3, this.textBlock.Lines.Count);
			}
		}

		public override Vector2f Position
		{
			get
			{
				return this.position;
			}
			set
			{
				this.Reposition(value);
			}
		}

		public TypewriterBox(RenderPipeline pipeline, Vector2f position, Vector2f size, int depth, Button advance, bool showBullets, TextBlock textBlock)
		{
			this.pipeline = pipeline;
			this.position = position;
			this.depth = depth;
			this.advance = advance;
			this.showBullets = showBullets;
			this.textBlock = textBlock;
			this.origTextSpeed = (float)Settings.TextSpeed / 2f;
			this.textSpeed = this.origTextSpeed;
			int num = (int)(size.Y / 14f);
			this.texts = new TextRegion[num];
			for (int i = 0; i < this.texts.Length; i++)
			{
				this.texts[i] = new TextRegion(this.position + new Vector2f(8f, (float)(14 * i)), this.depth + 1, Fonts.Main, (i < this.textBlock.Lines.Count) ? this.textBlock.Lines[i].Text : string.Empty, 0, 0);
				this.pipeline.Add(this.texts[i]);
			}
			this.SetUpBullets();
			this.topLineIndex = 0;
			this.currentLineIndex = 0;
			this.currentTextIndex = 0;
			this.currentText = this.texts[this.currentTextIndex];
			this.textPos = 0;
			this.textLen = this.currentText.Text.Length;
			this.textBeep = AudioManager.Instance.Use(Paths.SFXTEXT + "text1.wav", AudioType.Sound);
			this.useBeeps = true;
		}

		private void SetUpBullets()
		{
			if (this.showBullets)
			{
				this.bulletVisibility = new bool[this.texts.Length];
				this.bullets = new Graphic[this.texts.Length];
				for (int i = 0; i < this.bullets.Length; i++)
				{
					this.bullets[i] = new IndexedColorGraphic(Paths.GRAPHICS + "bullet.dat", "bullet", this.position + new Vector2f(0f, (float)(4 + 14 * i)), this.depth + 1);
					this.pipeline.Add(this.bullets[i]);
				}
				this.SetBulletVisibility();
				return;
			}
			this.bullets = new Graphic[0];
		}

		private void SetBulletVisibility()
		{
			for (int i = 0; i < this.bullets.Length; i++)
			{
				this.bulletVisibility[i] = (this.topLineIndex + i < this.textBlock.Lines.Count && this.texts[i].Length > 0 && this.textBlock.Lines[this.topLineIndex + i].HasBullet);
				this.bullets[i].Visible = (this.showBullets && this.bulletVisibility[i]);
			}
		}

		public void Reposition(Vector2f newPosition)
		{
			this.position = VectorMath.Truncate(newPosition);
			for (int i = 0; i < this.texts.Length; i++)
			{
				this.texts[i].Position = this.position + new Vector2f(8f, (float)(14 * i));
				if (this.showBullets)
				{
					this.bullets[i].Position = this.position + new Vector2f(0f, (float)(4 + 14 * i));
				}
			}
		}

		public void Reset(TextBlock textBlock)
		{
			this.textBlock = textBlock;
			this.topLineIndex = 0;
			this.currentLineIndex = 0;
			this.currentTextIndex = 0;
			this.currentText = this.texts[this.currentTextIndex];
			for (int i = 0; i < this.texts.Length; i++)
			{
				this.texts[i].Reset((i < this.textBlock.Lines.Count) ? this.textBlock.Lines[i].Text : string.Empty, 0, 0);
			}
			this.SetUpBullets();
			this.totalCharCount = 0f;
			this.pauseTimer = 0;
			this.pauseDuration = 0;
			this.commandIndex = 0;
			this.paused = false;
			this.waiting = false;
			this.nextCharWaiter = 0f;
			this.textPos = 0;
			this.textLen = this.currentText.Text.Length;
			this.finshed = false;
		}

		public void Show()
		{
			if (!this.visible)
			{
				this.visible = true;
				for (int i = 0; i < this.texts.Length; i++)
				{
					this.texts[i].Visible = true;
					if (this.showBullets)
					{
						this.bullets[i].Visible = this.bulletVisibility[i];
					}
				}
			}
		}

		public void Hide()
		{
			if (this.visible)
			{
				this.visible = false;
				for (int i = 0; i < this.texts.Length; i++)
				{
					this.texts[i].Visible = false;
					if (this.showBullets)
					{
						this.bullets[i].Visible = false;
					}
				}
			}
		}

		public void ContinueFromWait()
		{
			this.waiting = false;
		}

		public override void Input()
		{
			if (InputManager.Instance.State[this.advance])
			{
				this.textSpeed = this.origTextSpeed + 0.5f;
				return;
			}
			this.textSpeed = this.origTextSpeed;
		}

		public override void Update()
		{
			if (!this.visible)
			{
				return;
			}
			this.SetBulletVisibility();
			this.currentText.Length = Math.Min(this.textLen, this.textPos + 1);
			if (!this.waiting)
			{
				if (this.paused)
				{
					if (this.pauseTimer < this.pauseDuration)
					{
						this.pauseTimer++;
						return;
					}
					this.pauseTimer = 0;
					this.pauseDuration = 0;
					this.paused = false;
					return;
				}
				else if (!this.paused)
				{
					if (this.textPos < this.textLen)
					{
						this.nextCharWaiter += this.textSpeed;
						if (this.nextCharWaiter >= 1f)
						{
							int num = 0;
							int num2 = (int)this.nextCharWaiter;
							while (num < num2 && !this.paused)
							{
								this.textPos++;
								this.totalCharCount += 1f;
								this.HandleCommands();
								num++;
							}
							this.nextCharWaiter = 0f;
							if (this.useBeeps && this.totalCharCount % 3f == 0f)
							{
								this.textBeep.Play();
								return;
							}
						}
					}
					else if (!this.finshed)
					{
						this.HandleCommands();
						if (this.currentTextIndex < this.texts.Length - 1)
						{
							this.currentTextIndex++;
							this.currentText = this.texts[this.currentTextIndex];
							this.textPos = 0;
							this.commandIndex = 0;
							this.textLen = this.currentText.Text.Length;
							this.currentLineIndex++;
							return;
						}
						if (this.currentLineIndex < this.textBlock.Lines.Count - 1)
						{
							for (int i = 1; i < this.texts.Length; i++)
							{
								this.texts[i - 1].Reset(this.texts[i].Text, this.texts[i].Index, this.texts[i].Length);
							}
							this.topLineIndex++;
							this.currentLineIndex++;
							this.texts[this.currentTextIndex].Reset(this.textBlock.Lines[this.currentLineIndex].Text, 0, 0);
							this.currentText = this.texts[this.currentTextIndex];
							this.textPos = 0;
							this.commandIndex = 0;
							this.textLen = this.currentText.Text.Length;
							return;
						}
						if (this.OnTypewriterComplete != null)
						{
							this.OnTypewriterComplete();
						}
						this.finshed = true;
					}
				}
			}
		}

		private bool HandleCommands()
		{
			bool result = false;
			if (this.currentLineIndex < this.textBlock.Lines.Count && this.textBlock.Lines[this.currentLineIndex].Commands.Length > 0)
			{
				while (!this.paused && this.commandIndex < this.textBlock.Lines[this.currentLineIndex].Commands.Length && this.totalCharCount >= (float)this.textBlock.Lines[this.currentLineIndex].Commands[this.commandIndex].Position)
				{
					ITextCommand textCommand = this.textBlock.Lines[this.currentLineIndex].Commands[this.commandIndex];
					if (textCommand is TextPause)
					{
						this.pauseDuration = (textCommand as TextPause).Duration;
						this.paused = true;
					}
					else if (textCommand is TextWait)
					{
						this.waiting = true;
						if (this.OnTextWait != null)
						{
							this.OnTextWait();
						}
					}
					else if (textCommand is TextTrigger && this.OnTextTrigger != null)
					{
						this.OnTextTrigger(textCommand as TextTrigger);
					}
					this.commandIndex++;
					result = true;
				}
			}
			return result;
		}

		protected override void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				AudioManager.Instance.Unuse(this.textBeep);
			}
			base.Dispose(disposing);
		}

		public const int LINE_HEIGHT = 14;

		public const int BULLET_MARGIN = 8;

		public const int TEXT_OFFSET_Y = 0;

		public const int BULLET_OFFSET_Y = 4;

		private int depth;

		private Button advance;

		private int textPos;

		private int textLen;

		private bool finshed;

		private bool visible;

		private TextBlock textBlock;

		private CarbineSound textBeep;

		private bool useBeeps;

		private RenderPipeline pipeline;

		private int topLineIndex;

		private int currentLineIndex;

		private int currentTextIndex;

		private TextRegion currentText;

		private TextRegion[] texts;

		private Graphic[] bullets;

		private bool[] bulletVisibility;

		private bool showBullets;

		private float totalCharCount;

		private int pauseTimer;

		private int pauseDuration;

		private int commandIndex;

		private bool paused;

		private bool waiting;

		private float textSpeed;

		private float origTextSpeed;

		private float nextCharWaiter;

		public delegate void TypewriterCompleteHandler();

		public delegate void WaitCommandHandler();

		public delegate void TriggerCommandHandler(TextTrigger trigger);
	}
}
