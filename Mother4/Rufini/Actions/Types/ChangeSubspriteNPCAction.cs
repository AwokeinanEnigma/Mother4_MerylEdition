using System;
using System.Collections.Generic;
using Carbine.Actors;
using Mother4.Actors.NPCs;
using Mother4.Scripts;
using Mother4.Scripts.Actions;

namespace Rufini.Actions.Types
{
	internal class ChangeSubspriteNPCAction : RufiniAction
	{
		public ChangeSubspriteNPCAction()
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
					Name = "sub",
					Type = typeof(string)
				}
			};
		}

		public override ActionReturnContext Execute(ExecutionContext context)
		{
			string name = base.GetValue<string>("name");
			string value = base.GetValue<string>("sub");
			NPC npc = (NPC)context.ActorManager.Find((Actor x) => x is NPC && ((NPC)x).Name == name);
			if (npc != null)
			{
				if (value.Length > 0)
				{
					npc.OverrideSubsprite(value);
				}
				else
				{
					npc.ClearOverrideSubsprite();
				}
			}
			return default(ActionReturnContext);
		}
	}
}
