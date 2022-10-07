using System;

namespace Carbine.Audio
{
    public abstract class CarbineSound : IDisposable
    {
        public virtual uint LoopBegin
        {
            get
            {
                return this.loopBegin;
            }
            set
            {
                this.loopBegin = value;
            }
        }

        public virtual uint LoopEnd
        {
            get
            {
                return this.loopEnd;
            }
            set
            {
                this.loopEnd = value;
            }
        }

        public virtual int LoopCount
        {
            get
            {
                return this.loopCount;
            }
            set
            {
                this.loopCount = value;
            }
        }

        public virtual float Volume
        {
            get
            {
                return this.volume;
            }
            set
            {
                this.volume = Math.Max(0f, Math.Min(1f, value));
            }
        }

        public virtual float Pitch
        {
            get
            {
                return this.pitch;
            }
            set
            {
                this.pitch = value;
            }
        }

        public virtual uint Position
        {
            get
            {
                return this.position;
            }
            set
            {
                this.position = value;
            }
        }

        public AudioType AudioType
        {
            get
            {
                return this.type;
            }
        }

        public virtual bool IsPaused
        {
            get
            {
                throw new NotImplementedException("IsPaused not implemented.");
            }
        }

        public event CarbineSound.OnCompleteHandler OnComplete;

        public CarbineSound(AudioType soundType, uint loopBegin, uint loopEnd, int loopCount, float volume, float pitch)
        {
            this.loopBegin = loopBegin;
            this.loopEnd = loopEnd;
            this.loopCount = loopCount;
            this.volume = volume;
            this.pitch = pitch;
            this.type = soundType;
        }

        ~CarbineSound()
        {
            this.Dispose(false);
        }

        public abstract void Play();

        public abstract void Pause();

        public abstract void Resume();

        public abstract void Stop();

        protected void HandleSoundCompletion()
        {
            if (this.OnComplete != null)
            {
                this.OnComplete(this);
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected abstract void Dispose(bool disposing);

        protected uint position;

        protected int loopCount;

        protected uint loopBegin;

        protected uint loopEnd;

        protected float volume;

        protected float pitch;

        protected AudioType type;

        protected bool disposed;

        public delegate void OnCompleteHandler(CarbineSound sender);
    }
}
