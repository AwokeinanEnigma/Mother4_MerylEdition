using System;

namespace Mother4.Battle
{
	internal struct StatSet
	{
		public StatSet(StatSet set)
		{
			this.HP = set.HP;
			this.PP = set.PP;
			this.Defense = set.Defense;
			this.Guts = set.Guts;
			this.IQ = set.IQ;
			this.Luck = set.Luck;
			this.Offense = set.Offense;
			this.Speed = set.Speed;
			this.MaxHP = set.MaxHP;
			this.MaxPP = set.MaxPP;
			this.Meter = set.Meter;
			this.Experience = set.Experience;
			this.Level = set.Level;
		}

		public static StatSet operator +(StatSet set1, StatSet set2)
		{
			StatSet result = new StatSet(set1);
			result.Defense += set2.Defense;
			result.Guts += set2.Guts;
			result.HP += set2.HP;
			result.IQ += set2.IQ;
			result.Luck += set2.Luck;
			result.Offense += set2.Offense;
			result.PP += set2.PP;
			result.Speed += set2.Speed;
			result.MaxHP += set2.MaxHP;
			result.MaxPP += set2.MaxPP;
			result.Meter += set2.Meter;
			result.Level += set2.Level;
			result.Experience += set2.Experience;
			return result;
		}



		public int Level;

		public int Experience;

		public int HP;

		public int MaxHP;

		public int PP;

		public int MaxPP;

		public int Speed;

		public int Defense;

		public int Offense;

		public int IQ;

		public int Guts;

		public int Luck;

		public float Meter;
	}
}
