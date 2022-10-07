using System;
using Mother4.Battle.Actions;
using Mother4.Battle.Combatants;

namespace Mother4.Battle.EnemyAI
{
	internal interface IEnemyAI
	{
		BattleAction GetAction(int priority, Combatant[] potentialTargets);
	}
}
