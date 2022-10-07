using System;
using System.Collections.Generic;
using Carbine;
using Carbine.Flags;

namespace Mother4.Scripts.Actions.Types
{
	internal class ValueRandomAction : RufiniAction
	{
		public ValueRandomAction()
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
					Name = "min",
					Type = typeof(int)
				},
				new ActionParam
				{
					Name = "max",
					Type = typeof(int)
				}
			};
		}

		public override ActionReturnContext Execute(ExecutionContext context)
		{
			int value = base.GetValue<int>("id");
			int num = base.GetValue<int>("min");
			int num2 = base.GetValue<int>("max");
			if (num2 < num)
			{
				num = num2;
				num2 = num;
			}
			int maxValue = num2 - num;
			int value2 = num + Engine.Random.Next(maxValue);
			ValueManager.Instance[value] = value2;
			return default(ActionReturnContext);
		}
	}
}
