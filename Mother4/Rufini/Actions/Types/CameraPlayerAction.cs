using System;
using System.Collections.Generic;
using Carbine.Graphics;
using Mother4.Scripts;
using Mother4.Scripts.Actions;

namespace Rufini.Actions.Types
{
	internal class CameraPlayerAction : RufiniAction
	{
		public CameraPlayerAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "spd",
					Type = typeof(float)
				},
				new ActionParam
				{
					Name = "blk",
					Type = typeof(bool)
				}
			};
		}

		public override ActionReturnContext Execute(ExecutionContext context)
		{
			ActionReturnContext result = default(ActionReturnContext);
			float value = base.GetValue<float>("spd");
			bool value2 = base.GetValue<bool>("blk");
			ViewManager.Instance.MoveTo(context.Player, value);
			if (value2)
			{
				this.context = context;
				ViewManager.Instance.OnMoveToComplete += this.OnMoveToComplete;
				result.Wait = ScriptExecutor.WaitType.Event;
			}
			return result;
		}

		private void OnMoveToComplete(ViewManager sender)
		{
			ViewManager.Instance.OnMoveToComplete -= this.OnMoveToComplete;
			this.context.Executor.Continue();
		}

		private ExecutionContext context;
	}
}
