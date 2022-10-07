using System;
using System.Collections.Generic;
using Carbine.Audio;
using Mother4.Scripts;
using Mother4.Scripts.Actions;

namespace Rufini.Actions.Types
{
	internal class SetBGMAction : RufiniAction
	{
		public SetBGMAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "bgm",
					Type = typeof(string)
				},
				new ActionParam
				{
					Name = "loop",
					Type = typeof(bool)
				}
			};
		}

		public override ActionReturnContext Execute(ExecutionContext context)
		{
			string value = base.GetValue<string>("bgm");
			bool value2 = base.GetValue<bool>("loop");
			AudioManager.Instance.SetBGM(value);
			AudioManager.Instance.BGM.LoopCount = (value2 ? -1 : 0);
			AudioManager.Instance.BGM.Play();
			return default(ActionReturnContext);
		}
	}
}
