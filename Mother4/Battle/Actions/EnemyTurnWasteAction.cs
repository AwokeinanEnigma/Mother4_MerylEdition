using System;
using Mother4.Battle.Combatants;
using SFML.Graphics;

namespace Mother4.Battle.Actions
{
	internal class EnemyTurnWasteAction : BattleAction
	{
		public EnemyTurnWasteAction(ActionParams aparams) : base(aparams)
		{
			this.message = (string)aparams.data[0];
			this.useButton = (bool)aparams.data[1];
			this.state = EnemyTurnWasteAction.State.Initialize;
		}

		protected override void UpdateAction()
		{
			base.UpdateAction();
			switch (this.state)
			{
				case EnemyTurnWasteAction.State.Initialize:

					this.controller.InterfaceController.OnTextboxComplete += this.InteractionComplete;
					this.controller.InterfaceController.ShowMessage(this.message, this.useButton);
					this.controller.InterfaceController.FlashEnemy(this.sender as EnemyCombatant, Color.Black, 8, 2);
					this.controller.InterfaceController.PreEnemyAttack.Play();
					this.state = EnemyTurnWasteAction.State.WaitForUI;
					return;
				case EnemyTurnWasteAction.State.WaitForUI:
					break;
				case EnemyTurnWasteAction.State.Finish:
					this.controller.InterfaceController.OnTextboxComplete -= this.InteractionComplete;
					this.complete = true;
					break;
				default:
					return;
			}
		}

		public void InteractionComplete()
		{
			this.state = EnemyTurnWasteAction.State.Finish;
		}

		private const int MESSAGE_INDEX = 0;

		private const int USE_BUTTON_INDEX = 1;

		private const int BLINK_DURATION = 8;

		private const int BLINK_COUNT = 2;

		private EnemyTurnWasteAction.State state;

		private string message;

		private bool useButton;

		private enum State
		{
			Initialize,
			WaitForUI,
			Finish
		}
	}
}
