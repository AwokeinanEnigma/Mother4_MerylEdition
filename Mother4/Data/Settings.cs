using System;
using Carbine.GUI;

namespace Mother4.Data
{
	internal class Settings
	{
		public static string Locale
		{
			get
			{
				return Settings.language;
			}
			set
			{
				Settings.language = value;
			}
		}

		public static uint WindowFlavor
		{
			get
			{
				return Settings.windowFlavor;
			}
			set
			{
				Settings.windowFlavor = value;
			}
		}

		public static WindowBox.Style WindowStyle
		{
			get
			{
				return WindowBox.Style.Normal;
			}
		}

		public static float EffectsVolume
		{
			get
			{
				return Settings.sfxVolume;
			}
			set
			{
				Settings.sfxVolume = value;
			}
		}

		public static float MusicVolume
		{
			get
			{
				return Settings.bgmVolume;
			}
			set
			{
				Settings.bgmVolume = value;
			}
		}

		public static int TextSpeed
		{
			get
			{
				return Settings.textSpeed;
			}
			set
			{
				Settings.textSpeed = value;
			}
		}

		private static string language = "en_US";

		private static uint windowFlavor = 0U;

		private static float bgmVolume = 0.7f;

		private static float sfxVolume = 0.9f;

		private static int textSpeed = 2;
	}
}
