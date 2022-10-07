using System;
using Mother4.Battle.Combatants;
using Mother4.Data;
using SFML.Graphics;

namespace Mother4.Battle.Actions
{
	internal class DisablePSIAction : BattleAction
	{
		public DisablePSIAction(ActionParams aparams) : base(aparams)
		{
			Console.WriteLine($"Current Target: {targets[0] } ");
			this.state = DisablePSIAction.State.Initialize;
		}

		protected override void UpdateAction()
		{
			base.UpdateAction();
			switch (this.state)
			{
				case DisablePSIAction.State.Initialize:

					this.controller.InterfaceController.OnTextboxComplete += this.InteractionComplete;
					targets[0].AddStatusEffect(StatusEffect.DisablePSI, 3);
					
					EnemyCombatant cE = this.sender as EnemyCombatant;
					PlayerCombatant pC = targets[0] as PlayerCombatant;

					this.controller.InterfaceController.ShowMessage($"{  cE.Enemy.PlayerFriendlyName } disabled { CharacterNames.GetName(pC.Character) }'s PSI!", true);
					this.controller.InterfaceController.FlashEnemy(this.sender as EnemyCombatant, Color.Black, 8, 2);
					this.controller.InterfaceController.PreEnemyAttack.Play();
					this.state = DisablePSIAction.State.WaitForUI;
					return;
				case DisablePSIAction.State.WaitForUI:
					break;
				case DisablePSIAction.State.Finish:
					this.controller.InterfaceController.OnTextboxComplete -= this.InteractionComplete;
					this.complete = true;
					break;
				default:
					return;
			}
		}

		public void InteractionComplete()
		{
			this.state = DisablePSIAction.State.Finish;
		}

		private const int MESSAGE_INDEX = 0;

		private const int USE_BUTTON_INDEX = 1;

		private const int BLINK_DURATION = 8;

		private const int BLINK_COUNT = 2;

		private DisablePSIAction.State state;

		private enum State
		{
			Initialize,
			WaitForUI,
			Finish
		}
	}
}
