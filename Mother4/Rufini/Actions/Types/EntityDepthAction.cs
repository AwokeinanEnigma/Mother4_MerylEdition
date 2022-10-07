using System;
using System.Collections.Generic;
using Carbine.Actors;
using Mother4.Actors.NPCs;
using Mother4.Scripts;
using Mother4.Scripts.Actions;

namespace Rufini.Actions.Types
{
	internal class EntityDepthAction : RufiniAction
	{
		public EntityDepthAction()
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
					Name = "dpt",
					Type = typeof(int)
				},
				new ActionParam
				{
					Name = "rel",
					Type = typeof(bool)
				},
				new ActionParam
				{
					Name = "rst",
					Type = typeof(bool)
				}
			};
		}

		public override ActionReturnContext Execute(ExecutionContext context)
		{
			ActionReturnContext result = default(ActionReturnContext);
			string name = base.GetValue<string>("name");
			int value = base.GetValue<int>("dpt");
			bool value2 = base.GetValue<bool>("rel");
			bool value3 = base.GetValue<bool>("rst");
			NPC npc = (NPC)context.ActorManager.Find((Actor n) => n is NPC && ((NPC)n).Name == name);
			if (npc != null)
			{
				if (!value3)
				{
					int newDepth = value2 ? (npc.Depth + value) : value;
					npc.ForceDepth(newDepth);
				}
				else
				{
					npc.ResetDepth();
				}
			}
			return result;
		}
	}
}
