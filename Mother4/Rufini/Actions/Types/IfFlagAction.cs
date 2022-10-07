using System;
using System.Collections.Generic;
using Carbine.Flags;
using Mother4.Scripts;
using Mother4.Scripts.Actions;

namespace Rufini.Actions.Types
{
	internal class IfFlagAction : RufiniAction
	{
		public IfFlagAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "id",
					Type = typeof(int)
				},
				new ActionParam
				{
					Name = "val",
					Type = typeof(bool)
				}
			};
		}

		public override ActionReturnContext Execute(ExecutionContext context)
		{
			int value = base.GetValue<int>("id");
			bool value2 = base.GetValue<bool>("val");
			bool flag = FlagManager.Instance[value];
			if (value2 != flag)
			{
				context.Executor.JumpToElseOrEndIf();
			}
			return default(ActionReturnContext);
		}
	}
}
