using System;
using System.Collections.Generic;
using Mother4.Battle.Actions;

namespace Mother4.Battle.Combatants
{
	internal abstract class Combatant
	{
		public event Combatant.StatChangeHandler OnStatChange;

		public event Combatant.StatusEffectChangeHandler OnStatusEffectChange;

		public BattleFaction Faction
		{
			get
			{
				return this.faction;
			}
		}

		public StatSet Stats
		{
			get
			{
				return this.stats;
			}
		}

		public Combatant[] SavedTargets
		{
			get
			{
				return this.savedTargets;
			}
			set
			{
				this.savedTargets = value;
			}
		}

		public int ID
		{
			get
			{
				return this.id;
			}
			set
			{
				if (this.id < 0)
				{
					this.id = value;
					return;
				}
				throw new InvalidOperationException("Cannot reset combatant ID.");
			}
		}

		public Combatant(BattleFaction faction)
		{
			this.faction = faction;
			this.id = -1;
			this.stats = default(StatSet);
			this.statusEffects = new Dictionary<StatusEffect, StatusEffectInstance>();
		}

		public virtual void AlterStats(StatSet statChange)
		{
			this.stats += statChange;
			if (this.OnStatChange != null)
			{
				this.OnStatChange(this, statChange);
			}
		}

		public virtual void SetStats(StatSet stats) {
			this.stats = stats;
			if (this.OnStatChange != null)
			{
				this.OnStatChange(this, stats);
			}
		}

		public virtual bool AddStatusEffect(StatusEffect effect, int turnCount)
		{
			bool flag = false;
			if (!this.statusEffects.ContainsKey(effect))
			{
				this.statusEffects.Add(effect, new StatusEffectInstance
				{
					Type = effect,
					TurnsRemaining = turnCount
				});
				flag = true;
			}
			if (flag && this.OnStatusEffectChange != null)
			{
				this.OnStatusEffectChange(this, effect, true);
			}
			return flag;
		}

		public virtual bool RemoveStatusEffect(StatusEffect effect)
		{
			bool flag = this.statusEffects.Remove(effect);
			if (flag && this.OnStatusEffectChange != null)
			{
				this.OnStatusEffectChange(this, effect, false);
			}
			return flag;
		}

		public void DecrementStatusEffect(StatusEffect effect)
		{
			if (this.statusEffects.ContainsKey(effect))
			{
				StatusEffectInstance value = this.statusEffects[effect];
				value.TurnsRemaining--;
				if (value.TurnsRemaining > 0)
				{
					this.statusEffects[effect] = value;
					return;
				}
				this.RemoveStatusEffect(value.Type);
			}
		}

		public StatusEffectInstance[] GetStatusEffects()
		{
            ICollection<StatusEffectInstance> values = this.statusEffects.Values;
			StatusEffectInstance[] array = new StatusEffectInstance[values.Count];
			values.CopyTo(array, 0);
			return array;
		}

        public bool HasStatusEffect(StatusEffect effect)
		{
            foreach (StatusEffectInstance value in statusEffects.Values)
            {
                if (value.Type == effect)
                {
                    return true;
                }
            }

            return false;
        }

        public DecisionAction GetDecisionAction(BattleController controller)
		{
			return this.GetDecisionAction(controller, 0);
		}

		public virtual DecisionAction GetDecisionAction(BattleController controller, int priority, bool isFromUndo)
		{
			throw new NotImplementedException("GetDecisionAction was not overridden.");
		}

		public DecisionAction GetDecisionAction(BattleController controller, int priority)
		{
			return this.GetDecisionAction(controller, priority, false);
		}

		private const int PRIORITY_MAGIC = 0;

		private BattleFaction faction;

		private int id;

		protected StatSet stats;

		protected Dictionary<StatusEffect, StatusEffectInstance> statusEffects;

		protected Combatant[] savedTargets;

		public delegate void StatChangeHandler(Combatant sender, StatSet change);

		public delegate void StatusEffectChangeHandler(Combatant sender, StatusEffect statusEffect, bool added);
	}
}
