using System;
using System.Collections.Generic;
using Carbine.Flags;

namespace Mother4.Scripts.Actions.Types
{
	internal class TelepathyEndAction : RufiniAction
	{
		public TelepathyEndAction()
		{
			this.paramList = new List<ActionParam>();
		}

		public override ActionReturnContext Execute(ExecutionContext context)
		{
			Console.WriteLine("Telepathy time is over!");
			FlagManager.Instance[4] = false;
			if (context.CheckedNPC != null)
			{
				context.CheckedNPC.Untelepathize();
				context.TextBox.SetDimmer(0f);
			}
			return default(ActionReturnContext);
		}
	}
}
