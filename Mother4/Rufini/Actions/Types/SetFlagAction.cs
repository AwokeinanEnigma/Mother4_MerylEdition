using System;
using System.Collections.Generic;
using Carbine.Flags;
using Mother4.Scripts;
using Mother4.Scripts.Actions;

namespace Rufini.Actions.Types
{
	internal class SetFlagAction : RufiniAction
	{
		public SetFlagAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "flg",
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
			int value = base.GetValue<int>("flg");
			bool value2 = base.GetValue<bool>("val");
			FlagManager.Instance[value] = value2;
			return default(ActionReturnContext);
		}
	}
}
