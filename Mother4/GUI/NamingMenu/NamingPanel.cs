using System;
using Carbine.Graphics;
using Carbine.GUI;
using Carbine.Input;
using Carbine.Utility;
using Mother4.Data;
using Rufini.Strings;
using SFML.Graphics;
using SFML.System;

namespace Mother4.GUI.NamingMenu
{
	internal class NamingPanel : MenuPanel
	{
		public string Description
		{
			get
			{
				return this.descriptionText.Text;
			}
			set
			{
				this.SetDescription(value);
			}
		}

		public string Name
		{
			get
			{
				return this.nameText.Text;
			}
			set
			{
				this.SetName(value);
			}
		}

		public int NameWidth
		{
			get
			{
				return (int)this.nameText.Size.X;
			}
		}

		public NamingPanel(Vector2f position, Vector2f size) : base(position, size, 1, WindowBox.Style.Normal, 0U)
		{
			this.descriptionText = new TextRegion(new Vector2f(2f, 0f), 1, Fonts.Main, string.Empty);
			base.Add(this.descriptionText);
			string value = StringFile.Instance.Get("naming.prompt").Value;
			this.promptText = new TextRegion(new Vector2f(2f, (float)Fonts.Main.LineHeight), 1, Fonts.Main, value);
			base.Add(this.promptText);
			RectangleShape rectangleShape = new RectangleShape(new Vector2f(52f, (float)(Fonts.Main.LineHeight - 4)));
			rectangleShape.FillColor = UIColors.HighlightColor;
			this.textbox1 = new ShapeGraphic(rectangleShape, new Vector2f(4f + this.promptText.Size.X, (float)(Fonts.Main.LineHeight + 1)), VectorMath.ZERO_VECTOR, rectangleShape.Size, 1);
			base.Add(this.textbox1);
			RectangleShape rectangleShape2 = new RectangleShape(new Vector2f(50f, (float)(Fonts.Main.LineHeight - 2)));
			rectangleShape2.FillColor = UIColors.HighlightColor;
			this.textbox2 = new ShapeGraphic(rectangleShape2, new Vector2f(5f + this.promptText.Size.X, (float)Fonts.Main.LineHeight), VectorMath.ZERO_VECTOR, rectangleShape2.Size, 1);
			base.Add(this.textbox2);
			RectangleShape rectangleShape3 = new RectangleShape(new Vector2f(1f, (float)(Fonts.Main.LineHeight - 4)));
			rectangleShape3.FillColor = Color.Black;
			this.cursor = new ShapeGraphic(rectangleShape3, new Vector2f(8f + this.promptText.Size.X, (float)(Fonts.Main.LineHeight + 1)), VectorMath.ZERO_VECTOR, rectangleShape3.Size, 4);
			base.Add(this.cursor);
			this.nameText = new TextRegion(new Vector2f(6f + this.promptText.Size.X, (float)Fonts.Main.LineHeight), 2, Fonts.Main, string.Empty);
			this.nameText.Color = Color.Black;
			base.Add(this.nameText);
			this.cursorTimerIndex = TimerManager.Instance.StartTimer(30);
			TimerManager.Instance.OnTimerEnd += this.CursorTimerEnd;
		}

		private void CursorTimerEnd(int timerIndex)
		{
			if (timerIndex == this.cursorTimerIndex)
			{
				this.cursor.Visible = !this.cursor.Visible;
				this.cursorTimerIndex = TimerManager.Instance.StartTimer(30);
			}
		}

		private void SetDescription(string description)
		{
			this.descriptionText.Reset(description, 0, description.Length);
		}

		private void SetName(string name)
		{
			this.nameText.Reset(name, 0, name.Length);
			this.cursor.Position = this.nameText.Position + new Vector2f(this.nameText.Size.X + 1f, 1f);
		}

		public override object ButtonPressed(Button button)
		{
			return null;
		}

		public override void AxisPressed(Vector2f axis)
		{
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
				this.descriptionText.Dispose();
				this.promptText.Dispose();
				this.nameText.Dispose();
				this.cursor.Dispose();
				this.textbox1.Dispose();
				this.textbox2.Dispose();
			}
			base.Dispose(disposing);
		}

		private const string PROMPT_STRING = "naming.prompt";

		private const int FLAVOR = 0;

		private const int DEPTH = 1;

		private TextRegion descriptionText;

		private TextRegion promptText;

		private TextRegion nameText;

		private ShapeGraphic cursor;

		private ShapeGraphic textbox1;

		private ShapeGraphic textbox2;

		private int cursorTimerIndex;
	}
}
