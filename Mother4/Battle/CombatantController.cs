using System;
using System.Collections.Generic;
using System.Linq;
using Mother4.Battle.Combatants;
using Mother4.Data;
using Mother4.Data.Enemies;

namespace Mother4.Battle
{
	internal class CombatantController
	{
		public List<Combatant> CombatantList
		{
			get
			{
				return this.combatants.Values.ToList<Combatant>();
			}
		}

		public Combatant this[int i]
		{
			get
			{
				return this.combatants[i];
			}
		}

		public CombatantController(CharacterType[] party, EnemyData[] enemies)
		{
			this.uidCounter = 0;
			this.combatants = new Dictionary<int, Combatant>();
			for (int i = 0; i < party.Length; i++)
			{
				this.Add(new PlayerCombatant(party[i], i));
			}
			for (int j = 0; j < enemies.Length; j++)
			{
				this.Add(new EnemyCombatant(enemies[j]));
			}
		}

		public int Add(Combatant c)
		{
			c.ID = this.uidCounter;
			this.combatants.Add(this.uidCounter, c);
			this.uidCounter++;
			return c.ID;
		}

		public void Remove(Combatant c)
		{
			foreach (KeyValuePair<int, Combatant> keyValuePair in this.combatants.ToArray<KeyValuePair<int, Combatant>>())
			{
				if (keyValuePair.Value == c)
				{
					this.combatants.Remove(keyValuePair.Key);
				}
			}
		}

		public bool IsIdValid(int id)
		{
			return this.combatants.ContainsKey(id);
		}

		public Combatant GetFirstLiveCombatant(BattleFaction faction)
		{
			Combatant[] factionCombatants = this.GetFactionCombatants(faction);
			Combatant result = null;
			int num = int.MaxValue;
			foreach (Combatant combatant in factionCombatants)
			{
				if (combatant.ID < num && combatant.Stats.HP > 0)
				{
					num = combatant.ID;
					result = combatant;
				}
			}
			return result;
		}

		public int SelectFirst(BattleFaction faction)
		{
			Combatant[] factionCombatants = this.GetFactionCombatants(faction);
			int num = int.MaxValue;
			foreach (Combatant combatant in factionCombatants)
			{
				if (combatant.ID < num)
				{
					num = combatant.ID;
				}
			}
			if (num < 2147483647)
			{
				return num;
			}
			return int.MinValue;
		}

		public int SelectNext(BattleFaction faction, int id)
		{
			Combatant[] factionCombatants = this.GetFactionCombatants(faction);
			int num = int.MaxValue;
			foreach (Combatant combatant in factionCombatants)
			{
				if (combatant.ID > id && combatant.ID < num)
				{
					num = combatant.ID;
				}
			}
			if (num < 2147483647)
			{
				return num;
			}
			return int.MinValue;
		}

		public int SelectPrevious(BattleFaction faction, int id)
		{
			Combatant[] factionCombatants = this.GetFactionCombatants(faction);
			int num = int.MaxValue;
			foreach (Combatant combatant in factionCombatants)
			{
				if (combatant.ID > id && combatant.ID < num)
				{
					num = combatant.ID;
				}
			}
			if (num < 2147483647)
			{
				return num;
			}
			return int.MinValue;
		}

		public Combatant[] GetFactionCombatants(BattleFaction faction)
		{
			return this.GetFactionCombatants(faction, false);
		}

		public Combatant[] GetFactionCombatants(BattleFaction faction, bool alive)
		{
			List<Combatant> list = new List<Combatant>();
			foreach (Combatant combatant in this.combatants.Values)
			{
				if (combatant.Faction == faction && (!alive || combatant.Stats.HP > 0))
				{
					list.Add(combatant);
				}
			}
			return list.ToArray();
		}

		private int uidCounter;

		private Dictionary<int, Combatant> combatants;
	}
}
