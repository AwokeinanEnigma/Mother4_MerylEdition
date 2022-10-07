using System;
using Mother4.Battle.Combatants;
using Mother4.Battle.EnemyAI;
using Mother4.Data;
using Mother4.Data.Enemies;

namespace Mother4.Battle.Actions
{
	internal class EnemyDecisionAction : DecisionAction
	{
		public EnemyDecisionAction(ActionParams aparams, EnemyData data) : base(aparams)
		{
			this.enemyType = (this.sender as EnemyCombatant).Enemy;
            switch (enemyType.AIName)
            {
                case "TravisMustDie":
                    aicontrol = new TravisMustDieAI(controller, sender, data);
                    break;
                case "BoilerPlateAI":
                    aicontrol = new BoilerplateAI(controller, sender, data);
                    break;
				case "FrooguAI":
					aicontrol = new FrooguAI(controller, Sender, data);
					break;
				case "hermitcanai":
					aicontrol = new HermitCanAI(controller, Sender, data);
					break;
                default:
                    aicontrol = new BoilerplateAI(controller, sender, data);
                    break;
            }

			//this.aicontrol = new RandomAI(this.controller, this.sender);
		}

		protected override void UpdateAction()
		{
			base.UpdateAction();
			bool flag = false;
			StatusEffectInstance[] statusEffects = this.sender.GetStatusEffects();
			if (statusEffects.Length > 0)
			{
				foreach (StatusEffectInstance statusEffectInstance in statusEffects)
				{
					Type type = StatusEffectActions.Get(statusEffectInstance.Type);
					if (type != null)
					{
						ActionParams aparams = new ActionParams
						{
							actionType = type,
							controller = this.controller,
							sender = this.sender,
							priority = this.sender.Stats.Speed,
							targets = this.sender.SavedTargets,
							data = new object[]
							{
								statusEffectInstance
							}
						};
						this.controller.AddAction(BattleAction.GetInstance(aparams));
						flag |= (statusEffectInstance.Type == StatusEffect.Talking);
					}
				}
			}
			if (!flag)
			{
				Combatant[] factionCombatants = this.controller.CombatantController.GetFactionCombatants(BattleFaction.PlayerTeam);
				BattleAction action = this.aicontrol.GetAction(this.sender.Stats.Speed, factionCombatants);
				this.controller.AddAction(action);
			}
			this.complete = true;
		}

		private EnemyData enemyType;

		private IEnemyAI aicontrol;
	}
}
