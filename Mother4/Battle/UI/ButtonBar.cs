using System;
using System.Linq;
using Carbine.Actors;
using Carbine.Graphics;
using Mother4.Data;
using SFML.System;

namespace Mother4.Battle.UI
{
	internal class ButtonBar : Actor
	{
		public int SelectedIndex
		{
			get
			{
				return this.selIndex;
			}
			set
			{
				this.lastSelIndex = this.selIndex;
				this.selIndex = Math.Max(0, Math.Min(this.buttons.Length - 1, value));
				this.SelectedIndexChanged();
			}
		}

		public ButtonBar.Action SelectedAction
		{
			get
			{
				return this.buttonActions[this.selIndex];
			}
		}

		public bool Visible
		{
			get
			{
				return this.visible;
			}
			set
			{
				this.visible = value;
			}
		}

		public ButtonBar(RenderPipeline pipeline)
		{
			this.pipeline = pipeline;
			this.buttonActions = new ButtonBar.Action[0];
			this.buttons = new Graphic[0];
			this.selIndex = 0;
			this.visible = false;
		}

		private void SetUpButtons(ButtonBar.Action[] newActions)
		{
			for (int i = 0; i < this.buttons.Length; i++)
			{
				if (this.buttons[i] != null)
				{
					this.pipeline.Remove(this.buttons[i]);
				}
			}
			this.buttonActions = new ButtonBar.Action[newActions.Length];
			this.buttons = new Graphic[newActions.Length];
			this.buttonWidths = new int[this.buttons.Length];
			int num = 0;
			for (int j = 0; j < newActions.Length; j++)
			{
				string spriteName;
				switch (newActions[j])
				{
				case ButtonBar.Action.Bash:
					spriteName = "bash";
					break;
				case ButtonBar.Action.Psi:
					spriteName = "psi";
					break;
				case ButtonBar.Action.Items:
					spriteName = "goods";
					break;
				case ButtonBar.Action.Talk:
					spriteName = "talk";
					break;
				case ButtonBar.Action.Guard:
					spriteName = "guard";
					break;
				case ButtonBar.Action.Run:
					spriteName = "run";
					break;
				default:
					throw new NotImplementedException("Unimplemented button action type.");
				}
				this.buttonActions[j] = newActions[j];
				this.buttons[j] = new IndexedColorGraphic(ButtonBar.GRAPHIC_FILE, spriteName, default(Vector2f), 32757);
				this.buttonWidths[j] = this.buttons[j].TextureRect.Width;
				num += this.buttonWidths[j] + 2;
			}
			num -= 2;
			this.buttonYs = new int[this.buttons.Length];
			this.buttonHeights = new int[this.buttons.Length];
			int num2 = 160 - num / 2;
			for (int k = 0; k < this.buttons.Length; k++)
			{
				this.buttonHeights[k] = -24;
				this.buttonYs[k] = this.buttonHeights[k];
				this.buttons[k].Position = new Vector2f((float)num2, (float)this.buttonYs[k]);
				num2 += this.buttonWidths[k] + 2;
				this.pipeline.Add(this.buttons[k]);
			}
		}

		public void SetActions(ButtonBar.Action[] newActions)
		{
			if (!this.buttonActions.SequenceEqual(newActions))
			{
				this.SetUpButtons(newActions);
			}
		}

		private void SelectedIndexChanged()
		{
			for (int i = 0; i < this.buttons.Length; i++)
			{
				this.buttonHeights[i] = 4;
				this.buttons[i].Frame = 0f;
			}
			this.buttonHeights[this.SelectedIndex] = 4;
			this.buttons[this.SelectedIndex].Frame = 1f;
		}

		public void SelectRight()
		{
			if (this.selIndex + 1 < this.buttons.Length)
			{
				this.SelectedIndex = this.selIndex + 1;
				return;
			}
			this.SelectedIndex = 0;
		}

		public void SelectLeft()
		{
			if (this.selIndex - 1 >= 0)
			{
				this.SelectedIndex = this.selIndex - 1;
				return;
			}
			this.SelectedIndex = this.buttons.Length - 1;
		}

		public void Show(int index)
		{
			this.SelectedIndex = index;
			this.visible = true;
		}

		public void Show()
		{
			this.Show(this.selIndex);
		}

		public void Hide()
		{
			for (int i = 0; i < this.buttons.Length; i++)
			{
				this.buttonHeights[i] = -this.buttons[i].TextureRect.Height - 1;
			}
			this.visible = false;
		}

		public override void Input()
		{
			base.Input();
		}

		public override void Update()
		{
			base.Update();
			for (int i = 0; i < this.buttons.Length; i++)
			{
				if (this.buttonYs[i] < this.buttonHeights[i])
				{
					this.buttonYs[i] += (int)((float)(this.buttonHeights[i] - this.buttonYs[i]) / 2f);
					this.buttons[i].Position = new Vector2f(this.buttons[i].Position.X, (float)this.buttonYs[i]);
				}
				else if (this.buttonYs[i] > this.buttonHeights[i])
				{
					this.buttonYs[i] += (int)((float)(this.buttonHeights[i] - this.buttonYs[i]) / 2f);
					this.buttons[i].Position = new Vector2f(this.buttons[i].Position.X, (float)this.buttonYs[i]);
				}
			}
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			for (int i = 0; i < this.buttons.Length; i++)
			{
				this.buttons[i].Dispose();
			}
		}

		private const int BUTTON_MARGIN = 2;

		private const int BUTTON_MIN_Y = 4;

		private const int BUTTON_MAX_Y = 4;

		private const int BUTTON_HEIGHT = 24;

		private static readonly string GRAPHIC_FILE = Paths.GRAPHICS + "battleui2.dat";

		private int[] buttonYs;

		private int[] buttonHeights;

		private RenderPipeline pipeline;

		private int selIndex;

		private int lastSelIndex;

		private int[] buttonWidths;

		private Graphic[] buttons;

		private ButtonBar.Action[] buttonActions;

		private bool visible;

		public enum Action
		{
			None,
			Bash,
			Psi,
			Items,
			Talk,
			Guard,
			Run
		}
	}
}
