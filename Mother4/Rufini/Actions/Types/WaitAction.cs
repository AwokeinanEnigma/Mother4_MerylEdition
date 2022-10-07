using System;
using System.Collections.Generic;
using Carbine.Utility;
using Mother4.Scripts;
using Mother4.Scripts.Actions;

namespace Rufini.Actions.Types
{
	internal class WaitAction : RufiniAction
	{
		public WaitAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "wait",
					Type = typeof(int)
				}
			};
		}

		public override ActionReturnContext Execute(ExecutionContext context)
		{
			this.context = context;
			int value = base.GetValue<int>("wait");
			this.timerIndex = TimerManager.Instance.StartTimer(value);
			TimerManager.Instance.OnTimerEnd += this.TimerEnd;
			return new ActionReturnContext
			{
				Wait = ScriptExecutor.WaitType.Event
			};
		}

		private void TimerEnd(int timerIndex)
		{
			if (this.timerIndex == timerIndex)
			{
				TimerManager.Instance.OnTimerEnd -= this.TimerEnd;
				this.context.Executor.Continue();
			}
		}

		private ExecutionContext context;

		private int timerIndex;
	}
}
