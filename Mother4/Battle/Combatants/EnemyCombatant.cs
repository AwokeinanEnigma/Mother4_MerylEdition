using System;
using Mother4.Battle.Actions;
using Mother4.Data;
using Mother4.Data.Enemies;

namespace Mother4.Battle.Combatants
{
	internal class EnemyCombatant : Combatant
	{
		public EnemyData Enemy
		{
			get
			{
				return this.enemy;
			}
		}

		public EnemyCombatant(EnemyData enemy) : base(BattleFaction.EnemyTeam)
		{
			this.enemy = enemy;
			this.stats = enemy.GetStatSet();
		}

		public override DecisionAction GetDecisionAction(BattleController controller, int priority, bool isFromUndo)
		{
			return new EnemyDecisionAction(new ActionParams
			{
				actionType = typeof(EnemyDecisionAction),
				controller = controller,
				sender = this,
				priority = priority
			},
                enemy);
		}

		private EnemyData enemy;
	}
}
