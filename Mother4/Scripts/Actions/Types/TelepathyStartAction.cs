using System;
using System.Collections.Generic;
using Carbine.Flags;
using Mother4.Actors;

namespace Mother4.Scripts.Actions.Types
{
	internal class TelepathyStartAction : RufiniAction
	{
		public TelepathyStartAction()
		{
			this.paramList = new List<ActionParam>();
		}

		public override ActionReturnContext Execute(ExecutionContext context)
		{
			Console.WriteLine("It's telepathy time!");
			FlagManager.Instance[4] = true;
			this.context = context;
			if (this.context.Player != null)
			{
				this.context.Player.Telepathize();
				this.context.Player.OnTelepathyAnimationComplete += this.player_OnTelepathyAnimationComplete;
			}
			return new ActionReturnContext
			{
				Wait = ScriptExecutor.WaitType.Event
			};
		}

		private void player_OnTelepathyAnimationComplete(Player player)
		{
			this.context.Player.OnTelepathyAnimationComplete -= this.player_OnTelepathyAnimationComplete;
			if (this.context.CheckedNPC != null)
			{
				this.context.CheckedNPC.Telepathize();
				this.context.TextBox.SetDimmer(0.5f);
			}
			this.context.Executor.Continue();
			this.context = null;
		}

		private ExecutionContext context;
	}
}
