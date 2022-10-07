using System;
using System.Collections.Generic;
using Carbine;
using Carbine.Audio;
using Carbine.Graphics;
using Carbine.Input;
using Carbine.Scenes;
using Mother4.Data;
using Mother4.GUI;
using Mother4.GUI.ProfileMenu;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Scenes
{
	internal class ProfilesScene : StandardScene
	{
		public ProfilesScene()
		{
			this.panelList = new List<MenuPanel>();
		}

		private void ButtonPressed(InputManager sender, Button b)
		{
			if (b == Button.A)
			{
				this.DoLoad();
				return;
			}
			if (b == Button.B)
			{
				this.sfxCancel.Play();
				SceneManager.Instance.Pop();
			}
		}

		private void AxisPressed(InputManager sender, Vector2f axis)
		{
			if (axis.Y < 0f)
			{
				this.selectedIndex = Math.Max(0, this.selectedIndex - 1);
				this.UpdateCursor();
				return;
			}
			if (axis.Y > 0f)
			{
				this.selectedIndex = Math.Min(this.panelList.Count - 1, this.selectedIndex + 1);
				this.UpdateCursor();
			}
		}

		private void UpdateCursor()
		{
			this.cursorGraphic.Position = new Vector2f(24f, (float)(32 + 57 * this.selectedIndex));
			this.sfxCursorY.Play();
		}

		private void DoLoad()
		{
			if (this.profileList.ContainsKey(this.selectedIndex))
			{
				this.sfxConfirm.Play();
				SaveFileManager.Instance.LoadFile(this.selectedIndex);
				Engine.StartSession();
				OverworldScene newScene = new OverworldScene(SaveFileManager.Instance.CurrentProfile.MapName, SaveFileManager.Instance.CurrentProfile.Position, 6, false, false, true);
				SceneManager.Instance.Push(newScene, true);
				return;
			}
			this.sfxCancel.Play();
		}

		private void GenerateSelectionList()
		{
			this.profileList = SaveFileManager.Instance.LoadProfiles();
			int num = Math.Max(3, this.profileList.Count);
			for (int i = 0; i < num; i++)
			{
				MenuPanel item = new ProfilePanel(new Vector2f(8f, (float)(8 + 57 * i)), new Vector2f(288f, 33f), i, this.profileList.ContainsKey(i) ? this.profileList[i] : default(SaveProfile));
				this.panelList.Add(item);
			}
			this.pipeline.AddAll<MenuPanel>(this.panelList);
			this.cursorGraphic = new IndexedColorGraphic(Paths.GRAPHICS + "realcursor.dat", "right", new Vector2f(24f, (float)(32 + 57 * this.selectedIndex)), 100);
			this.pipeline.Add(this.cursorGraphic);
		}

		public override void Focus()
		{
			base.Focus();
			if (!this.isInitialized)
			{
				this.GenerateSelectionList();
				this.sfxCursorX = AudioManager.Instance.Use(Paths.SFXMENU + "cursorx.wav", AudioType.Sound);
				this.sfxCursorY = AudioManager.Instance.Use(Paths.SFXMENU + "cursory.wav", AudioType.Sound);
				this.sfxConfirm = AudioManager.Instance.Use(Paths.SFXMENU + "confirm.wav", AudioType.Sound);
				this.sfxCancel = AudioManager.Instance.Use(Paths.SFXMENU + "cancel.wav", AudioType.Sound);
				this.isInitialized = true;
			}
			ViewManager.Instance.Center = Engine.HALF_SCREEN_SIZE;
			Engine.ClearColor = Color.Black;
			InputManager.Instance.ButtonPressed += this.ButtonPressed;
			InputManager.Instance.AxisPressed += this.AxisPressed;
		}

		public override void Unfocus()
		{
			base.Unfocus();
			InputManager.Instance.ButtonPressed -= this.ButtonPressed;
			InputManager.Instance.AxisPressed -= this.AxisPressed;
		}

		protected override void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					this.cursorGraphic.Dispose();
					foreach (MenuPanel menuPanel in this.panelList)
					{
						menuPanel.Dispose();
					}
				}
				AudioManager.Instance.Unuse(this.sfxCursorX);
				AudioManager.Instance.Unuse(this.sfxCursorY);
				AudioManager.Instance.Unuse(this.sfxConfirm);
				AudioManager.Instance.Unuse(this.sfxCancel);
				base.Dispose(disposing);
			}
		}

		private const int PANEL_WIDTH = 288;

		private const int PANEL_HEIGHT = 33;

		private bool isInitialized;

		private CarbineSound sfxCursorX;

		private CarbineSound sfxCursorY;

		private CarbineSound sfxConfirm;

		private CarbineSound sfxCancel;

		private IDictionary<int, SaveProfile> profileList;

		private IndexedColorGraphic cursorGraphic;

		private IList<MenuPanel> panelList;

		private int selectedIndex;
	}
}
