using System;
using System.Collections.Generic;
using Carbine.Actors;
using Mother4.Actors.NPCs;
using Mother4.Scripts;
using Mother4.Scripts.Actions;

namespace Rufini.Actions.Types
{
	internal class SetNpcAction : RufiniAction
	{
		public SetNpcAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "name",
					Type = typeof(string)
				},
				new ActionParam
				{
					Name = "talk",
					Type = typeof(bool)
				}
			};
		}

		public override ActionReturnContext Execute(ExecutionContext context)
		{
			string name = base.GetValue<string>("name");
			base.GetValue<bool>("talk");
			context.ActiveNPC = null;
			if (name != null && name.Length > 0)
			{
				NPC npc = (NPC)context.ActorManager.Find((Actor n) => n is NPC && ((NPC)n).Name == name);
				if (npc != null)
				{
					context.ActiveNPC = npc;
				}
			}
			return default(ActionReturnContext);
		}
	}
}
