using System;
using Carbine;
using Carbine.Utility;
using Mother4.Battle.Combatants;
using Mother4.Data;

namespace Mother4.Battle
{
	internal class BattleCalculator
	{
		private static bool IsSmashAttack(Combatant attacker, Combatant target)
		{
			double num = (double)attacker.Stats.Guts / 500.0 * 100.0;
			return (double)Engine.Random.Next(100) <= num;
		}

		public static int CalculatePhysicalDamage(float power, Combatant attacker, Combatant target, out bool smash)
		{
			smash = false;
			float num = power;
			if (BattleCalculator.IsSmashAttack(attacker, target))
			{
				num = 4f;
				smash = true;
			}
			double mean = (double)(num * (float)attacker.Stats.Offense - (float)attacker.Stats.Defense);
			double val = GaussianRandom.Next(mean, 2.0);
			return (int)Math.Max(0.0, val);
		}

		public static int CalculateComboDamage(float power, Combatant attacker, Combatant target, int minimum, out bool smash)
		{
			smash = false;
			float num;
			if (BattleCalculator.IsSmashAttack(attacker, target))
			{
				num = power * 0.25f;
				smash = true;
			}
			else
			{
				num = power * 0.2f;
			}
			double mean = (double)(num * (float)attacker.Stats.Offense - (float)attacker.Stats.Defense);
			double val = GaussianRandom.Next(mean, 2.0);
			return (int)Math.Max((double)minimum, val);
		}

		public static int CalculatePsiDamage(int lowerDamage, int upperDamage, Combatant attacker, Combatant target)
		{
			int num = lowerDamage + (upperDamage - lowerDamage) / 2;
			double val = GaussianRandom.Next((double)num, 3.0);
			return (int)Math.Max((double)lowerDamage, Math.Min((double)upperDamage, val));
		}

		public static int CalculateProjectileDamage(int targetDamage, Combatant attacker, Combatant target)
		{
			double val = GaussianRandom.Next((double)targetDamage, 2.0);
			return (int)Math.Max(0.0, val);
		}

		public static bool RunSuccess(CombatantController combatantController, int turnNumber)
		{
			Combatant[] factionCombatants = combatantController.GetFactionCombatants(BattleFaction.EnemyTeam);
			Combatant[] factionCombatants2 = combatantController.GetFactionCombatants(BattleFaction.PlayerTeam);
			int num = int.MinValue;
			int num2 = int.MinValue;
			foreach (Combatant combatant in factionCombatants)
			{
				if (combatant.Stats.Speed > num)
				{
					num = combatant.Stats.Speed;
				}
			}
			foreach (Combatant combatant2 in factionCombatants2)
			{
				if (combatant2.Stats.Speed > num2)
				{
					num2 = combatant2.Stats.Speed;
				}
			}
			int num3 = num2 - num + 10 * turnNumber;
			return Engine.Random.Next(100) < num3;
		}

		public static bool CalculateReflection(Combatant attacker, Combatant target)
		{
			bool flag = target is PlayerCombatant && ((PlayerCombatant)target).Character == CharacterType.Travis;
			double num = (double)attacker.Stats.Guts / 500.0 * 100.0;
			return (double)Engine.Random.Next(100) <= num && flag;
		}

		private const double PHYSICAL_STD_DEV = 2.0;

		private const double PSI_STD_DEV = 3.0;

		private const float SMAAASH_POWER = 4f;

		private const float COMBO_POWER_FACTOR = 0.2f;

		private const float COMBO_POWER_FACTOR_SMAAASH = 0.25f;

		private const double SMAAASH_DIVISOR = 500.0;

		private const double REFLECT_DIVISOR = 500.0;
	}
}
