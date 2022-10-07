using System;
using System.Collections.Generic;
using Carbine.Graphics;
using Mother4.Scripts;
using Mother4.Scripts.Actions;
using SFML.System;

namespace Rufini.Actions.Types
{
	internal class ScreenShakeAction : RufiniAction
	{
		public ScreenShakeAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "pow",
					Type = typeof(int)
				},
				new ActionParam
				{
					Name = "dur",
					Type = typeof(int)
				},
				new ActionParam
				{
					Name = "x",
					Type = typeof(bool)
				},
				new ActionParam
				{
					Name = "y",
					Type = typeof(bool)
				}
			};
		}

		public override ActionReturnContext Execute(ExecutionContext context)
		{
			bool value = base.GetValue<bool>("x");
			bool value2 = base.GetValue<bool>("y");
			int value3 = base.GetValue<int>("dur");
			float num = (float)base.GetValue<int>("pow");
			Vector2f intensity = new Vector2f(value ? num : 0f, value2 ? num : 0f);
			ViewManager.Instance.Shake(intensity, (float)value3 / 60f);
			return default(ActionReturnContext);
		}
	}
}
