using System;
using System.Collections.Generic;
using Carbine.Audio;
using Carbine.Graphics;
using Carbine.GUI;
using Carbine.Input;
using Carbine.Utility;
using Mother4.Data;
using Rufini.Strings;
using SFML.System;

namespace Mother4.GUI.NamingMenu
{
	internal class TextInputPanel : MenuPanel
	{
		public bool CursorVisibility
		{
			get
			{
				return this.cursor.Visible;
			}
			set
			{
				this.cursor.Visible = value;
			}
		}

		public TextInputPanel(Vector2f position, Vector2f size) : base(position, size, 0, WindowBox.Style.Normal, 0U)
		{
			this.selectingLetters = true;
			this.SetupSounds();
			this.inputChars = new List<char[][]>();
			this.inputCharTexts = new List<TextRegion[][]>();
			this.inputTitles = new List<string>();
			string qualifiedName = string.Empty;
			string text = string.Empty;
			int num = 0;
			while (text != null)
			{
				qualifiedName = string.Format("input.page{0}", num + 1);
				text = StringFile.Instance.Get(qualifiedName).Value;
				if (text != null && text.Length > 0)
				{
					text = text.Replace("\r", "");
					string[] array = text.Split(new char[]
					{
						'\n'
					});
					char[][] array2 = new char[array.Length][];
					TextRegion[][] array3 = new TextRegion[array.Length][];
					for (int i = 0; i < array.Length; i++)
					{
						int num2 = (int)this.size.X / array[i].Length;
						array2[i] = array[i].ToCharArray();
						array3[i] = new TextRegion[array[i].Length];
						for (int j = 0; j < array2[i].Length; j++)
						{
							TextRegion textRegion = new TextRegion(new Vector2f((float)(8 + j * num2), (float)(8 + i * Fonts.Main.LineHeight)), 1, Fonts.Main, array2[i][j].ToString());
							textRegion.Visible = (this.inputChars.Count == this.selectedPage);
							array3[i][j] = textRegion;
							base.Add(textRegion);
						}
					}
					this.inputChars.Add(array2);
					this.inputCharTexts.Add(array3);
					string qualifiedName2 = string.Format("input.title{0}", num + 1);
					string value = StringFile.Instance.Get(qualifiedName2).Value;
					this.inputTitles.Add(value);
				}
				num++;
			}
			this.cursor = new IndexedColorGraphic(Paths.GRAPHICS + "realcursor.dat", "right", VectorMath.ZERO_VECTOR, 1);
			base.Add(this.cursor);
			this.UpdateCursorPosition();
			this.CreateButtons();
		}

		private void SetupSounds()
		{
			this.sfxCursorX = AudioManager.Instance.Use(Paths.SFXMENU + "cursorx.wav", AudioType.Sound);
			this.sfxCursorY = AudioManager.Instance.Use(Paths.SFXMENU + "cursory.wav", AudioType.Sound);
		}

		private void CreateButtons()
		{
			this.buttonTexts = new TextRegion[]
			{
				new TextRegion(new Vector2f(8f, this.size.Y - (float)Fonts.Main.LineHeight), 1, Fonts.Main, this.inputTitles[(this.selectedPage + 1) % this.inputTitles.Count]),
				new TextRegion(new Vector2f(83f, this.size.Y - (float)Fonts.Main.LineHeight), 1, Fonts.Main, StringFile.Instance.Get("naming.space").Value),
				new TextRegion(new Vector2f(this.size.X - 8f - 115f, this.size.Y - (float)Fonts.Main.LineHeight), 1, Fonts.Main, StringFile.Instance.Get("naming.dontCare").Value),
				new TextRegion(new Vector2f(this.size.X - 8f - 50f, this.size.Y - (float)Fonts.Main.LineHeight), 1, Fonts.Main, StringFile.Instance.Get("naming.back").Value),
				new TextRegion(new Vector2f(this.size.X - 8f - 10f, this.size.Y - (float)Fonts.Main.LineHeight), 1, Fonts.Main, StringFile.Instance.Get("naming.ok").Value)
			};
			foreach (TextRegion control in this.buttonTexts)
			{
				base.Add(control);
			}
		}

		private void SetSelectedPage(int page)
		{
			this.selectedPage = Math.Max(0, Math.Min(this.inputChars.Count - 1, page));
			this.cursorX = 0;
			this.cursorY = 0;
			string text = this.inputTitles[(this.selectedPage + 1) % this.inputTitles.Count];
			this.buttonTexts[0].Reset(text, 0, text.Length);
			for (int i = 0; i < this.inputCharTexts.Count; i++)
			{
				for (int j = 0; j < this.inputCharTexts[i].Length; j++)
				{
					for (int k = 0; k < this.inputCharTexts[i][j].Length; k++)
					{
						this.inputCharTexts[i][j][k].Visible = (i == this.selectedPage);
					}
				}
			}
		}

		private void AdvancePage()
		{
			this.SetSelectedPage((this.selectedPage + 1) % this.inputChars.Count);
			this.UpdateCursorPosition();
		}

		public override object ButtonPressed(Button b)
		{
			char c = '\0';
			if (b == Button.Select)
			{
				this.AdvancePage();
			}
			else if (b == Button.B)
			{
				c = '\b';
			}
			else if (b == Button.A)
			{
				if (this.selectingLetters)
				{
					c = this.inputChars[this.selectedPage][this.cursorY][this.cursorX];
				}
				else
				{
					switch (this.cursorX)
					{
					case 0:
						this.AdvancePage();
						c = '\t';
						break;
					case 1:
						c = ' ';
						break;
					case 2:
						c = '\r';
						break;
					case 3:
						c = '\b';
						break;
					case 4:
						c = '\n';
						break;
					}
				}
			}
			return c;
		}

		private void UpdateCursorPosition()
		{
			if (this.selectingLetters)
			{
				TextRegion textRegion = this.inputCharTexts[this.selectedPage][this.cursorY][this.cursorX];
				this.cursor.Position = textRegion.Position + new Vector2f(-2f, textRegion.Size.Y / 3f);
				return;
			}
			TextRegion textRegion2 = this.buttonTexts[this.cursorX];
			this.cursor.Position = textRegion2.Position + new Vector2f(-2f, textRegion2.Size.Y / 3f);
		}

		public override void AxisPressed(Vector2f axis)
		{
			if (this.selectingLetters)
			{
				if (axis.X != 0f)
				{
					this.cursorX += Math.Sign(axis.X);
					this.sfxCursorX.Play();
					if (this.cursorX < 0)
					{
						this.cursorX = this.inputChars[this.selectedPage][this.cursorY].Length - 1;
					}
					if (this.cursorX > this.inputChars[this.selectedPage][this.cursorY].Length - 1)
					{
						this.cursorX = 0;
					}
				}
				else if (axis.Y != 0f)
				{
					float num = (float)this.cursorX / (float)(this.inputChars[this.selectedPage][this.cursorY].Length - 1);
					this.cursorY += Math.Sign(axis.Y);
					this.sfxCursorY.Play();
					if (this.cursorY < 0 || this.cursorY > this.inputChars[this.selectedPage].Length - 1)
					{
						this.cursorY = Math.Max(0, Math.Min(this.inputChars[this.selectedPage].Length - 1, this.cursorY));
						this.selectingLetters = false;
					}
					this.cursorX = (this.selectingLetters ? ((int)Math.Round((double)(num * (float)(this.inputChars[this.selectedPage][this.cursorY].Length - 1)))) : ((int)Math.Round((double)(num * (float)(this.buttonTexts.Length - 1)))));
				}
			}
			else if (axis.X != 0f)
			{
				this.cursorX += Math.Sign(axis.X);
				this.sfxCursorX.Play();
				if (this.cursorX < 0)
				{
					this.cursorX = this.buttonTexts.Length - 1;
				}
				if (this.cursorX > this.buttonTexts.Length - 1)
				{
					this.cursorX = 0;
				}
			}
			else if (axis.Y != 0f)
			{
				this.selectingLetters = true;
				if (axis.Y > 0f)
				{
					this.cursorY = 0;
				}
				else if (axis.Y < 0f)
				{
					this.cursorY = this.inputChars[this.selectedPage].Length - 1;
				}
				float num2 = (float)this.cursorX / (float)(this.buttonTexts.Length - 1);
				this.cursorX = (int)Math.Round((double)(num2 * (float)(this.inputChars[this.selectedPage][this.cursorY].Length - 1)));
				this.sfxCursorY.Play();
			}
			this.UpdateCursorPosition();
		}

		public override void Focus()
		{
		}

		public override void Unfocus()
		{
		}

		protected override void Dispose(bool disposing)
		{
			if (!this.disposed && disposing)
			{
				foreach (TextRegion textRegion in this.buttonTexts)
				{
					textRegion.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		private const int FLAVOR = 0;

		private const int DEPTH = 0;

		private const int MARGIN = 8;

		private const string INPUT_PAGE_STRING_FORMAT = "input.page{0}";

		private const string PAGE_TITLE_STRING_FORMAT = "input.title{0}";

		private List<char[][]> inputChars;

		private List<TextRegion[][]> inputCharTexts;

		private List<string> inputTitles;

		private TextRegion[] buttonTexts;

		private bool selectingLetters;

		private int selectedPage;

		private Renderable cursor;

		private int cursorX;

		private int cursorY;

		private CarbineSound sfxCursorX;

		private CarbineSound sfxCursorY;
	}
}
