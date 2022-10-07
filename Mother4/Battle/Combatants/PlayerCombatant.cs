using System;
using Mother4.Battle.Actions;
using Mother4.Data;

namespace Mother4.Battle.Combatants
{
	internal class PlayerCombatant : Combatant
	{
		public CharacterType Character
		{
			get
			{
				return this.character;
			}
		}

		public int PartyIndex
		{
			get
			{
				return this.partyIndex;
			}
		}

		public PlayerCombatant(CharacterType character, int partyIndex) : base(BattleFaction.PlayerTeam)
		{
			this.character = character;
			this.partyIndex = partyIndex;
			this.stats = CharacterStats.GetStats(character);
		}

		public override DecisionAction GetDecisionAction(BattleController controller, int priority, bool isFromUndo)
		{
			ActionParams aparams = new ActionParams
			{
				actionType = typeof(PlayerDecisionAction),
				controller = controller,
				sender = this,
				priority = priority,
				data = new object[]
				{
					false,
					isFromUndo
				}
			};
			return new PlayerDecisionAction(aparams);
		}

		public void HandleStatusChangeFromOther(Combatant sender, StatusEffect statusEffect, bool added)
		{
			if (sender.Faction == BattleFaction.EnemyTeam && !added && statusEffect == StatusEffect.Talking)
			{
				this.RemoveStatusEffect(StatusEffect.Talking);
				sender.OnStatusEffectChange -= this.HandleStatusChangeFromOther;
			}
		}

		private CharacterType character;

		private int partyIndex;
	}
}
