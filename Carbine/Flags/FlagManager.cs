using fNbt;
using System.Collections.Generic;

namespace Carbine.Flags
{
    /// <summary>
    /// Handles 'flags', values that can be set on the fly and also used, on the fly.
    /// </summary>
    public class FlagManager
    {
        public const string NBT_TAG_NAME = "flags";

        public const int FLAG_TRUE = 0;

        public const int FLAG_DAY_NIGHT = 1;

        public const int FLAG_QUESTION_REGISTER = 2;

        public const int FLAG_TELEPATHY_REQUEST = 3;

        public const int FLAG_TELEPATHY_MODE = 4;

        private static FlagManager instance;

        private readonly Dictionary<int, bool> flags;

        private FlagManager()
        {
            flags = new Dictionary<int, bool>();
            SetInitialState();
        }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public static FlagManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FlagManager();
                }

                return instance;
            }
        }

        public bool this[int flag]
        {
            get => flags.ContainsKey(flag) && flags[flag];
            set
            {
                if (flag > 0)
                {
                    if (flags.ContainsKey(flag))
                    {
                        flags[flag] = value;
                        return;
                    }

                    flags.Add(flag, value);
                }
            }
        }

        private void SetInitialState()
        {
            flags.Add(0, true);
        }

        public void Toggle(int flag)
        {
            if (flags.ContainsKey(flag))
            {
                flags[flag] = !flags[flag];
                return;
            }

            flags.Add(flag, true);
        }

        public void Reset()
        {
            flags.Clear();
            SetInitialState();
        }

        public void LoadFromNBT(NbtIntArray flagTag)
        {
            Reset();
            if (flagTag != null)
            {
                foreach (int num in flagTag.IntArrayValue)
                {
                    int flag = num >> 1;
                    bool value = (num & 1) == 1;
                    this[flag] = value;
                }
            }
        }

        public NbtIntArray ToNBT()
        {
            int[] array = new int[flags.Count - 1];
            int num = 0;
            foreach (KeyValuePair<int, bool> keyValuePair in flags)
            {
                if (keyValuePair.Key > 0)
                {
                    array[num++] = (keyValuePair.Key << 1) | (keyValuePair.Value ? 1 : 0);
                }
            }

            return new NbtIntArray("flags", array);
        }
    }
}