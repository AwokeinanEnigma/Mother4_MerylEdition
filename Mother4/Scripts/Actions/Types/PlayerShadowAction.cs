using System;
using System.Collections.Generic;

namespace Mother4.Scripts.Actions.Types
{
	internal class PlayerShadowAction : RufiniAction
	{
		public PlayerShadowAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "shw",
					Type = typeof(bool)
				}
			};
		}

		public override ActionReturnContext Execute(ExecutionContext context)
		{
			bool value = base.GetValue<bool>("shw");
			context.Player.SetShadow(value);
			return default(ActionReturnContext);
		}
	}
}
