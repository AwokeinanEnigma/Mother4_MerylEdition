using System;
using Mother4.Battle.Combatants;

namespace Mother4.Battle.Actions
{
	internal struct ActionParams
	{
		public Type actionType;

		public BattleController controller;

		public Combatant sender;

		public Combatant[] targets;

		public int priority;

		public object[] data;
	}
}
