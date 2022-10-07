using System;
using Carbine;
using Carbine.Audio;
using Carbine.Graphics;
using Carbine.GUI;
using Carbine.Input;
using Carbine.Scenes;
using Mother4.Data;
using Mother4.GUI;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Scenes
{
	internal class OptionsScene : StandardScene
	{
		public OptionsScene()
		{
			this.sfxCursorX = AudioManager.Instance.Use(Paths.SFXMENU + "cursorx.wav", AudioType.Sound);
			this.sfxCursorY = AudioManager.Instance.Use(Paths.SFXMENU + "cursory.wav", AudioType.Sound);
			this.sfxConfirm = AudioManager.Instance.Use(Paths.SFXMENU + "confirm.wav", AudioType.Sound);
			this.sfxCancel = AudioManager.Instance.Use(Paths.SFXMENU + "cancel.wav", AudioType.Sound);
			this.titleText = new TextRegion(new Vector2f(4f, 4f), 0, Fonts.Title, "Global Options");
			this.pipeline.Add(this.titleText);
			this.mainList = new ScrollingList(OptionsScene.MENU_POSITION, 0, OptionsScene.MAIN_MENU, 8, 16f, 80f, OptionsScene.CURSOR_FILE);
			this.bgmVolumeList = new ScrollingList(OptionsScene.MENU_POSITION, 0, OptionsScene.BGM_MENU, 8, 16f, 80f, OptionsScene.CURSOR_FILE);
			this.sfxVolumeList = new ScrollingList(OptionsScene.MENU_POSITION, 0, OptionsScene.SFX_MENU, 8, 16f, 80f, OptionsScene.CURSOR_FILE);
			this.flavorList = new ScrollingList(OptionsScene.MENU_POSITION, 0, OptionsScene.FLAVOR_MENU, 8, 16f, 80f, OptionsScene.CURSOR_FILE);
			this.textSpeedList = new ScrollingList(OptionsScene.MENU_POSITION, 0, OptionsScene.SPEED_MENU, 8, 16f, 80f, OptionsScene.CURSOR_FILE);
			this.scaleList = new ScrollingList(OptionsScene.MENU_POSITION, 0, OptionsScene.SCALE_MENU, 8, 16f, 80f, OptionsScene.CURSOR_FILE);
			this.fullscreenList = new ScrollingList(OptionsScene.MENU_POSITION, 0, OptionsScene.FULLSCREEN_MENU, 8, 16f, 80f, OptionsScene.CURSOR_FILE);
			this.focusedList = this.mainList;
			this.pipeline.Add(this.mainList);
			this.pipeline.Add(this.bgmVolumeList);
			this.pipeline.Add(this.sfxVolumeList);
			this.pipeline.Add(this.flavorList);
			this.pipeline.Add(this.textSpeedList);
			this.pipeline.Add(this.scaleList);
			this.pipeline.Add(this.fullscreenList);
			this.bgmVolumeList.Hide();
			this.sfxVolumeList.Hide();
			this.flavorList.Hide();
			this.textSpeedList.Hide();
			this.scaleList.Hide();
			this.fullscreenList.Hide();
		}

		private void AxisPressed(InputManager sender, Vector2f axis)
		{
			if (this.focusedList != null)
			{
				if (axis.Y < -0.1f)
				{
					if (this.focusedList.SelectPrevious())
					{
						this.sfxCursorY.Play();
						return;
					}
				}
				else if (axis.Y > 0.1f && this.focusedList.SelectNext())
				{
					this.sfxCursorX.Play();
				}
			}
		}

		private void ButtonPressed(InputManager sender, Button b)
		{
			switch (b)
			{
			case Button.A:
			case Button.Start:
				this.DoSelection();
				return;
			case Button.B:
				this.DoCancel();
				break;
			case Button.X:
			case Button.Y:
				break;
			default:
				return;
			}
		}

		private void DoMainListSelection()
		{
			if (this.mainList.SelectedIndex < OptionsScene.MAIN_MENU.Length - 1)
			{
				this.sfxConfirm.Play();
				this.mainList.Hide();
			}
			switch (this.mainList.SelectedIndex)
			{
			case 0:
				(this.focusedList = this.bgmVolumeList).Show();
				this.bgmVolumeList.SelectedIndex = 10 - (int)(Settings.MusicVolume * 11f);
				return;
			case 1:
				(this.focusedList = this.sfxVolumeList).Show();
				this.sfxVolumeList.SelectedIndex = 10 - (int)(Settings.EffectsVolume * 11f);
				return;
			case 2:
				(this.focusedList = this.flavorList).Show();
				this.flavorList.SelectedIndex = (int)Settings.WindowFlavor;
				return;
			case 3:
				(this.focusedList = this.textSpeedList).Show();
				this.textSpeedList.SelectedIndex = Settings.TextSpeed - 1;
				return;
			case 4:
				(this.focusedList = this.scaleList).Show();
				this.scaleList.SelectedIndex = (int)(Engine.ScreenScale - 1U);
				return;
			case 5:
				(this.focusedList = this.fullscreenList).Show();
				this.fullscreenList.SelectedIndex = (Engine.Fullscreen ? 1 : 0);
				return;
			case 6:
				this.sfxCancel.Play();
				SceneManager.Instance.Pop();
				return;
			default:
				return;
			}
		}

		private void DoBGMListSelection()
		{
			if (this.bgmVolumeList.SelectedIndex < OptionsScene.BGM_MENU.Length - 1)
			{
				this.sfxConfirm.Play();
				AudioManager.Instance.MusicVolume = (float)(10 - this.bgmVolumeList.SelectedIndex) / 10f;
				Settings.MusicVolume = AudioManager.Instance.MusicVolume;
				this.GoBackToMainList();
				return;
			}
			this.DoCancel();
		}

		private void DoSFXListSelection()
		{
			if (this.sfxVolumeList.SelectedIndex < OptionsScene.SFX_MENU.Length - 1)
			{
				this.sfxConfirm.Play();
				AudioManager.Instance.EffectsVolume = (float)(10 - this.sfxVolumeList.SelectedIndex) / 10f;
				Settings.EffectsVolume = AudioManager.Instance.EffectsVolume;
				this.GoBackToMainList();
				return;
			}
			this.DoCancel();
		}

		private void DoFlavorSelection()
		{
			if (this.flavorList.SelectedIndex < OptionsScene.FLAVOR_MENU.Length - 1)
			{
				this.sfxConfirm.Play();
				Settings.WindowFlavor = (uint)this.flavorList.SelectedIndex;
				this.GoBackToMainList();
				return;
			}
			this.DoCancel();
		}

		private void DoSpeedSelection()
		{
			if (this.textSpeedList.SelectedIndex < OptionsScene.SPEED_MENU.Length - 1)
			{
				this.sfxConfirm.Play();
				Settings.TextSpeed = this.textSpeedList.SelectedIndex + 1;
				this.GoBackToMainList();
				return;
			}
			this.DoCancel();
		}

		private void DoScaleSelection()
		{
			if (this.scaleList.SelectedIndex < OptionsScene.SCALE_MENU.Length - 1)
			{
				this.sfxConfirm.Play();
				Engine.ScreenScale = (uint)(this.scaleList.SelectedIndex + 1);
				this.GoBackToMainList();
				return;
			}
			this.DoCancel();
		}

		private void DoFullscreenSelection()
		{
			if (this.fullscreenList.SelectedIndex < OptionsScene.FULLSCREEN_MENU.Length - 1)
			{
				this.sfxConfirm.Play();
				Engine.Fullscreen = (this.fullscreenList.SelectedIndex > 0);
				this.GoBackToMainList();
				return;
			}
			this.DoCancel();
		}

		private void DoSelection()
		{
			if (this.focusedList == this.mainList)
			{
				this.DoMainListSelection();
				return;
			}
			if (this.focusedList == this.bgmVolumeList)
			{
				this.DoBGMListSelection();
				return;
			}
			if (this.focusedList == this.sfxVolumeList)
			{
				this.DoSFXListSelection();
				return;
			}
			if (this.focusedList == this.flavorList)
			{
				this.DoFlavorSelection();
				return;
			}
			if (this.focusedList == this.textSpeedList)
			{
				this.DoSpeedSelection();
				return;
			}
			if (this.focusedList == this.scaleList)
			{
				this.DoScaleSelection();
				return;
			}
			if (this.focusedList == this.fullscreenList)
			{
				this.DoFullscreenSelection();
			}
		}

		private void GoBackToMainList()
		{
			this.focusedList.Hide();
			this.mainList.Show();
			this.focusedList = this.mainList;
		}

		private void DoCancel()
		{
			this.sfxCancel.Play();
			if (this.focusedList == this.mainList)
			{
				SceneManager.Instance.Pop();
				return;
			}
			this.GoBackToMainList();
		}

		public override void Focus()
		{
			base.Focus();
			ViewManager.Instance.Center = new Vector2f(160f, 90f);
			Engine.ClearColor = Color.Black;
			InputManager.Instance.AxisPressed += this.AxisPressed;
			InputManager.Instance.ButtonPressed += this.ButtonPressed;
		}

		public override void Unfocus()
		{
			base.Unfocus();
			InputManager.Instance.AxisPressed -= this.AxisPressed;
			InputManager.Instance.ButtonPressed -= this.ButtonPressed;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			AudioManager.Instance.Unuse(this.sfxCursorX);
			AudioManager.Instance.Unuse(this.sfxCursorY);
			AudioManager.Instance.Unuse(this.sfxConfirm);
			AudioManager.Instance.Unuse(this.sfxCancel);
		}

		private const int LIST_DISPLAY_COUNT = 8;

		private const int LIST_LINE_HEIGHT = 16;

		private const int LIST_WIDTH = 80;

		private TextRegion titleText;

		private static readonly Vector2f MENU_POSITION = new Vector2f(32f, 32f);

		private static readonly string CURSOR_FILE = Paths.GRAPHICS + "realcursor.dat";

		private ScrollingList focusedList;

		private ScrollingList mainList;

		private ScrollingList bgmVolumeList;

		private ScrollingList sfxVolumeList;

		private ScrollingList flavorList;

		private ScrollingList textSpeedList;

		private ScrollingList scaleList;

		private ScrollingList fullscreenList;

		private CarbineSound sfxCursorX;

		private CarbineSound sfxCursorY;

		private CarbineSound sfxConfirm;

		private CarbineSound sfxCancel;

		private static readonly string[] MAIN_MENU = new string[]
		{
			"BGM Volume",
			"SFX Volume",
			"Window Flavor",
			"Text Speed",
			"Window Scale",
			"Fullscreen",
			"Back"
		};

		private static readonly string[] BGM_MENU = new string[]
		{
			"100%",
			"90%",
			"80%",
			"70%",
			"60%",
			"50%",
			"40%",
			"30%",
			"20%",
			"10%",
			"Mute",
			"Back"
		};

		private static readonly string[] SFX_MENU = new string[]
		{
			"100%",
			"90%",
			"80%",
			"70%",
			"60%",
			"50%",
			"40%",
			"30%",
			"20%",
			"10%",
			"Mute",
			"Back"
		};

		private static readonly string[] FLAVOR_MENU = new string[]
		{
			"Plain",
			"Lime",
			"Strawberry",
			"Banana",
			"Peanut",
			"Blue Raspberry",
			"Grape",
			"Doom",
			"Back"
		};

		private static readonly string[] SPEED_MENU = new string[]
		{
			"Slow",
			"Average",
			"Fast",
			"Very Fast",
			"Ludicrous Speed",
			"Back"
		};

		private static readonly string[] SCALE_MENU = new string[]
		{
			"1x",
			"2x",
			"3x",
			"4x",
			"5x",
			"Back"
		};

		private static readonly string[] FULLSCREEN_MENU = new string[]
		{
			"Windowed",
			"Fullscreen",
			"Back"
		};

		private static readonly string[] VSYNC_MENU = new string[]
		{
			"Enabled",
			"Disabled",
			"Back"
		};
	}
}
