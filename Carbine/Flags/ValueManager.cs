using fNbt;
using System.Collections.Generic;

namespace Carbine.Flags
{
    public class ValueManager
    {
        public const string NBT_TAG_NAME = "vals";

        public const int VALUE_ACTION_RETURN = 0;

        public const int VALUE_MONEY = 1;

        private static ValueManager instance;

        private readonly Dictionary<int, int> values;

        private ValueManager()
        {
            values = new Dictionary<int, int>();
        }

        public static ValueManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ValueManager();
                }

                return instance;
            }
        }

        public int this[int index]
        {
            get
            {
                if (!values.ContainsKey(index))
                {
                    return 0;
                }

                return values[index];
            }
            set
            {
                if (values.ContainsKey(index))
                {
                    values[index] = value;
                    return;
                }

                values.Add(index, value);
            }
        }

        public void Reset()
        {
            values.Clear();
        }

        public void LoadFromNBT(NbtIntArray valueTag)
        {
            values.Clear();
            if (valueTag != null)
            {
                int[] intArrayValue = valueTag.IntArrayValue;
                for (int i = 0; i < intArrayValue.Length; i += 2)
                {
                    this[intArrayValue[i]] = intArrayValue[i + 1];
                }
            }
        }

        public NbtTag ToNBT()
        {
            int[] array = new int[values.Count * 2];
            int num = 0;
            foreach (KeyValuePair<int, int> keyValuePair in values)
            {
                array[num++] = keyValuePair.Key;
                array[num++] = keyValuePair.Value;
            }

            return new NbtIntArray("vals", array);
        }
    }
}