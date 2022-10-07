using System;
using System.Collections.Generic;

namespace Mother4.Data
{
	internal static class CharacterComboSounds
	{
		public static string Get(CharacterType character, int type, int index, int bpm)
		{
			CharacterType key = character;
			if (!CharacterComboSounds.prefixes.ContainsKey(key))
			{
				key = CharacterType.Travis;
			}
			int num = 0;
			int num2 = int.MaxValue;
			for (int i = 0; i < CharacterComboSounds.BPMS.Length; i++)
			{
				int num3 = Math.Abs(bpm - CharacterComboSounds.BPMS[i]);
				if (num3 < num2)
				{
					num2 = num3;
					num = i;
					if (num3 == 0)
					{
						break;
					}
				}
			}
			return string.Format("{0}{1}{2}-{3}.{4}", new object[]
			{
				CharacterComboSounds.prefixes[key],
				CharacterComboSounds.TYPES[type % CharacterComboSounds.TYPES.Length],
				index % 3 + 1,
				CharacterComboSounds.BPMS[num],
				"wav"
			});
		}

		private const string EXTENSION = "wav";

		private const int INDEXES = 3;

		private static char[] TYPES = new char[]
		{
			'A',
			'B',
			'C'
		};

		private static int[] BPMS = new int[]
		{
			120
		};

		private static Dictionary<CharacterType, string> prefixes = new Dictionary<CharacterType, string>
		{
			{
				CharacterType.Travis,
				"TravisCombo"
			},
			{
				CharacterType.Floyd,
				"FloydCombo"
			},
			{
				CharacterType.Meryl,
				"MerylCombo"
			}
		};
	}
}
