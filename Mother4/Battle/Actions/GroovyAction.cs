using System;
using Mother4.Battle.Combatants;

namespace Mother4.Battle.Actions
{
	internal class GroovyAction : BattleAction
	{
		public GroovyAction(ActionParams aparams) : base(aparams)
		{
			this.combatant = (aparams.targets[0] as PlayerCombatant);
			this.state = GroovyAction.State.Start;
		}

		protected override void UpdateAction()
		{
			base.UpdateAction();
			switch (this.state)
			{
			case GroovyAction.State.Start:
				if (this.CanGroovy())
				{
					this.controller.InterfaceController.SetCardGroovy(this.combatant.ID, true);
					this.state = GroovyAction.State.WaitForPop;
					return;
				}
				this.state = GroovyAction.State.Cancel;
				this.complete = true;
				return;
			case GroovyAction.State.WaitForPop:
				this.TimerWait(30, GroovyAction.State.Groovy);
				return;
			case GroovyAction.State.Groovy:
				this.controller.InterfaceController.DoGroovy(this.combatant.ID);
				this.state = GroovyAction.State.WaitForGroovy;
				return;
			case GroovyAction.State.WaitForGroovy:
				this.TimerWait(80, GroovyAction.State.Finish);
				return;
			case GroovyAction.State.Finish:
			{
				this.combatant.AlterStats(new StatSet
				{
					Meter = -this.combatant.Stats.Meter
				});
				ActionParams aparams = new ActionParams
				{
					actionType = typeof(PlayerDecisionAction),
					controller = this.controller,
					sender = this.combatant,
					priority = 2147483646,
					data = new object[]
					{
						true
					}
				};
				this.controller.AddDecisionAction(new PlayerDecisionAction(aparams));
				this.complete = true;
				return;
			}
			default:
				return;
			}
		}

		private void TimerWait(int delay, GroovyAction.State nextState)
		{
			this.timer++;
			if (this.timer > delay)
			{
				this.timer = 0;
				this.state = nextState;
			}
		}

		private bool CanGroovy()
		{
			Combatant[] factionCombatants = this.controller.CombatantController.GetFactionCombatants(BattleFaction.EnemyTeam);
			return factionCombatants.Length > 0;
		}

		private const int POP_HEIGHT = 28;

		private const int POP_DELAY = 30;

		private const int GROOVY_DELAY = 80;

		private GroovyAction.State state;

		private PlayerCombatant combatant;

		private int timer;

		private enum State
		{
			Start,
			WaitForPop,
			Groovy,
			WaitForGroovy,
			Finish,
			Cancel
		}
	}
}
