using System;
using Mother4.Battle.Combatants;
using Mother4.Data;
using Mother4.Data.Enemies;
using Mother4.Utility;

namespace Mother4.Battle.Actions
{
	internal class EnemyDeathAction : BattleAction
	{
		public EnemyDeathAction(ActionParams aparams) : base(aparams)
		{
			this.combatant = (this.sender as EnemyCombatant);
			this.state = EnemyDeathAction.State.Initialize;
		}

		protected override void UpdateAction()
		{
			base.UpdateAction();
			switch (this.state)
			{
			case EnemyDeathAction.State.Initialize:
				this.controller.InterfaceController.DoEnemyDeathAnimation(this.combatant.ID);
				this.state = EnemyDeathAction.State.WaitForAnimation;
				return;
			case EnemyDeathAction.State.WaitForUI:
				break;
			case EnemyDeathAction.State.WaitForAnimation:
				this.timer++;
				if (this.timer > 40)
				{
					this.timer = 0;
					this.state = EnemyDeathAction.State.Removal;
					return;
				}
				break;
			case EnemyDeathAction.State.Removal:
			{
				Console.WriteLine("Enemy got dead.");
				this.controller.RemoveCombatant(this.combatant);
				this.controller.InterfaceController.OnTextboxComplete += this.TextboxComplete;
                EnemyData enemy = this.combatant.Enemy;

                if (combatant.HasStatusEffect(StatusEffect.Talking))
                {
                    controller.InterfaceController.RemoveTalkers();
                }

                string message = string.Format("{0}{1} {2}", Capitalizer.Capitalize(enemy.Article), enemy.PlayerFriendlyName, enemy.GetStringQualifiedName("defeat"));
				this.controller.InterfaceController.ShowMessage(message, false);
				this.state = EnemyDeathAction.State.WaitForUI;
				return;
			}
			case EnemyDeathAction.State.WaitForMovement:
				this.timer++;
				if (this.timer > 10)
				{
					this.timer = 0;
					this.state = EnemyDeathAction.State.Finish;
					return;
				}
				break;
			case EnemyDeathAction.State.Finish:
				this.controller.InterfaceController.OnTextboxComplete -= this.TextboxComplete;
				this.complete = true;
				break;
			default:
				return;
			}
		}

		public void TextboxComplete()
		{
			this.state = EnemyDeathAction.State.WaitForMovement;
		}

		private const int ANIMATION_WAIT_FRAMES = 40;

		private const int MOVEMENT_WAIT_FRAMES = 10;

		private EnemyDeathAction.State state;

		private EnemyCombatant combatant;

		private int timer;

		private enum State
		{
			Initialize,
			WaitForUI,
			WaitForAnimation,
			Removal,
			WaitForMovement,
			Finish
		}
	}
}
