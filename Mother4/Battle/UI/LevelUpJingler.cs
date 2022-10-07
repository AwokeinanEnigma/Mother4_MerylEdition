using System;
using System.Collections.Generic;
using Carbine.Audio;
using Mother4.Data;

namespace Mother4.Battle.UI
{
	internal class LevelUpJingler : IDisposable
	{
		public LevelUpJingler(CharacterType[] characters, bool useOutro)
		{
			this.useOutro = useOutro;
			this.baseJingle = AudioManager.Instance.Use(Paths.SFXBATTLEJINGLES + "jingleBase.wav", AudioType.Stream);
			this.baseJingle.LoopCount = -1;
			if (this.useOutro)
			{
				this.groupOutro = AudioManager.Instance.Use(Paths.SFXBATTLE + "groupOutro.wav", AudioType.Sound);
			}
			this.characterJingles = new Dictionary<CharacterType, CarbineSound>();
			foreach (CharacterType characterType in characters)
			{
				string filename = string.Format("{0}jingle{1}.{2}", Paths.SFXBATTLEJINGLES, CharacterNames.GetName(characterType), "wav");
				CarbineSound CarbineSound = AudioManager.Instance.Use(filename, AudioType.Stream);
				CarbineSound.LoopCount = -1;
				this.characterJingles.Add(characterType, CarbineSound);
			}
			this.state = LevelUpJingler.State.Stopped;
		}

		~LevelUpJingler()
		{
			this.Dispose(false);
		}

		public void Play()
		{
			if (this.state == LevelUpJingler.State.Stopped)
			{
				this.baseJingle.Play();
				foreach (CarbineSound CarbineSound in this.characterJingles.Values)
				{
					CarbineSound.Volume = 0f;
					CarbineSound.Play();
				}
				this.state = LevelUpJingler.State.Playing;
			}
		}

		public void Play(CharacterType character)
		{
			this.Play();
			if (this.characterJingles.ContainsKey(character))
			{
				AudioManager.Instance.FadeIn(this.characterJingles[character], 800U);
				this.state = LevelUpJingler.State.Playing;
			}
		}

		public void End()
		{
			if (this.useOutro)
			{
				foreach (CarbineSound sound in this.characterJingles.Values)
				{
					AudioManager.Instance.FadeOut(sound, 400U);
				}
				AudioManager.Instance.FadeOut(this.baseJingle, 400U);
				this.groupOutro.Play();
			}
			else
			{
				foreach (CarbineSound sound2 in this.characterJingles.Values)
				{
					AudioManager.Instance.FadeOut(sound2, 3000U);
				}
				AudioManager.Instance.FadeOut(this.baseJingle, 3000U);
			}
			this.state = LevelUpJingler.State.Ending;
		}

		public void Stop()
		{
			this.baseJingle.Stop();
			this.groupOutro.Stop();
			foreach (CarbineSound CarbineSound in this.characterJingles.Values)
			{
				CarbineSound.Stop();
			}
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				AudioManager.Instance.Unuse(this.baseJingle);
				if (this.useOutro)
				{
					AudioManager.Instance.Unuse(this.groupOutro);
				}
				foreach (CarbineSound sound in this.characterJingles.Values)
				{
					AudioManager.Instance.Unuse(sound);
				}
				this.disposed = true;
			}
		}

		private const string AUDIO_EXT = "wav";

		private const uint FADE_IN_DURATION = 800U;

		private const uint FADE_OUT_DURATION = 3000U;

		private const uint FADE_OUT_QUICK_DURATION = 400U;

		private bool disposed;

		private CarbineSound baseJingle;

		private CarbineSound groupOutro;

		private Dictionary<CharacterType, CarbineSound> characterJingles;

		private bool useOutro;

		private LevelUpJingler.State state;

		private enum State
		{
			Playing,
			Ending,
			Stopped
		}
	}
}
