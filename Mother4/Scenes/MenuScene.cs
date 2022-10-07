using System;
using Carbine.Flags;
using Carbine.Input;
using Carbine.Scenes;
using Carbine.Scenes.Transitions;
using Mother4.Battle.UI;
using Mother4.Data;
using Mother4.GUI;
using Mother4.GUI.OverworldMenu;
using Mother4.Psi;
using SFML.System;

namespace Mother4.Scenes
{
	internal class MenuScene : StandardScene
	{
		public MenuScene()
		{
			this.mainPanel = new MainMenu();
			this.mainPanel.Visible = false;
			this.pipeline.Add(this.mainPanel);
			this.activePanel = this.mainPanel;
			this.moneyPanel = new MoneyMenu();
			this.moneyPanel.Visible = false;
			this.pipeline.Add(this.moneyPanel);
			this.goodsPanel = new GoodsMenu();
			this.goodsPanel.Visible = false;
			this.pipeline.Add(this.goodsPanel);
			this.psiPanel = new PsiMenu();
			this.psiPanel.Visible = false;
			this.pipeline.Add(this.psiPanel);
			this.cardBar = new CardBar(this.pipeline, PartyManager.Instance.ToArray(), null);
			this.cardBar.Hide(true);
			this.cardBar.Show();
			this.actorManager.Add(this.cardBar);
		}

		private void Initialize()
		{
			if (!this.initialized)
			{
				this.mainPanel.Visible = true;
				this.moneyPanel.Visible = true;
				this.initialized = true;
			}
		}

		private void AxisPressed(InputManager sender, Vector2f axis)
		{
			this.isCursorTime = true;
			this.axis = axis;
		}

		private void ChangeActivePanel(MenuPanel panel)
		{
			this.activePanel.Unfocus();
			this.activePanel = panel;
			this.activePanel.Visible = true;
			this.activePanel.Focus();
		}

		private void ExitMenu()
		{
			SceneManager.Instance.Transition = new InstantTransition();
			SceneManager.Instance.Pop();
		}

		private void HandleMainMenuButton(object retVal)
		{
			switch (((int?)retVal).Value)
			{
			case -1:
				this.ExitMenu();
				return;
			case 0:
				this.ChangeActivePanel(this.goodsPanel);
				return;
			case 1:
				this.ChangeActivePanel(this.psiPanel);
				break;
			case 2:
			case 3:
				break;
			default:
				return;
			}
		}

		private void HandleGoodsMenuButton(object retVal)
		{
			int value = ((int?)retVal).Value;
			if (value != -1)
			{
				return;
			}
			this.activePanel.Unfocus();
			this.goodsPanel.Visible = false;
			this.activePanel = this.mainPanel;
			this.activePanel.Focus();
		}

		private void HandlePsiMenuButton(object retVal)
		{
			if (!(retVal is int))
			{
				if (retVal is Tuple<IPsi, int>)
				{
					IPsi item = ((Tuple<IPsi, int>)retVal).Item1;
					int item2 = ((Tuple<IPsi, int>)retVal).Item2;
					if (item.aux.Key== "psi.telepathy")
					{
						FlagManager.Instance[3] = true;
						this.ExitMenu();
					}
				}
				return;
			}
			int num = (int)retVal;
			if (num != -1)
			{
				return;
			}
			this.activePanel.Unfocus();
			this.psiPanel.Visible = false;
			this.activePanel = this.mainPanel;
			this.activePanel.Focus();
		}

		private void ButtonPressed(InputManager sender, Button b)
		{
			this.isButtonTime = true;
			this.button = b;
		}

		public override void Focus()
		{
			base.Focus();
			SceneManager.Instance.CompositeMode = true;
			base.DrawBehind = true;
			this.Initialize();
			InputManager.Instance.AxisPressed += this.AxisPressed;
			InputManager.Instance.ButtonPressed += this.ButtonPressed;
		}

		public override void Unfocus()
		{
			base.Unfocus();
			SceneManager.Instance.CompositeMode = false;
			base.DrawBehind = false;
			InputManager.Instance.AxisPressed -= this.AxisPressed;
			InputManager.Instance.ButtonPressed -= this.ButtonPressed;
		}

		public override void Update()
		{
			base.Update();
			if (this.isButtonTime)
			{
				if (this.activePanel != null)
				{
					object obj = this.activePanel.ButtonPressed(this.button);
					if (obj != null)
					{
						if (this.activePanel is MainMenu)
						{
							this.HandleMainMenuButton(obj);
						}
						else if (this.activePanel is GoodsMenu)
						{
							this.HandleGoodsMenuButton(obj);
						}
						else if (this.activePanel is PsiMenu)
						{
							this.HandlePsiMenuButton(obj);
						}
					}
				}
				this.isButtonTime = false;
			}
			if (this.isCursorTime)
			{
				if (this.activePanel != null)
				{
					this.activePanel.AxisPressed(this.axis);
				}
				this.isCursorTime = false;
			}
		}

		protected override void Dispose(bool disposing)
		{
			bool disposed = this.disposed;
			base.Dispose(disposing);
		}

		private bool initialized;

		private bool isButtonTime;

		private bool isCursorTime;

		private Button button;

		private Vector2f axis;

		private MenuPanel activePanel;

		private MenuPanel mainPanel;

		private MenuPanel moneyPanel;

		private MenuPanel goodsPanel;

		private MenuPanel psiPanel;

		private CardBar cardBar;
	}
}
