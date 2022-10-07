using System;
using System.Collections.Generic;
using Carbine.Scenes;
using Carbine.Utility;
using Mother4.Scenes;
using Mother4.Scripts;
using Mother4.Scripts.Actions;
using Mother4.Utility;
using SFML.Graphics;

namespace Rufini.Actions.Types
{
	internal class ScreenFlashAction : RufiniAction
	{
		public ScreenFlashAction()
		{
			this.timerId = -1;
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "col",
					Type = typeof(Color)
				},
				new ActionParam
				{
					Name = "tdur",
					Type = typeof(int)
				},
				new ActionParam
				{
					Name = "hdur",
					Type = typeof(int)
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
			Color value = base.GetValue<Color>("col");
			this.transDuration = base.GetValue<int>("tdur");
			this.holdDuration = base.GetValue<int>("hdur");
			this.blocking = base.GetValue<bool>("blk");
			Scene scene = SceneManager.Instance.Peek();
			if (scene is OverworldScene)
			{
				this.dimmer = ((OverworldScene)scene).Dimmer;
				this.originalFadeColor = this.dimmer.TargetColor;
				this.dimmer.ChangeColor(value, this.transDuration);
				if (this.blocking)
				{
					this.dimmer.OnFadeComplete += this.OnFadeComplete;
					result = new ActionReturnContext
					{
						Wait = ScriptExecutor.WaitType.Event
					};
					this.context = context;
				}
			}
			return result;
		}

		private void OnFadeComplete(ScreenDimmer sender)
		{
			if (this.timerId == -1)
			{
				this.timerId = TimerManager.Instance.StartTimer(this.holdDuration);
				TimerManager.Instance.OnTimerEnd += this.OnTimerEnd;
				return;
			}
			this.dimmer.OnFadeComplete -= this.OnFadeComplete;
			TimerManager.Instance.OnTimerEnd -= this.OnTimerEnd;
			this.context.Executor.Continue();
		}

		private void OnTimerEnd(int timerIndex)
		{
			if (this.timerId == timerIndex)
			{
				this.dimmer.ChangeColor(this.originalFadeColor, this.transDuration);
			}
		}

		private ExecutionContext context;

		private Color originalFadeColor;

		private int transDuration;

		private int holdDuration;

		private bool blocking;

		private int timerId;

		private ScreenDimmer dimmer;
	}
}
