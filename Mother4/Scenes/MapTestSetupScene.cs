using System;
using System.Collections.Generic;
using System.IO;
using Carbine;
using Carbine.Audio;
using Carbine.Flags;
using Carbine.Graphics;
using Carbine.GUI;
using Carbine.Input;
using Carbine.Scenes;
using Carbine.Scenes.Transitions;
using Mother4.Data;
using Mother4.GUI;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Scenes
{
	internal class MapTestSetupScene : StandardScene
	{
		public MapTestSetupScene()
		{
			string[] files = Directory.GetFiles(Paths.MAPS);
			this.mapItems = new string[files.Length];
			for (int i = 0; i < files.Length; i++)
			{
				this.mapItems[i] = Path.GetFileName(files[i]);
			}
			this.selectedMap = this.mapItems[0];
			string[] items = new string[]
			{
				"Select Characters",
				string.Format("Select Map ({0})", this.selectedMap),
				"Map Test Options",
				"Start",
				"Back"
			};
			this.optionList = new ScrollingList(new Vector2f(32f, 80f), 0, items, 5, 16f, 80f, Paths.GRAPHICS + "realcursor.dat");
			this.optionList.ShowSelectionRectangle = false;
			this.pipeline.Add(this.optionList);
			this.focusedList = this.optionList;
			this.mapList = new ScrollingList(new Vector2f(32f, 80f), 0, this.mapItems, 5, 16f, 256f, Paths.GRAPHICS + "realcursor.dat");
			this.mapList.ShowSelectionRectangle = false;
			this.pipeline.Add(this.mapList);
			this.mapList.Hide();
			string[] names = Enum.GetNames(typeof(CharacterType));
			string[] array = new string[names.Length + 1];
			array[0] = "Remove";
			Array.Copy(names, 0, array, 1, names.Length);
			this.selectedCharacters = new List<CharacterType>();
			this.selectedCharacters.Add(CharacterType.Travis);
			this.ResetCharacterGraphics();
			this.charactersList = new ScrollingList(new Vector2f(32f, 80f), 0, array, 5, 16f, 256f, Paths.GRAPHICS + "realcursor.dat");
			this.charactersList.ShowSelectionRectangle = false;
			this.pipeline.Add(this.charactersList);
			this.charactersList.Hide();
			string[] items2 = new string[]
			{
				string.Format("Run scripts on map load: {0}", this.runScriptsOnLoad),
				string.Format("Start at night: {0}", FlagManager.Instance[1]),
				"Back"
			};
			this.settingsList = new ScrollingList(new Vector2f(32f, 80f), 0, items2, 5, 16f, 256f, Paths.GRAPHICS + "realcursor.dat");
			this.settingsList.ShowSelectionRectangle = false;
			this.pipeline.Add(this.settingsList);
			this.settingsList.Hide();
			this.titleText = new TextRegion(new Vector2f(4f, 4f), 0, Fonts.Title, "Map Test Setup");
			this.pipeline.Add(this.titleText);
			this.sfxCursorX = AudioManager.Instance.Use(Paths.SFXMENU + "cursorx.wav", AudioType.Sound);
			this.sfxCursorY = AudioManager.Instance.Use(Paths.SFXMENU + "cursory.wav", AudioType.Sound);
			this.sfxConfirm = AudioManager.Instance.Use(Paths.SFXMENU + "confirm.wav", AudioType.Sound);
			this.sfxCancel = AudioManager.Instance.Use(Paths.SFXMENU + "cancel.wav", AudioType.Sound);
			FlagManager.Instance.Reset();
			ValueManager.Instance.Reset();
		}

		private void ResetCharacterGraphics()
		{
			if (this.characterSprites != null)
			{
				for (int i = 0; i < this.characterSprites.Length; i++)
				{
					if (this.characterSprites[i] != null)
					{
						this.pipeline.Remove(this.characterSprites[i]);
						this.characterSprites[i].Dispose();
					}
				}
			}
			this.characterSprites = new IndexedColorGraphic[this.selectedCharacters.Count];
			for (int j = 0; j < this.selectedCharacters.Count; j++)
			{
				Vector2f position = new Vector2f((float)(32 + 32 * j), 64f);
				this.characterSprites[j] = new IndexedColorGraphic(CharacterGraphics.GetFile(this.selectedCharacters[j]), "walk south", position, 0);
				if (this.characterSprites[j].GetSpriteDefinition("idle south") != null)
				{
					this.characterSprites[j].SetSprite("idle south");
				}
				if (this.characterSprites[j] != null)
				{
					this.pipeline.Add(this.characterSprites[j]);
				}
			}
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

		private void DoSelection()
		{
			this.sfxConfirm.Play();
			if (this.focusedList == this.optionList)
			{
				switch (this.optionList.SelectedIndex)
				{
				case 0:
					this.optionList.Hide();
					this.charactersList.Show();
					this.focusedList = this.charactersList;
					return;
				case 1:
					this.optionList.Hide();
					this.mapList.Show();
					this.focusedList = this.mapList;
					return;
				case 2:
					this.optionList.Hide();
					this.settingsList.Show();
					this.focusedList = this.settingsList;
					return;
				case 3:
					if (this.selectedCharacters.Count > 0)
					{
						AudioManager.Instance.FadeOut(AudioManager.Instance.BGM, 1500U);
						PartyManager.Instance.Clear();
						PartyManager.Instance.AddAll(this.selectedCharacters);
						SceneManager.Instance.Transition = new ColorFadeTransition(0.5f, Color.Black);
						SceneManager.Instance.Push(new OverworldScene(this.selectedMap, this.runScriptsOnLoad));
						return;
					}
					break;
				case 4:
					SceneManager.Instance.Pop();
					return;
				default:
					return;
				}
			}
			else
			{
				if (this.focusedList == this.mapList)
				{
					this.selectedMap = this.mapItems[this.mapList.SelectedIndex];
					this.optionList.ChangeItem(1, string.Format("Select Map ({0})", this.mapItems[this.mapList.SelectedIndex]));
					this.mapList.Hide();
					this.optionList.Show();
					this.focusedList = this.optionList;
					return;
				}
				if (this.focusedList == this.charactersList)
				{
					int selectedIndex = this.charactersList.SelectedIndex;
					if (selectedIndex > 0)
					{
						this.selectedCharacters.Add((CharacterType)(selectedIndex - 1));
					}
					else if (this.selectedCharacters.Count > 0)
					{
						this.selectedCharacters.RemoveAt(this.selectedCharacters.Count - 1);
					}
					this.ResetCharacterGraphics();
					this.charactersList.Hide();
					this.optionList.Show();
					this.focusedList = this.optionList;
					return;
				}
				if (this.focusedList == this.settingsList)
				{
					switch (this.settingsList.SelectedIndex)
					{
					case 0:
						this.runScriptsOnLoad = !this.runScriptsOnLoad;
						this.settingsList.ChangeItem(0, string.Format("Run scripts on map load: {0}", this.runScriptsOnLoad));
						return;
					case 1:
						FlagManager.Instance.Toggle(1);
						this.settingsList.ChangeItem(1, string.Format("Start at night: {0}", FlagManager.Instance[1]));
						return;
					case 2:
						this.settingsList.Hide();
						this.optionList.Show();
						this.focusedList = this.optionList;
						break;
					default:
						return;
					}
				}
			}
		}

		private void DoCancel()
		{
			if (this.focusedList == this.optionList)
			{
				SceneManager.Instance.Pop();
				return;
			}
			this.focusedList.Hide();
			this.optionList.Show();
			this.focusedList = this.optionList;
		}

		public override void Focus()
		{
			base.Focus();
			ViewManager.Instance.Center = new Vector2f(160f, 90f);
			Engine.ClearColor = Color.Black;
			AudioManager.Instance.SetBGM(Paths.BGMOVERWORLD + "test.mp3");
			AudioManager.Instance.BGM.Play();
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

		private const string MAP_FORMAT_SEL_MAP = "Select Map ({0})";

		private const string OPT_FORMAT_RUN_SCRIPTS = "Run scripts on map load: {0}";

		private const string OPT_FORMAT_NIGHTTIME = "Start at night: {0}";

		private TextRegion titleText;

		private ScrollingList focusedList;

		private ScrollingList optionList;

		private ScrollingList mapList;

		private ScrollingList charactersList;

		private ScrollingList settingsList;

		private CarbineSound sfxCursorX;

		private CarbineSound sfxCursorY;

		private CarbineSound sfxConfirm;

		private CarbineSound sfxCancel;

		private string selectedMap;

		private List<CharacterType> selectedCharacters;

		private IndexedColorGraphic[] characterSprites;

		private string[] mapItems;

		private bool runScriptsOnLoad = true;
	}
}
