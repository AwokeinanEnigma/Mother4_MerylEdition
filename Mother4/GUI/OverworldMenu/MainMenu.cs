using System;
using Carbine.Graphics;
using Carbine.Input;
using Mother4.Data;
using Rufini.Strings;
using SFML.System;

namespace Mother4.GUI.OverworldMenu
{
	internal class MainMenu : MenuPanel
	{
		public MainMenu() : base(ViewManager.Instance.FinalTopLeft + MainMenu.PANEL_POSITION, MainMenu.PANEL_SIZE, 0)
		{
            this.mainList = new ScrollingList(new Vector2f(8f, 0f), 1, MainMenu.MAIN_ITEMS, MainMenu.MAIN_ITEMS.Length, 14f, MainMenu.PANEL_SIZE.X - 14f, MainMenu.CURSOR_PATH); 
            base.Add(this.mainList);
		}

		public override void AxisPressed(Vector2f axis)
		{
			if (axis.Y < 0f)
			{
				this.mainList.SelectPrevious();
				return;
			}
			if (axis.Y > 0f)
			{
				this.mainList.SelectNext();
			}
		}

		public override object ButtonPressed(Button button)
		{
			int? num = null;
			if (button == Button.A)
			{
				num = new int?(this.mainList.SelectedIndex);
			}
			else if (button == Button.B || button == Button.Start)
			{
				num = new int?(-1);
			}
			return num;
		}

		public override void Focus()
		{
			this.mainList.Focused = true;
		}

		public override void Unfocus()
		{
			this.mainList.Focused = false;
		}

        public const int PANEL_DEPTH = 0;

        private static readonly string CURSOR_PATH = Paths.GRAPHICS + "cursor.dat";

        public static readonly Vector2f PANEL_POSITION = new Vector2f(4f, 4f);

        public static readonly Vector2f PANEL_SIZE = new Vector2f(73f, 57f);

        private static readonly string[] MAIN_ITEMS = new string[]
        {
            StringFile.Instance.Get("menu.goods").Value,
            StringFile.Instance.Get("menu.psi").Value,
            StringFile.Instance.Get("menu.status").Value,
            StringFile.Instance.Get("menu.map").Value
        };

        private ScrollingList mainList;
	}
}
