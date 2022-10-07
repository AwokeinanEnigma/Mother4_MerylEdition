using System;
using System.Collections.Generic;
using Carbine.Flags;
using Mother4.Scripts;
using Mother4.Scripts.Actions;
using Mother4.Scripts.Actions.ParamTypes;

namespace Rufini.Actions.Types
{
	internal class IfValueAction : RufiniAction
	{
		public IfValueAction()
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
					Name = "op",
					Type = typeof(RufiniOption)
				},
				new ActionParam
				{
					Name = "val",
					Type = typeof(int)
				}
			};
		}

		public override ActionReturnContext Execute(ExecutionContext context)
		{
			int value = base.GetValue<int>("id");
			int value2 = base.GetValue<int>("op");
			int value3 = base.GetValue<int>("val");
			int num = ValueManager.Instance[value];
			bool[] array = new bool[]
			{
				value3 == num,
				value3 != num,
				value3 <= num,
				value3 >= num,
				value3 < num,
				value3 > num
			};
			if (!array[value2])
			{
				context.Executor.JumpToElseOrEndIf();
			}
			return default(ActionReturnContext);
		}
	}
}
