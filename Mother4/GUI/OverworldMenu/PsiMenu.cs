using System;
using System.Collections.Generic;
using System.Linq;
using Carbine.Graphics;
using Carbine.GUI;
using Carbine.Input;
using Carbine.Utility;
using Mother4.Battle;
using Mother4.Data;
using Mother4.Psi;
using Rufini.Strings;
using SFML.Graphics;
using SFML.System;

namespace Mother4.GUI.OverworldMenu
{
	internal class PsiMenu : MenuPanel
	{
		public PsiMenu() : base(ViewManager.Instance.FinalTopLeft + PsiMenu.PANEL_POSITION, PsiMenu.PANEL_SIZE, 0)
		{
			Console.Write("create");

			RectangleShape rectangleShape = new RectangleShape(new Vector2f(1f, PsiMenu.PANEL_SIZE.Y * 0.6f));
			rectangleShape.FillColor = PsiMenu.DIVIDER_COLOR;
			this.vertDivider = new ShapeGraphic(rectangleShape, new Vector2f(PsiMenu.PANEL_SIZE.X * 0.33f, PsiMenu.PANEL_SIZE.Y * 0.3f), VectorMath.Truncate(rectangleShape.Size / 2f), rectangleShape.Size, 1);
			base.Add(this.vertDivider);
			RectangleShape rectangleShape2 = new RectangleShape(new Vector2f(PsiMenu.PANEL_SIZE.X, 1f));
			rectangleShape2.FillColor = PsiMenu.DIVIDER_COLOR;
			this.horizDivider = new ShapeGraphic(rectangleShape2, new Vector2f(PsiMenu.PANEL_SIZE.X * 0.5f, PsiMenu.PANEL_SIZE.Y * 0.66f), VectorMath.Truncate(rectangleShape2.Size / 2f), rectangleShape2.Size, 1);
			base.Add(this.horizDivider);
			CharacterType[] array = PartyManager.Instance.ToArray();
			this.tabs = new IndexedColorGraphic[array.Length];
			this.tabLabels = new TextRegion[array.Length];
			uint num = Settings.WindowFlavor * 2U;
			int num2 = 0;
			for (int i = 0; i < array.Length; i++)
			{
				if (PsiManager.Instance.CharacterHasPsi(array[i]))
				{
					this.tabs[num2] = new IndexedColorGraphic(Paths.GRAPHICS + "pause.dat", (num2 == this.selectedTab) ? "firsttag" : "tag", new Vector2f(-8f, -7f) + new Vector2f(50f * (float)num2, 0f), (num2 == this.selectedTab) ? 1 : -2);
					this.tabs[num2].CurrentPalette = ((num2 == this.selectedTab) ? num : (num + 1U));
					base.Add(this.tabs[num2]);
					this.tabLabels[num2] = new TextRegion(new Vector2f(-4f, -21f) + new Vector2f(50f * (float)num2, 0f), (num2 == this.selectedTab) ? 2 : -1, Fonts.Main, CharacterNames.GetName(array[i]));
					this.tabLabels[num2].Color = ((num2 == this.selectedTab) ? PsiMenu.ACTIVE_TAB_TEXT_COLOR : PsiMenu.INACTIVE_TAB_TEXT_COLOR);
					base.Add(this.tabLabels[num2]);
					num2++;
				}
			}
			Array.Resize<IndexedColorGraphic>(ref this.tabs, num2);
			Array.Resize<TextRegion>(ref this.tabLabels, num2);
			this.psiTypeList = new ScrollingList(new Vector2f(8f, 0f), 0, PsiMenu.PSI_TYPE_STRINGS, 4, 14f, 50f, PsiMenu.CURSOR_FILE);
			base.Add(this.psiTypeList);
			this.selectedList = this.psiTypeList;
			this.SetupPsiList();
			this.descriptionText = new TextRegion(new Vector2f(8f, (float)((int)(PsiMenu.PANEL_SIZE.Y * 0.66f) + 4)), 0, Fonts.Main, this.GetDescription());
			base.Add(this.descriptionText);
		}

		private string GetDescription()
		{
			string text = null;
			if (this.selectedList != this.psiTypeList && this.selectedList == this.psiList)
			{
				string arg = this.psiList.SelectedItem.Replace(" ", "").ToLower();
				string str = string.Format("{0}{1}", arg, this.selectedLevel + 1);
				text = StringFile.Instance.Get("psiDesc." + str).Value;
			}
			if (text == null)
			{
				text = string.Empty;
			}
			return text;
		}

		private void ChangeSelectedLevel(int newLevel)
		{
			this.levelList[this.selectedLevel].ShowCursor = false;
			this.selectedLevel = newLevel;
			this.levelList[this.selectedLevel].ShowCursor = true;
		}

		private void UpdateDescription()
		{
			string description = this.GetDescription();
			this.descriptionText.Reset(description, 0, description.Length);
		}

		private void SelectPsiList()
		{
			this.psiTypeList.ShowSelectionRectangle = false;
			this.psiTypeList.ShowCursor = false;
			this.psiTypeList.Focused = false;
			this.selectedList = this.psiList;
			this.UpdateDescription();
			this.selectedList.ShowSelectionRectangle = true;
			this.selectedList.Focused = true;
			for (int i = 0; i < this.levelList.Length; i++)
			{
				this.levelList[i].ShowSelectionRectangle = true;
				this.levelList[i].Focused = true;
			}
		}

		private void SelectPsiTypeList()
		{
			if (this.psiList != null)
			{
				this.psiList.SelectedIndex = 0;
				this.psiList.ShowSelectionRectangle = false;
				this.psiList.Focused = false;
				for (int i = 0; i < this.levelList.Length; i++)
				{
					this.levelList[i].SelectedIndex = 0;
					this.levelList[i].ShowSelectionRectangle = false;
					this.levelList[i].Focused = false;
				}
			}
			this.selectedList = this.psiTypeList;
			this.UpdateDescription();
			this.selectedList.ShowSelectionRectangle = true;
			this.selectedList.ShowCursor = true;
			this.selectedList.Focused = true;
			Console.Write("list");

		}

		public override void AxisPressed(Vector2f axis)
		{
			if (axis.Y < 0f)
			{
				int selectedIndex = this.selectedList.SelectedIndex;
				this.selectedList.SelectPrevious();
				if (selectedIndex != this.selectedList.SelectedIndex)
				{
					if (this.selectedList == this.psiTypeList)
					{
						this.SetupPsiList();
					}
					else if (this.selectedList == this.psiList)
					{
						for (int i = 0; i < this.levelList.Length; i++)
						{
							this.levelList[i].SelectPrevious();
						}
						while (this.levelList[this.selectedLevel].SelectedItem.Length == 0)
						{
							this.ChangeSelectedLevel(this.selectedLevel - 1);
						}
					}
					this.UpdateDescription();
					return;
				}
			}
			else if (axis.Y > 0f)
			{
				int selectedIndex2 = this.selectedList.SelectedIndex;
				this.selectedList.SelectNext();
				if (selectedIndex2 != this.selectedList.SelectedIndex)
				{
					if (this.selectedList == this.psiTypeList)
					{
						this.SetupPsiList();
					}
					else if (this.selectedList == this.psiList)
					{
						for (int j = 0; j < this.levelList.Length; j++)
						{
							this.levelList[j].SelectNext();
						}
						while (this.levelList[this.selectedLevel].SelectedItem.Length == 0)
						{
							this.ChangeSelectedLevel(this.selectedLevel - 1);
						}
					}
					this.UpdateDescription();
					return;
				}
			}
			else if (axis.X != 0f)
			{
				if (this.selectedList == this.psiTypeList)
				{
					if (axis.X > 0f)
					{
						if (this.psiList != null)
						{
							this.SelectPsiList();
							return;
						}
					}
					else if (axis.X < 0f)
					{
						return;
					}
				}
				else if (this.selectedList == this.psiList)
				{
					if (axis.X < 0f)
					{
						if (this.selectedLevel <= 0)
						{
							this.SelectPsiTypeList();
							return;
						}
						this.ChangeSelectedLevel(this.selectedLevel - 1);
						this.UpdateDescription();
						return;
					}
					else if (axis.X > 0f)
					{
						int num = Math.Min(this.levelList.Length - 1, this.selectedLevel + 1);
						if (this.levelList[num].SelectedItem.Length > 0)
						{
							this.ChangeSelectedLevel(num);
							this.UpdateDescription();
						}
					}
				}
			}
		}

		private void SetupPsiList()
		{
			CharacterType[] array = PartyManager.Instance.ToArray();
			CharacterType characterType = array[this.selectedTab];
			IEnumerable<IPsi> collection;
			switch (this.psiTypeList.SelectedIndex)
			{
			case 1:
				collection = PsiManager.Instance.GetCharacterAssistPsi(characterType).Cast<IPsi>();
				break;
			case 2:
				collection = PsiManager.Instance.GetCharacterDefensePsi(characterType).Cast<IPsi>();
				break;
			case 3:
				collection = PsiManager.Instance.GetCharacterOtherPsi(characterType).Cast<IPsi>();
				break;
			default:
				collection = PsiManager.Instance.GetCharacterOffensePsi(characterType).Cast<IPsi>();
				break;
			}
			this.psiItemList = new List<IPsi>(collection);
			if (this.psiList != null)
			{
				base.Remove(this.psiList);
				this.psiList.Dispose();
				this.psiList = null;
			}
			if (this.levelList != null)
			{
				for (int i = 0; i < this.levelList.Length; i++)
				{
					if (this.levelList[i] != null)
					{
						base.Remove(this.levelList[i]);
						this.levelList[i].Dispose();
						this.levelList[i] = null;
					}
				}
			}
			else
			{
				this.levelList = new ScrollingList[4];
			}
			if (this.psiItemList.Count > 0)
			{
				Console.Write("psi>0");
				StatSet stats = CharacterStats.GetStats(characterType);
				string[] array2 = new string[this.psiItemList.Count];
				string[][] array3 = new string[4][];
				for (int j = 0; j < array3.Length; j++)
				{
					array3[j] = new string[array2.Length];
				}
				for (int k = 0; k < array2.Length; k++)
				{
					array2[k] = this.psiItemList[k].aux.QualifiedName;
					for (int l = 0; l < array3.Length; l++)
					{
						if (l < this.psiItemList[k].aux.Symbols.Length && this.psiItemList[k].aux.Symbols[l] <= stats.Level)
						{
							Console.Write(PsiMenu.PSI_LEVEL_STRINGS[l]);
							array3[l][k] = PsiMenu.PSI_LEVEL_STRINGS[l];
						}
						else
						{
							array3[l][k] = PsiMenu.PSI_LEVEL_STRINGS[l];
						}
					}
				}
				this.psiList = new ScrollingList(new Vector2f(PsiMenu.PANEL_SIZE.X * 0.33f + 8f, 0f), 1, array2, 5, 14f, PsiMenu.PANEL_SIZE.X * 0.66f - 2f, PsiMenu.CURSOR_FILE);
				this.psiList.ShowSelectionRectangle = false;
				this.psiList.ShowCursor = false;
				this.psiList.Focused = false;
				base.Add(this.psiList);
				for (int m = 0; m < this.levelList.Length; m++)
				{
					this.levelList[m] = new ScrollingList(new Vector2f(PsiMenu.PANEL_SIZE.X * 0.33f + 80f + (float)(16 * m), 0f), 1, array3[m], 5, 14f, 1f, PsiMenu.CURSOR_FILE);
					this.levelList[m].ShowSelectionRectangle = false;
					this.levelList[m].ShowCursor = (m == 0);
					this.levelList[m].Focused = false;
				//	levelList.[m]
					base.Add(this.levelList[m]);
				}
			}
		}

		private void SelectTab(int index)
		{
			if (index < 0)
			{
				this.selectedTab = this.tabs.Length - 1;
			}
			else if (index >= this.tabs.Length)
			{
				this.selectedTab = 0;
			}
			else
			{
				this.selectedTab = index;
			}
			for (int i = 0; i < this.tabs.Length; i++)
			{
				this.tabs[i].CurrentPalette = ((i == this.selectedTab) ? 0U : 1U);
				this.tabs[i].Depth = ((i == this.selectedTab) ? 1 : -2);
				this.tabLabels[i].Color = ((i == this.selectedTab) ? PsiMenu.ACTIVE_TAB_TEXT_COLOR : PsiMenu.INACTIVE_TAB_TEXT_COLOR);
				this.tabLabels[i].Depth = ((i == this.selectedTab) ? 2 : -1);
			}
			this.SelectPsiTypeList();
			this.SetupPsiList();
		}

		public override object ButtonPressed(Button button)
		{
			object result = null;
			if (button == Button.A)
			{
				if (this.selectedList == this.psiTypeList)
				{
					this.SelectPsiList();
				}
				else if (this.selectedList == this.psiList)
				{
					result = new Tuple<IPsi, int>(this.psiItemList[this.psiList.SelectedIndex], this.selectedLevel);
				}
			}
			else if (button == Button.B)
			{
				if (this.selectedList == this.psiTypeList)
				{
					result = -1;
				}
				else if (this.selectedList == this.psiList)
				{
					this.SelectPsiTypeList();
				}
			}
			else if (button == Button.L)
			{
				this.SelectTab(this.selectedTab - 1);
			}
			else if (button == Button.R)
			{
				this.SelectTab(this.selectedTab + 1);
			}
			return result;
		}

		public override void Focus()
		{
		}

		public override void Unfocus()
		{
		}

		public const int PANEL_DEPTH = 0;

		public const float TAB_WIDTH = 50f;

		public const int MAX_SUPPORTED_PARTY_MEMBERS = 4;

		private const string FILE = "pause.dat";

		private const string FRONT_TAG = "firsttag";

		private const string TAG = "tag";

		public static readonly Vector2f PANEL_POSITION = MainMenu.PANEL_POSITION + new Vector2f(MainMenu.PANEL_SIZE.X + 20f, 13f);

		public static readonly Vector2f PANEL_SIZE = new Vector2f(320f - PsiMenu.PANEL_POSITION.X - 20f, 99f);

		public static readonly Color ACTIVE_TAB_TEXT_COLOR = Color.Black;

		public static readonly Color INACTIVE_TAB_TEXT_COLOR = new Color(65, 80, 79);

		public static readonly Color DIVIDER_COLOR = new Color(128, 140, 138);

		private static readonly string CURSOR_FILE = Paths.GRAPHICS + "realcursor.dat";

		private static readonly string[] PSI_TYPE_STRINGS = new string[]
		{
			StringFile.Instance.Get("psi.offense").Value,
			StringFile.Instance.Get("psi.recovery").Value,
			StringFile.Instance.Get("psi.support").Value,
			StringFile.Instance.Get("psi.other").Value
		};

		private static readonly string[] PSI_LEVEL_STRINGS = new string[]
		{
			"α",
			"β",
			"γ",
			"Ω"
		};

		private ShapeGraphic horizDivider;

		private ShapeGraphic vertDivider;

		private IndexedColorGraphic[] tabs;

		private TextRegion[] tabLabels;

		private int selectedTab;

		private ScrollingList psiTypeList;

		private ScrollingList psiList;

		private ScrollingList selectedList;

		private ScrollingList[] levelList;

		private int selectedLevel;

		private List<IPsi> psiItemList;

		private TextRegion descriptionText;
	}
}
