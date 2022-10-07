using System;
using System.Collections.Generic;
using Carbine.Audio;
using Mother4.Scripts;
using Mother4.Scripts.Actions;

namespace Rufini.Actions.Types
{
	internal class PlaySFXAction : RufiniAction
	{
		public PlaySFXAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "sfx",
					Type = typeof(string)
				},
				new ActionParam
				{
					Name = "loop",
					Type = typeof(int)
				},
				new ActionParam
				{
					Name = "bal",
					Type = typeof(float)
				}
			};
		}

		public override ActionReturnContext Execute(ExecutionContext context)
		{
			string value = base.GetValue<string>("sfx");
			bool value2 = base.GetValue<bool>("loop");
			base.GetValue<float>("bal");
			CarbineSound CarbineSound = AudioManager.Instance.Use(value, AudioType.Sound);
			CarbineSound.LoopCount = (value2 ? -1 : 0);
			CarbineSound.OnComplete += this.SoundComplete;
			CarbineSound.Play();
			return default(ActionReturnContext);
		}

		private void SoundComplete(CarbineSound sender)
		{
			AudioManager.Instance.Unuse(sender);
		}
	}
}
