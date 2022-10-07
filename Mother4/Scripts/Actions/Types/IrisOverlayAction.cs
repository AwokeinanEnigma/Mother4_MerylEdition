using System;
using System.Collections.Generic;
using Carbine.Scenes;
using Mother4.GUI;
using Mother4.Scenes;

namespace Mother4.Scripts.Actions.Types
{
	internal class IrisOverlayAction : RufiniAction
	{
		public IrisOverlayAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "prg",
					Type = typeof(float)
				},
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
			float value = base.GetValue<float>("prg");
			float value2 = base.GetValue<float>("spd");
			this.isBlocking = base.GetValue<bool>("blk");
			Scene scene = SceneManager.Instance.Peek();
			if (scene is OverworldScene)
			{
				OverworldScene overworldScene = (OverworldScene)scene;
				IrisOverlay irisOverlay = overworldScene.IrisOverlay;
				if (value2 > 0f)
				{
					irisOverlay.Speed = value2;
					irisOverlay.OnAnimationComplete += this.OnAnimationComplete;
				}
				else
				{
					this.isBlocking = false;
				}
				irisOverlay.Progress = value;
				irisOverlay.Visible = true;
			}
			this.context = context;
			return new ActionReturnContext
			{
				Wait = (this.isBlocking ? ScriptExecutor.WaitType.Event : ScriptExecutor.WaitType.None)
			};
		}

		private void OnAnimationComplete(IrisOverlay sender)
		{
			sender.OnAnimationComplete -= this.OnAnimationComplete;
			if (this.isBlocking)
			{
				this.context.Executor.Continue();
			}
		}

		private ExecutionContext context;

		private bool isBlocking;
	}
}
