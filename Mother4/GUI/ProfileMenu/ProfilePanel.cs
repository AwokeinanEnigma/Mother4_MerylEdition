using System;
using Carbine.Graphics;
using Carbine.GUI;
using Carbine.Input;
using Carbine.Maps;
using Mother4.Data;
using Rufini.Strings;
using SFML.System;

namespace Mother4.GUI.ProfileMenu
{
	internal class ProfilePanel : MenuPanel
	{
		public ProfilePanel(Vector2f position, Vector2f size, int index, SaveProfile profile) : base(position, size, 0, WindowBox.Style.Normal, (uint)profile.Flavor)
		{
			TextRegion control = new TextRegion(new Vector2f(1f, -3f), 0, Fonts.Main, string.Format("#{0}", index + 1));
			base.Add(control);
			if (profile.IsValid)
			{
				this.SetupForFile(profile);
				return;
			}
			TextRegion control2 = new TextRegion(new Vector2f(16f, 10f), 0, Fonts.Main, StringFile.Instance.Get("system.noSaveData").Value);
			base.Add(control2);
		}

		private void SetupForFile(SaveProfile profile)
		{
			int num = 0;
			int num2 = Math.Min(4, profile.Party.Length);
			for (int i = 0; i < num2; i++)
			{
				Graphic graphic = new IndexedColorGraphic(CharacterGraphics.GetFile(profile.Party[i]), "walk south", new Vector2f((float)(24 + num), this.size.Y - 2f), 0);
				graphic.SpeedModifier = 0f;
				num += 4 + (int)graphic.Size.X;
				base.Add(graphic);
			}
			string[] array = MapLoader.LoadTitle(Paths.MAPS + profile.MapName);
			TextRegion control = new TextRegion(new Vector2f(128f, -3f), 0, Fonts.Main, array[0]);
			base.Add(control);
			TextRegion control2 = new TextRegion(new Vector2f(128f, 12f), 0, Fonts.Main, array[1]);
			base.Add(control2);
			TimeSpan timeSpan = TimeSpan.FromSeconds((double)profile.Time);
			TextRegion textRegion = new TextRegion(default(Vector2f), 0, Fonts.Main, string.Format("{0:00}:{1:00}:{2:00}", (int)timeSpan.TotalHours, (int)timeSpan.TotalMinutes % 60, (int)timeSpan.TotalSeconds % 60));
			textRegion.Position = new Vector2f((float)((int)(this.size.X - textRegion.Size.X) - 2), -3f);
			base.Add(textRegion);
		}

		public override object ButtonPressed(Button b)
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

		private const int DEPTH = 0;

		private const int PLAYER_CHARACTER_COUNT = 4;
	}
}
