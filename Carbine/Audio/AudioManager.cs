using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Carbine.Audio.fmod;

namespace Carbine.Audio
{
    public abstract class AudioManager : IDisposable
    {
        public static AudioManager Instance
        {
            get
            {
                if (AudioManager.instance == null)
                {
                    AudioManager.instance = new FmodAudioManager();
                }
                return AudioManager.instance;
            }
        }

        public float EffectsVolume
        {
            get
            {
                return this.effectsVolume;
            }
            set
            {
                this.effectsVolume = value;
                this.UpdateSoundVolume();
            }
        }

        public float MusicVolume
        {
            get
            {
                return this.musicVolume;
            }
            set
            {
                this.musicVolume = value;
                this.UpdateSoundVolume();
            }
        }

        public CarbineSound BGM
        {
            get
            {
                return this.bgmSound;
            }
        }

        public AudioManager()
        {
            this.instances = new Dictionary<int, int>();
            this.sounds = new Dictionary<int, CarbineSound>();
            this.faders = new List<AudioManager.Fader>();
            this.deadFaders = new List<AudioManager.Fader>();
        }

        ~AudioManager()
        {
            this.Dispose(false);
        }

        public virtual void Update()
        {
            this.UpdateFaders();
        }

        private void UpdateSoundVolume()
        {
            foreach (CarbineSound CarbineSound in this.sounds.Values)
            {
                CarbineSound.Volume = ((CarbineSound.AudioType == AudioType.Stream) ? this.musicVolume : this.effectsVolume);
            }
        }

        private void UpdateFaders()
        {
            for (int i = 0; i < this.faders.Count; i++)
            {
                AudioManager.Fader fader = this.faders[i];
                fader.ticks += 16U;
                float num = fader.ticks / fader.duration;
                fader.sound.Volume = fader.fromVolume + (fader.toVolume - fader.fromVolume) * num;
                if (fader.ticks >= fader.duration)
                {
                    fader.sound.Volume = fader.toVolume;
                    this.deadFaders.Add(fader);
                }
            }
            for (int j = 0; j < this.deadFaders.Count; j++)
            {
                AudioManager.Fader fader2 = this.deadFaders[j];
                if (fader2.stopOnEnd)
                {
                    fader2.sound.Stop();
                }
                this.faders.Remove(fader2);
            }
            this.deadFaders.Clear();
        }

        private void Fade(CarbineSound sound, uint duration, float volume, bool stopOnEnd)
        {
            AudioManager.Fader item = new AudioManager.Fader
            {
                sound = sound,
                duration = duration,
                ticks = 0U,
                fromVolume = sound.Volume,
                toVolume = volume,
                stopOnEnd = stopOnEnd
            };
            this.faders.Add(item);
        }

        public void Fade(CarbineSound sound, uint duration, float volume)
        {
            this.Fade(sound, duration, volume, false);
        }

        public void FadeOut(CarbineSound sound, uint duration)
        {
            this.Fade(sound, duration, 0f, true);
        }

        public void FadeIn(CarbineSound sound, uint duration)
        {
            sound.Play();
            this.Fade(sound, duration, 1f, false);
        }

        public void SetBGM(string name)
        {
            Console.WriteLine($"REQUESTING BGM: {name}");
            CarbineSound bgm = this.Use(name, AudioType.Stream);
            this.SetBGM(bgm);
        }

        private void SetBGM(CarbineSound newBGM)
        {
            if (this.bgmSound != null)
            {
                this.Unuse(this.bgmSound);
            }
            this.bgmSound = newBGM;
        }

        public abstract void SetSpeakerMode(AudioMode mode);

        public abstract CarbineSound Use(string filename, AudioType type);

        public abstract void Unuse(CarbineSound sound);

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                foreach (CarbineSound CarbineSound in this.sounds.Values)
                {
                    CarbineSound.Dispose();
                }
            }
            this.disposed = true;
        }

        private static AudioManager instance;

        private List<AudioManager.Fader> faders;

        private List<AudioManager.Fader> deadFaders;

        protected Dictionary<int, int> instances;

        protected Dictionary<int, CarbineSound> sounds;

        protected float musicVolume;

        protected float effectsVolume;

        protected bool disposed;

        protected CarbineSound bgmSound;

        private class Fader
        {
            public CarbineSound sound;

            public uint ticks;

            public uint duration;

            public float fromVolume;

            public float toVolume;

            public bool stopOnEnd;
        }
    }
}
