using System;
using System.Collections.Generic;
using Mother4.Scripts.Actions.ParamTypes;
using SFML.System;

namespace Mother4.Scripts.Actions.Types
{
	internal class PlayerPositionAction : RufiniAction
	{
		public PlayerPositionAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "x",
					Type = typeof(int)
				},
				new ActionParam
				{
					Name = "y",
					Type = typeof(int)
				},
				new ActionParam
				{
					Name = "dir",
					Type = typeof(RufiniOption)
				}
			};
		}

		public override ActionReturnContext Execute(ExecutionContext context)
		{
			ActionReturnContext result = default(ActionReturnContext);
			int value = base.GetValue<int>("x");
			int value2 = base.GetValue<int>("y");
			int option = base.GetValue<RufiniOption>("dir").Option;
			context.Player.SetPosition(new Vector2f((float)value, (float)value2));
			context.Player.SetDirection(option);
			return result;
		}
	}
}
