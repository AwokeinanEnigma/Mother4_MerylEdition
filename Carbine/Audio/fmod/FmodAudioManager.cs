// Decompiled with JetBrains decompiler
// Type: Carbine.Audio.fmod.FmodAudioManager
// Assembly: Carbine, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9929100E-21E2-4663-A88C-1F977D6B46C4
// Assembly location: D:\OddityPrototypes\Carbine.dll

using FMOD;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Carbine.Audio.fmod
{
    internal class FmodAudioManager : AudioManager
    {
        private FMOD.System system;
        private Dictionary<int, CHANNEL_CALLBACK> callbacks;
        private int callbackCounter;

        public FmodAudioManager()
        {
            uint version = 0;
            int controlpaneloutputrate = 0;
            int numdrivers = 0;
            SPEAKERMODE controlpanelspeakermode = SPEAKERMODE.STEREO;
            CAPS caps = CAPS.NONE;
            FmodAudioManager.ERRCHECK(Factory.System_Create(ref this.system));
            FmodAudioManager.ERRCHECK(this.system.getVersion(ref version));
            FmodAudioManager.ERRCHECK(this.system.getNumDrivers(ref numdrivers));
            RESULT result1;
            if (numdrivers == 0)
            {
                result1 = this.system.setOutput(OUTPUTTYPE.NOSOUND);
                FmodAudioManager.ERRCHECK(result1);
            }
            else
            {
                FmodAudioManager.ERRCHECK(this.system.getDriverCaps(0, ref caps, ref controlpaneloutputrate, ref controlpanelspeakermode));
                if ((caps & CAPS.HARDWARE_EMULATED) == CAPS.HARDWARE)
                {
                    RESULT result2 = this.system.setDSPBufferSize(1024U, 10);
                    Console.WriteLine("Audio hardware acceleration is turned off. Audio performance may be degraded.");
                    FmodAudioManager.ERRCHECK(result2);
                }
                StringBuilder name = new StringBuilder(256);
                GUID guid = new GUID();
                result1 = this.system.getDriverInfo(0, name, 256, ref guid);
                FmodAudioManager.ERRCHECK(result1);
                string str = name.ToString();
                Console.WriteLine("Audio driver name: {0}", str);
                if (str.Contains("SigmaTel"))
                {
                    result1 = this.system.setSoftwareFormat(48000, SOUND_FORMAT.PCMFLOAT, 0, 0, DSP_RESAMPLER.LINEAR);
                    FmodAudioManager.ERRCHECK(result1);
                    Console.WriteLine("Sigmatel card detected; format changed to PCM floating point.");
                }
            }
            this.InitFmodSystem();
            if (result1 == RESULT.ERR_OUTPUT_CREATEBUFFER)
            {
                FmodAudioManager.ERRCHECK(this.system.setSpeakerMode(SPEAKERMODE.STEREO));
                this.InitFmodSystem();
                Console.WriteLine("Selected speaker mode is not supported, defaulting to stereo.");
            }
            this.callbacks = new Dictionary<int, CHANNEL_CALLBACK>();
        }

        private unsafe void InitFmodSystem() => FmodAudioManager.ERRCHECK(this.system.init(32, INITFLAGS.NORMAL, (IntPtr)(void*)null));

        public override void SetSpeakerMode(AudioMode mode)
        {
            SPEAKERMODE speakermode;
            switch (mode)
            {
                case AudioMode.Mono:
                    speakermode = SPEAKERMODE.MONO;
                    break;
                default:
                    speakermode = SPEAKERMODE.STEREO;
                    break;
            }
            FmodAudioManager.ERRCHECK(this.system.setSpeakerMode(speakermode));
        }

        public override void Update()
        {
            base.Update();
            int num = (int)this.system.update();
        }

        public override CarbineSound Use(string filename, AudioType type)
        {
            int hashCode = filename.GetHashCode();
            FmodSound fmodSound;
            //Console.WriteLine(string.Format("AUDIO - {0} - LINE: {1} - METHOD - {2}", filename, i, member));
            if (!this.sounds.ContainsKey(hashCode))
            {
                fmodSound = type != AudioType.Stream ? FmodAudioLoader.Instance.LoadSound(ref this.system, filename, 0, this.effectsVolume) : FmodAudioLoader.Instance.LoadStreamSound(ref this.system, filename, 0, this.musicVolume);
                this.instances.Add(hashCode, 1);
                this.sounds.Add(hashCode, fmodSound);
            }
            else
            {
                fmodSound = (FmodSound)this.sounds[hashCode];
                Dictionary<int, int> instances;
                int key;
                (instances = this.instances)[key = hashCode] = instances[key] + 1;
            }
            return fmodSound;
        }

        public override void Unuse(CarbineSound sound)
        {
            int key1 = 0;
            CarbineSound CarbineSound = null;
            foreach (KeyValuePair<int, CarbineSound> sound1 in this.sounds)
            {
                key1 = sound1.Key;
                CarbineSound = sound1.Value;
                if (CarbineSound == sound)
                {
                    Dictionary<int, int> instances;
                    int key2;
                    (instances = this.instances)[key2 = key1] = instances[key2] - 1;
                    break;
                }
            }
            if (CarbineSound == null || this.instances[key1] > 0)
            {
                return;
            }

            //Console.WriteLine("Cleaning up audio");
            this.instances.Remove(key1);
            this.sounds.Remove(key1);
            CarbineSound.Dispose();
        }

        public int AddCallback(CHANNEL_CALLBACK callback)
        {
            this.callbacks.Add(++this.callbackCounter, callback);
            return this.callbackCounter;
        }

        public void RemoveCallback(int index) => this.callbacks.Remove(index);

        public static void ERRCHECK(RESULT result)
        {

            if (result != RESULT.OK)
            {
                Console.WriteLine(($"There was an error trying to play the song for the map!!! "));
            }
            //throw new FmodException(string.Format("FMOD error: {0} - {1}", (object)result, (object)Error.String(result)));
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (this.disposed || this.system == null)
            {
                return;
            }

            FmodAudioManager.ERRCHECK(this.system.close());
            FmodAudioManager.ERRCHECK(this.system.release());
        }
    }
}
