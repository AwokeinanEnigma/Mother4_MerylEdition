using System;
using System.Collections.Generic;
using Carbine;
using Mother4.Battle.Actions;
using Mother4.Battle.Combatants;
using Mother4.Data;
using Mother4.Data.Enemies;

namespace Mother4.Battle.EnemyAI
{
	internal class FrooguAI : IEnemyAI
	{
		public FrooguAI(BattleController controller, Combatant sender, EnemyData data)
		{
			this.controller = controller;
			this.sender = sender;
			//this.battleActionParams = EnemyBattleActions.GetBattleActionParams((sender as EnemyCombatant).Enemy);
		}

		public BattleAction GetAction(int priority, Combatant[] potentialTargets)
		{

			ActionParams aparams = this.battleActionParams[Engine.Random.Next(this.battleActionParams.Count)];
			aparams.controller = this.controller;
			aparams.sender = this.sender;
			aparams.priority = this.sender.Stats.Speed;
			List<Combatant> targets = new List<Combatant>();

			if (aparams.actionType == typeof(EnemyBashAction))
			{

				//we do a slight amount of trolling
				if (Engine.Random.NextDouble() >= 0.5)
				{
					Console.WriteLine("Snago is hitting everyone!");
					//bash everyone
					foreach (Combatant pottar in potentialTargets)
					{
						targets.Add(pottar);
					}
				}
				else
				{
					//bash meryl
					Console.WriteLine("Snago is hitting Meryl!");
					foreach (Combatant combatant2 in potentialTargets)
					{
						if (combatant2.Faction == BattleFaction.PlayerTeam)
						{
							PlayerCombatant playerCombatant = combatant2 as PlayerCombatant;
							if (playerCombatant.Character == CharacterType.Meryl)
							{
								targets.Add(playerCombatant);
								break;
							}
						}
					}
				}
			}

			aparams.targets = targets.ToArray();

			return BattleAction.GetInstance(aparams);
		}

		private List<ActionParams> battleActionParams = new List<ActionParams>() {
						new ActionParams
						{
							actionType = typeof(EnemyTurnWasteAction),
							data = new object[]
							{
								"The Snagtagious Froog is... just standing there?",
								true
							}
						},
						new ActionParams
						{
							actionType = typeof(EnemyBashAction),
							data = new object[]
							{
								5f
							}
						}

		};

		private BattleController controller;

		private Combatant sender;
	}
}
