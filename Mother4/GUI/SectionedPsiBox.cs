using System;
using System.Collections.Generic;
using Carbine.Actors;
using Carbine.Graphics;
using Carbine.GUI;
using Mother4.Data;
using Mother4.Psi;
using Rufini.Strings;
using SFML.Graphics;
using SFML.System;

namespace Mother4.GUI
{

	internal class SectionedPsiBox : Actor
	{
		public IEnumerable<OffensePsi> OffensePsiItems
		{
			get
			{
				return this.offensePsiItems;
			}
			set
			{
				this.offensePsiItems = new List<OffensePsi>(value);
			}
		}

		public IEnumerable<DefensivePsi> DefensePsiItems
		{
			get
			{
				return this.defensePsiItems;
			}
			set
			{
				this.defensePsiItems = new List<DefensivePsi>(value);
			}
		}

		public IEnumerable<AssistivePsi> AssistPsiItems
		{
			get
			{
				return this.assistPsiItems;
			}
			set
			{
				this.assistPsiItems = new List<AssistivePsi>(value);
			}
		}

		public IEnumerable<OtherPsi> OtherPsiItems
		{
			get
			{
				return this.otherPsiItems;
			}
			set
			{
				this.otherPsiItems = new List<OtherPsi>(value);
			}
		}

		public SectionedPsiBox(RenderPipeline pipeline, int depth, float lineHeight)
		{
			this.pipeline = pipeline;
			this.currentSelection = 0;
			this.currentTopLevelSelection = 0;
			this.currentSelectionLevel = 0;
			this.firstVisibleIndex = 0;
			this.lastVisibleIndex = 2;
			this.states = new RenderStates(BlendMode.Alpha);
			this.visible = false;
			this.depth = depth;
			this.lineHeight = lineHeight;
			this.psiTypes = new List<TextRegion>(4);
			this.activePsiList = new List<TextRegion>();
			this.activeAlphaList = new List<TextRegion>();
			this.activeBetaList = new List<TextRegion>();
			this.activeGammaList = new List<TextRegion>();
			this.activeOmegaList = new List<TextRegion>();
			this.windowPosition = new Vector2f(40f, 0f);
			this.window = new WindowBox(Settings.WindowStyle, Settings.WindowFlavor, this.windowPosition, new Vector2f(240f, 3f * lineHeight + 16f), 32766);
			this.selectorFillColor = UIColors.HighlightColor;

			//RectangleShape rectangleShape = new RectangleShape(new Vector2f(48f, lineHeight  ));
			//rectangleShape.FillColor = this.selectorFillColor;
			RectangleShape rectangleShape = new RectangleShape(new Vector2f(48,/* 11 * 1.3f - ScrollingList.SELECT_RECT_OFFSET.Y * 2f*/ lineHeight) - ScrollingList.SELECT_RECT_SIZE_OFFSET);
			rectangleShape.FillColor = UIColors.HighlightColor;

			this.selectorBox = new ShapeGraphic(rectangleShape, default(Vector2f), default(Vector2f), rectangleShape.Size, 32767);
			RectangleShape rectangleShape2 = new RectangleShape(new Vector2f(2f, 3f * lineHeight - 2f));
			rectangleShape2.FillColor = new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue, 70);


			this.separator = new ShapeGraphic(rectangleShape2, new Vector2f(102.399994f, lineHeight - 8f), default(Vector2f), rectangleShape2.Size, 32767);
			this.cursor = new IndexedColorGraphic(Paths.GRAPHICS + "realcursor.dat", "right", default(Vector2f), 32767);
			this.nucursor = new IndexedColorGraphic(Paths.GRAPHICS + "realcursor.dat", "right", default(Vector2f), 32767);
			this.states = new RenderStates(BlendMode.Alpha);
			//pipeline.Add(cursor);
		}


		internal OffensePsi SelectOffensePsi()
		{

			return this.offensePsiItems.Find((OffensePsi s) => s.aux.QualifiedName == this.activePsiList[this.currentSelection].Text);
		}

		internal AssistivePsi SelectAssistPsi()
		{
			return this.assistPsiItems.Find((AssistivePsi s) => s.aux.QualifiedName == this.activePsiList[this.currentSelection].Text);
		}

		internal DefensivePsi SelectDefensePsi()
		{
			return this.defensePsiItems.Find((DefensivePsi s) => s.aux.QualifiedName == this.activePsiList[this.currentSelection].Text);
		}

		internal OtherPsi SelectOtherPsi()
		{
			return this.otherPsiItems.Find((OtherPsi s) => s.aux.QualifiedName == this.activePsiList[this.currentSelection].Text);
		}

		internal bool InTypeSelection()
		{
			return this.currentSelectionLevel == 0;
		}
		public PsiType SelectedPsiType()
		{
			string text;
			if ((text = this.psiTypes[this.currentTopLevelSelection].Text) != null)
			{
				if (text == "Offense")
				{
					return PsiType.Offense;
				}
				if (text == "Recovery")
				{
					return PsiType.Assist;
				}
				if (text == "Support")
				{
					return PsiType.Defense;
				}
				if (text == "Other")
				{
					return PsiType.Other;
				}
			}
			throw new MissingMemberException("No Psi Type selected");
		}

		internal int SelectedLevel()
		{
			return this.currentSelectionLevel - 1;
		}

		internal void Reset()
		{
			if (this.offensePsiItems != null)
			{
				this.offensePsiItems.Clear();
			}
			if (this.defensePsiItems != null)
			{
				this.defensePsiItems.Clear();
			}
			if (this.assistPsiItems != null)
			{
				this.assistPsiItems.Clear();
			}
			if (this.otherPsiItems != null)
			{
				this.otherPsiItems.Clear();
			}
			this.currentSelection = 0;
			this.currentSelectionLevel = 0;
			this.currentTopLevelSelection = 0;
		}

		internal void Show()
		{
			if (!this.visible)
			{
				this.visible = true;
				this.pipeline.Add(this.window);
				if (this.offensePsiItems.Count != 0)
				{
					this.AddPsiType(StringFile.Instance.Get("psi.offense").Value);
				}
				if (this.assistPsiItems.Count != 0)
				{
					this.AddPsiType(StringFile.Instance.Get("psi.recovery").Value);
				}
				if (this.defensePsiItems.Count != 0)
				{
					this.AddPsiType(StringFile.Instance.Get("psi.support").Value);
				}
				foreach (TextRegion renderable in this.psiTypes)
				{
					this.pipeline.Add(renderable);
				}
				if (this.psiTypes.Count != 0)
				{
					this.UpdateActiveAbilityList();
					this.UpdateSelectorBox();
					this.UpdateCursor();
					this.pipeline.Add(this.selectorBox);
					this.pipeline.Add(this.cursor);
					this.pipeline.Add(this.nucursor);
				}
				this.pipeline.Add(this.separator);
			}
		}

		internal void Hide()
		{
			if (this.visible)
			{
				this.visible = false;
				this.pipeline.Remove(this.window);
				this.ClearListFromPipeline<TextRegion>(this.psiTypes);
				this.ClearListFromPipeline<TextRegion>(this.activePsiList);
				this.ClearListFromPipeline<TextRegion>(this.activeAlphaList);
				this.ClearListFromPipeline<TextRegion>(this.activeBetaList);
				this.ClearListFromPipeline<TextRegion>(this.activeGammaList);
				this.ClearListFromPipeline<TextRegion>(this.activeOmegaList);
				this.pipeline.Remove(this.selectorBox);
				this.pipeline.Remove(this.cursor);
				this.pipeline.Remove(this.separator);
				this.pipeline.Remove(nucursor);
				this.psiTypes.Clear();
				this.activePsiList.Clear();
				this.pipeline.Target.Clear();
			}
		}


		internal void SelectDown()
		{
			if (this.currentSelectionLevel == 0)
			{
				this.psiTypes[this.currentTopLevelSelection].Color = Color.White;
				this.currentTopLevelSelection = (this.currentTopLevelSelection + 1) % this.psiTypes.Count;
				this.currentSelection = 0;
				this.firstVisibleIndex = 0;
				this.lastVisibleIndex = 2;
				this.UpdateActiveAbilityList();
			}
			else if (this.currentSelectionLevel >= 1)
			{
				this.currentSelection = (this.currentSelection + 1) % this.activePsiList.Count;
				if (this.currentSelection > this.lastVisibleIndex)
				{
					this.lastVisibleIndex = this.currentSelection;
					this.firstVisibleIndex = this.lastVisibleIndex - 3 + 1;
					this.UpdateActiveAbilityList();
				}
				else if (this.currentSelection < this.firstVisibleIndex)
				{
					this.firstVisibleIndex = this.currentSelection;
					this.lastVisibleIndex = this.currentSelection + 3 - 1;
					this.UpdateActiveAbilityList();
				}
				if (this.currentSelectionLevel == 2)
				{
					if (this.activeBetaList[this.currentSelection] == null && this.activeOmegaList[this.currentSelection] == null)
					{
						this.currentSelectionLevel = 1;
					}
				}
				else if (this.currentSelectionLevel == 3)
				{
					if (this.activeBetaList[this.currentSelection] == null)
					{
						this.currentSelectionLevel = 1;
					}
					else if (this.activeGammaList[this.currentSelection] == null)
					{
						this.currentSelectionLevel = 2;
					}
				}
				else if (this.activeBetaList[this.currentSelection] == null)
				{
					this.currentSelectionLevel = 1;
				}
				else if (this.activeGammaList[this.currentSelection] == null)
				{
					this.currentSelectionLevel = 2;
				}
				else if (this.activeOmegaList[this.currentSelection] == null)
				{
					this.currentSelectionLevel = 3;
				}
			}
			this.UpdateSelectorBox();
		}

		internal void SelectUp()
		{
			if (this.currentSelectionLevel == 0)
			{
				this.psiTypes[this.currentTopLevelSelection].Color = Color.White;
				this.currentTopLevelSelection--;
				if (this.currentTopLevelSelection < 0)
				{
					this.currentTopLevelSelection = this.psiTypes.Count - 1;
				}
				this.currentSelection = 0;
				this.firstVisibleIndex = 0;
				this.lastVisibleIndex = 2;
				this.UpdateActiveAbilityList();
			}
			else if (this.currentSelectionLevel >= 1)
			{
				this.currentSelection--;
				if (this.currentSelection < 0)
				{
					this.currentSelection = this.activePsiList.Count - 1;
				}
				if (this.currentSelection < this.firstVisibleIndex)
				{
					this.firstVisibleIndex = this.currentSelection;
					this.lastVisibleIndex = this.currentSelection + 3 - 1;
					this.UpdateActiveAbilityList();
				}
				else if (this.currentSelection > this.lastVisibleIndex)
				{
					this.lastVisibleIndex = this.currentSelection;
					this.firstVisibleIndex = this.lastVisibleIndex - 3 + 1;
					this.UpdateActiveAbilityList();
				}
				if (this.currentSelectionLevel == 2)
				{
					if (this.activeBetaList[this.currentSelection] == null && this.activeOmegaList[this.currentSelection] == null)
					{
						this.currentSelectionLevel = 1;
					}
				}
				else if (this.currentSelectionLevel == 3)
				{
					if (this.activeBetaList[this.currentSelection] == null)
					{
						this.currentSelectionLevel = 1;
					}
					else if (this.activeGammaList[this.currentSelection] == null)
					{
						this.currentSelectionLevel = 2;
					}
				}
				else if (this.activeBetaList[this.currentSelection] == null)
				{
					this.currentSelectionLevel = 1;
				}
				else if (this.activeGammaList[this.currentSelection] == null)
				{
					this.currentSelectionLevel = 2;
				}
				else if (this.activeOmegaList[this.currentSelection] == null)
				{
					this.currentSelectionLevel = 3;
				}
			}
			this.UpdateSelectorBox();
		}


		internal void SelectRight()
		{
			switch (this.currentSelectionLevel)
			{
				case 0:
					this.psiTypes[this.currentTopLevelSelection].Color = Color.White;
					this.currentSelectionLevel = 1;
					this.UpdateSelectorBox();
					return;
				case 1:
					if (this.activeBetaList[this.currentSelection] != null || this.activeOmegaList[this.currentSelection] != null)
					{
						this.currentSelectionLevel = 2;
						this.UpdateSelectorBox();
						return;
					}
					break;
				case 2:
					if (this.activeGammaList[this.currentSelection] != null)
					{
						this.currentSelectionLevel = 3;
						this.UpdateSelectorBox();
						return;
					}
					break;
				case 3:
					if (this.activeOmegaList[this.currentSelection] != null)
					{
						this.currentSelectionLevel = 4;
						this.UpdateSelectorBox();
					}
					break;
				default:
					return;
			}
		}

		internal void SelectLeft()
		{
			if (this.currentSelectionLevel > 0)
			{
				this.currentSelectionLevel--;
				this.UpdateSelectorBox();
			}
		}

		private void AddPsiType(string name)
		{
			this.psiTypes.Add(new TextRegion(new Vector2f(this.windowPosition.X + 12f, this.windowPosition.Y + 5f + this.lineHeight * (float)this.psiTypes.Count), 32768, Fonts.Main, name));
		}

		private void AddPsiForCurrentSelection()
		{
			string text;
			if ((text = this.psiTypes[this.currentTopLevelSelection].Text) != null)
			{
				if (text == "Offense")
				{
					this.AddPsiAbilityFromList<OffensePsi>(this.offensePsiItems);
					return;
				}
				if (text == "Recovery")
				{
					this.AddPsiAbilityFromList<AssistivePsi>(this.assistPsiItems);
					return;
				}
				if (text == "Support")
				{

					this.AddPsiAbilityFromList<DefensivePsi>(this.defensePsiItems);
					return;
				}
				if (!(text == "Other"))
				{
					return;
				}
				this.AddPsiAbilityFromList<OtherPsi>(this.otherPsiItems);
			}
		}

		private void AddPsiAbilityFromList<T>(List<T> psiList) where T : IPsi
		{
			for (int i = 0; i < psiList.Count; i++)
			{
				IPsi psi = psiList[i];
				if (psi.aux.Symbols[0] <= this.MaxLevel)
				{
					if (i < this.firstVisibleIndex || i > this.lastVisibleIndex)
					{
						this.activePsiList.Add(null);
						this.activeAlphaList.Add(null);
						this.activeBetaList.Add(null);
						this.activeGammaList.Add(null);
						this.activeOmegaList.Add(null);
					}
					else
					{
						/*	private static readonly string[] PSI_LEVEL_STRINGS = new string[]
	{
		"α",
		"β",
		"γ",
		"Ω"
	};*/

						this.activeAlphaList.Add(new TextRegion(new Vector2f(200f, this.windowPosition.Y + 5f + this.lineHeight * (float)(this.activePsiList.Count - this.firstVisibleIndex)), 32768, Fonts.Main, "α"));
						if (psi.aux.Symbols.Length == 4)
						{
							if (psi.aux.Symbols[1] < this.MaxLevel)
							{
								this.activeBetaList.Add(new TextRegion(new Vector2f(220f, this.windowPosition.Y + 5f + this.lineHeight * (float)(this.activePsiList.Count - this.firstVisibleIndex)), 32768, Fonts.Main, "β"));
								if (psi.aux.Symbols[2] < this.MaxLevel)
								{
									this.activeGammaList.Add(new TextRegion(new Vector2f(240f, this.windowPosition.Y + 5f + this.lineHeight * (float)(this.activePsiList.Count - this.firstVisibleIndex)), 32768, Fonts.Main, "γ"));
									if (psi.aux.Symbols[3] < this.MaxLevel)
									{
										this.activeOmegaList.Add(new TextRegion(new Vector2f(260f, this.windowPosition.Y + 5f + this.lineHeight * (float)(this.activePsiList.Count - this.firstVisibleIndex)), 32768, Fonts.Main, "Ω"));
									}
									else
									{
										this.activeOmegaList.Add(null);
									}
								}
								else
								{
									this.activeGammaList.Add(null);
									this.activeOmegaList.Add(null);
								}
							}
							else
							{
								this.activeBetaList.Add(null);
								this.activeGammaList.Add(null);
								this.activeOmegaList.Add(null);
							}
						}
						else if (psi.aux.Symbols.Length == 2)
						{
							if (psi.aux.Symbols[1] < this.MaxLevel)
							{
								this.activeOmegaList.Add(new TextRegion(new Vector2f(220f, this.windowPosition.Y + 5f + this.lineHeight * (float)(this.activePsiList.Count - this.firstVisibleIndex)), 32768, Fonts.Main, "Ω"));
							}
							else
							{
								this.activeOmegaList.Add(null);
							}
							this.activeBetaList.Add(null);
							this.activeGammaList.Add(null);
						}
						this.activePsiList.Add(new TextRegion(new Vector2f(120f, this.windowPosition.Y + 5f + this.lineHeight * (float)(this.activePsiList.Count - this.firstVisibleIndex)), 32768, Fonts.Main, psi.aux.QualifiedName));
					}
				}
			}
		}

		private void UpdateSelectorBox()
		{
			if (this.currentSelectionLevel == 0)
			{
				this.nucursor.Position = new Vector2f(this.psiTypes[this.currentTopLevelSelection].Position.X, //- 2f,
					this.psiTypes[this.currentTopLevelSelection].Position.Y + 5f);

				this.selectorBox.Position = new Vector2f(this.psiTypes[this.currentTopLevelSelection].Position.X - 1f, this.psiTypes[this.currentTopLevelSelection].Position.Y - 1f);
				this.psiTypes[this.currentTopLevelSelection].Color = Color.Black;
				this.cursor.Visible = false;
				return;
			}
			if (this.currentSelectionLevel == 1)
			{
				this.cursor.Visible = true;

				try
				{


					this.cursor.Position = new Vector2f(this.activeAlphaList[this.currentSelection].Position.X - 3f,
						this.activeAlphaList[this.currentSelection].Position.Y + 8f);
				}
				catch (Exception e)
				{
					this.selectorBox.Position = new Vector2f(this.psiTypes[this.currentTopLevelSelection].Position.X - 1f, this.psiTypes[this.currentTopLevelSelection].Position.Y + 3f);
					this.psiTypes[this.currentTopLevelSelection].Color = Color.Black;
					this.cursor.Visible = false;
					currentSelectionLevel = 0;
				}

				return;
			}
			if (this.currentSelectionLevel == 2)
			{
				if (this.activeBetaList[this.currentSelection] != null)
				{
					this.cursor.Position = new Vector2f(this.activeBetaList[this.currentSelection].Position.X - 3f, this.activeBetaList[this.currentSelection].Position.Y + 8f);
					return;
				}
				this.cursor.Position = new Vector2f(this.activeOmegaList[this.currentSelection].Position.X - 3f, this.activeOmegaList[this.currentSelection].Position.Y + 8f);
				return;
			}
			else
			{
				if (this.currentSelectionLevel == 3)
				{
					this.cursor.Position = new Vector2f(this.activeGammaList[this.currentSelection].Position.X - 3f, this.activeGammaList[this.currentSelection].Position.Y + 8f);
					return;
				}
				this.cursor.Position = new Vector2f(this.activeOmegaList[this.currentSelection].Position.X - 3f, this.activeOmegaList[this.currentSelection].Position.Y + 8f);
				return;
			}
		}
		private void UpdateCursor()
		{
			int num = this.currentSelectionLevel;
		}

		private void UpdateActiveAbilityList()
		{
			this.ClearListFromPipeline<TextRegion>(this.activePsiList);
			this.ClearListFromPipeline<TextRegion>(this.activeAlphaList);
			this.ClearListFromPipeline<TextRegion>(this.activeBetaList);
			this.ClearListFromPipeline<TextRegion>(this.activeGammaList);
			this.ClearListFromPipeline<TextRegion>(this.activeOmegaList);
			this.AddPsiForCurrentSelection();
			for (int i = this.firstVisibleIndex; i < Math.Min(this.activePsiList.Count, this.lastVisibleIndex + 1); i++)
			{
				this.pipeline.Add(this.activePsiList[i]);
				this.pipeline.Add(this.activeAlphaList[i]);
				if (this.activeBetaList[i] != null)
				{
					this.pipeline.Add(this.activeBetaList[i]);
				}
				if (this.activeGammaList[i] != null)
				{
					this.pipeline.Add(this.activeGammaList[i]);
				}
				if (this.activeOmegaList[i] != null)
				{
					this.pipeline.Add(this.activeOmegaList[i]);
				}
			}
		}

		private void ClearListFromPipeline<T>(List<T> list) where T : Renderable
		{
			foreach (T t in list)
			{
				if (t != null)
				{
					this.pipeline.Remove(t);
				}
			}
			list.Clear();
		}

		private const int SCROLL_LIMIT = 3;

		private readonly WindowBox window;

		private readonly ShapeGraphic selectorBox;

		private readonly Vector2f windowPosition;

		private readonly ShapeGraphic separator;

		private readonly IndexedColorGraphic cursor;
		private readonly IndexedColorGraphic nucursor;

		private int depth;

		private float lineHeight;

		private bool visible;

		private int currentSelection;

		private int currentTopLevelSelection;

		private int currentSelectionLevel;

		private int firstVisibleIndex;

		private int lastVisibleIndex;

		private RenderStates states;

		private Color selectorFillColor;

		private List<TextRegion> psiTypes;

		private List<TextRegion> activePsiList;

		private List<TextRegion> activeAlphaList;

		private List<TextRegion> activeBetaList;

		private List<TextRegion> activeGammaList;

		private List<TextRegion> activeOmegaList;

		private List<OffensePsi> offensePsiItems;

		private List<DefensivePsi> defensePsiItems;

		private List<AssistivePsi> assistPsiItems;

		private List<OtherPsi> otherPsiItems;

		public int MaxLevel;

		protected readonly RenderPipeline pipeline;
	}
}
