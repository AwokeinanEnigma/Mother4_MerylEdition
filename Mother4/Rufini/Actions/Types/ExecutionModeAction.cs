using System;
using System.Collections.Generic;
using Mother4.Scripts;
using Mother4.Scripts.Actions;
using Mother4.Scripts.Actions.ParamTypes;

namespace Rufini.Actions.Types
{
	internal class ExecutionModeAction : RufiniAction
	{
		public ExecutionModeAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "mode",
					Type = typeof(RufiniOption)
				}
			};
		}

		public override ActionReturnContext Execute(ExecutionContext context)
		{
			Console.WriteLine("NOT IMPLEMENTED - BUG DAVE");
			return default(ActionReturnContext);
		}
	}
}
