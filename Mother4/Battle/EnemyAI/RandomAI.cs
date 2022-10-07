using System;
using System.Collections.Generic;
using Carbine;
using Mother4.Battle.Actions;
using Mother4.Battle.Combatants;
using Mother4.Data;

namespace Mother4.Battle.EnemyAI
{
	internal class RandomAI : IEnemyAI
	{
		public RandomAI(BattleController controller, Combatant sender)
		{
			this.controller = controller;
			this.sender = sender;
			//this.battleActionParams = EnemyBattleActions.GetBattleActionParams((sender as EnemyCombatant).Enemy);
		}

		public BattleAction GetAction(int priority, Combatant[] potentialTargets)
		{
			Combatant[] targets = new Combatant[]
			{
				potentialTargets[Engine.Random.Next(potentialTargets.Length)]
			};
			ActionParams aparams = this.battleActionParams[Engine.Random.Next(this.battleActionParams.Count)];
			aparams.controller = this.controller;
			aparams.sender = this.sender;
			aparams.priority = this.sender.Stats.Speed;
			aparams.targets = targets;
			return BattleAction.GetInstance(aparams);
		}

		private List<ActionParams> battleActionParams;

		private BattleController controller;

		private Combatant sender;
	}
}
