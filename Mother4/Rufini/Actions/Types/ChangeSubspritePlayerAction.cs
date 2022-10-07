using System;
using System.Collections.Generic;
using Mother4.Scripts;
using Mother4.Scripts.Actions;

namespace Rufini.Actions.Types
{
	internal class ChangeSubspritePlayerAction : RufiniAction
	{
		public ChangeSubspritePlayerAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "sub",
					Type = typeof(string)
				}
			};
		}

		public override ActionReturnContext Execute(ExecutionContext context)
		{
			base.GetValue<string>("name");
			string value = base.GetValue<string>("sub");
			if (value.Length > 0)
			{
				context.Player.OverrideSubsprite(value);
			}
			else
			{
				context.Player.ClearOverrideSubsprite();
			}
			return default(ActionReturnContext);
		}
	}
}
