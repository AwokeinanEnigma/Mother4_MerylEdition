using System;
using System.Collections.Generic;
using Mother4.Scripts;
using Mother4.Scripts.Actions;

namespace Rufini.Actions.Types
{
	internal class HopPlayerAction : RufiniAction
	{
		public HopPlayerAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "h",
					Type = typeof(int)
				},
				new ActionParam
				{
					Name = "col",
					Type = typeof(bool)
				}
			};
		}

		public override ActionReturnContext Execute(ExecutionContext context)
		{
			base.GetValue<string>("name");
			int value = base.GetValue<int>("h");
			base.GetValue<bool>("col");
			context.Player.HopFactor = (float)value;
			return default(ActionReturnContext);
		}
	}
}
