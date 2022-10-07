using System;

namespace Mother4.Battle.Actions
{
	internal class MessageAction : BattleAction
	{
		public MessageAction(ActionParams aparams) : base(aparams)
		{
			this.message = (string)aparams.data[0];
			this.useButton = (bool)aparams.data[1];
			this.state = MessageAction.State.Initialize;
		}

		protected override void UpdateAction()
		{
			base.UpdateAction();
			switch (this.state)
			{
			case MessageAction.State.Initialize:
				this.controller.InterfaceController.OnTextboxComplete += this.InteractionComplete;
				this.controller.InterfaceController.ShowMessage(this.message, this.useButton);
				this.state = MessageAction.State.WaitForUI;
				return;
			case MessageAction.State.WaitForUI:
				break;
			case MessageAction.State.Finish:
				this.controller.InterfaceController.OnTextboxComplete -= this.InteractionComplete;
				this.complete = true;
				break;
			default:
				return;
			}
		}

		public void InteractionComplete()
		{
			this.state = MessageAction.State.Finish;
		}

		private const int MESSAGE_INDEX = 0;

		private const int USE_BUTTON_INDEX = 1;

		private MessageAction.State state;

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
