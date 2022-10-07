using System;
using System.Collections.Generic;
using Mother4.Battle;

namespace Mother4.Data
{
	internal static class CharacterStats
	{
		public static StatSet GetStats(CharacterType character)
		{
			StatSet result;
			CharacterStats.stats.TryGetValue(character, out result);
			return result;
		}

		public static void SetStats(CharacterType character, StatSet statset)
		{
			if (CharacterStats.stats.ContainsKey(character))
			{
				CharacterStats.stats[character] = statset;
			}
		}

		private static Dictionary<CharacterType, StatSet> stats = new Dictionary<CharacterType, StatSet>
		{
			{
				CharacterType.Travis,
				new StatSet
				{
					MaxHP = 98,
					MaxPP = 50,
					HP = 84,
					PP = 45,
					Speed = 8,
					Offense = 9,
					Defense = 5,
					IQ = 3,
					Guts = 80,
					Luck = 2,
					Meter = 0.12f,
					Level = 16
				}
			},
			{
				CharacterType.Meryl,
				new StatSet
				{
					HP = 21,
					PP = 34,
					Speed = 8,
					Offense = 4,
					Defense = 5,
					IQ = 3,
					Guts = 2,
					Luck = 2,
					Meter = 0.5f,
					Level = 1
				}
			},
			{
				CharacterType.Floyd,
				new StatSet
				{
					HP = 31,
					PP = 0,
					Speed = 10,
					Offense = 5,
					Defense = 5,
					IQ = 3,
					Guts = 2,
					Luck = 2,
					Meter = 0.5f,
					Level = 1
				}
			},
			{
				CharacterType.Leo,
				new StatSet
				{
					HP = 42,
					PP = 30,
					Speed = 5,
					Offense = 12,
					Defense = 6,
					IQ = 3,
					Guts = 2,
					Luck = 2,
					Meter = 0.5f,
					Level = 1
				}
			},
			{
				CharacterType.Zack,
				new StatSet
				{
					HP = 15,
					PP = 0,
					Speed = 1,
					Offense = 6,
					Defense = 1,
					IQ = 1,
					Guts = 1,
					Luck = 1,
					Meter = 0.5f,
					Level = 1
				}
			}
		};
	}
}
