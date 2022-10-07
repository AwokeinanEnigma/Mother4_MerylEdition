using System;
using Carbine.Flags;
using Carbine.Graphics;
using Carbine.GUI;
using Carbine.Input;
using Rufini.Strings;
using SFML.System;

namespace Mother4.GUI.OverworldMenu
{
	internal class MoneyMenu : MenuPanel
	{
		public MoneyMenu() : base(ViewManager.Instance.FinalTopLeft + MoneyMenu.PANEL_POSITION, MoneyMenu.PANEL_SIZE, 0)
		{
			this.dollarText = new TextRegion(new Vector2f(1f, -1f), 1, Fonts.Main, StringFile.Instance.Get("system.currency").Value);
			base.Add(this.dollarText);
			this.RefreshValue();
		}

		public void RefreshValue()
		{
			int num = ValueManager.Instance[1];
			string text = string.Format("{0:0.00}", num);
			if (this.moneyText == null)
			{
				this.moneyText = new TextRegion(new Vector2f(MoneyMenu.PANEL_SIZE.X - 2f, -1f), 1, Fonts.Main, text);
				base.Add(this.moneyText);
			}
			this.moneyText.Position -= new Vector2f(this.moneyText.Size.X, 0f);
		}

		public override void AxisPressed(Vector2f axis)
		{
		}

		public override object ButtonPressed(Button button)
		{
			return null;
		}

		public override void Focus()
		{
		}

		public override void Unfocus()
		{
		}

		public const int PANEL_DEPTH = 0;

		public static readonly Vector2f PANEL_POSITION = MainMenu.PANEL_POSITION + new Vector2f(0f, MainMenu.PANEL_SIZE.Y + 19f);

		public static readonly Vector2f PANEL_SIZE = new Vector2f(MainMenu.PANEL_SIZE.X, 10f);

		private TextRegion dollarText;

		private TextRegion moneyText;
	}
}
