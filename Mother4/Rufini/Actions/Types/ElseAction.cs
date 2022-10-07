using System;
using System.Collections.Generic;
using Mother4.Scripts;
using Mother4.Scripts.Actions;

namespace Rufini.Actions.Types
{
	internal class ElseAction : RufiniAction
	{
		public ElseAction()
		{
			this.paramList = new List<ActionParam>();
		}

		public override ActionReturnContext Execute(ExecutionContext context)
		{
			context.Executor.JumpToElseOrEndIf();
			return default(ActionReturnContext);
		}
	}
}
