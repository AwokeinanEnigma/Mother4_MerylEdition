namespace Carbine.Audio.Stub
{
    internal class StubSound : CarbineSound
    {
        public override bool IsPaused
        {
            get
            {
                return true;
            }
        }

        public StubSound(AudioType audioType, uint loopBegin, uint loopEnd, int loopCount, float volume, float pitch) : base(audioType, loopBegin, loopEnd, loopCount, volume, pitch)
        {
        }

        public override void Play()
        {
            base.HandleSoundCompletion();
        }

        public override void Pause()
        {
        }

        public override void Resume()
        {
        }

        public override void Stop()
        {
            base.HandleSoundCompletion();
        }

        protected override void Dispose(bool disposing)
        {
        }
    }
}
