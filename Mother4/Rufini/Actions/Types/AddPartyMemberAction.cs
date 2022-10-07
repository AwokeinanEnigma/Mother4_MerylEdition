using System;
using System.Collections.Generic;
using Carbine.Actors;
using Carbine.Scenes;
using Mother4.Actors;
using Mother4.Actors.NPCs;
using Mother4.Data;
using Mother4.Overworld;
using Mother4.Scenes;
using Mother4.Scripts;
using Mother4.Scripts.Actions;
using Mother4.Scripts.Actions.ParamTypes;
using Rufini.Strings;
using SFML.System;

namespace Rufini.Actions.Types
{
	internal class AddPartyMemberAction : RufiniAction
	{
		public AddPartyMemberAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "char",
					Type = typeof(RufiniOption)
				},
				new ActionParam
				{
					Name = "name",
					Type = typeof(string)
				},
				new ActionParam
				{
					Name = "sup",
					Type = typeof(bool)
				}
			};
		}

		public override ActionReturnContext Execute(ExecutionContext context)
		{
			ActionReturnContext result = default(ActionReturnContext);
			CharacterType option = (CharacterType)base.GetValue<RufiniOption>("char").Option;
			string npcName = base.GetValue<string>("name");
			bool value = base.GetValue<bool>("sup");
			PartyManager.Instance.Add(option);
			Scene scene = SceneManager.Instance.Peek();
			if (scene is OverworldScene)
			{
				NPC npc = (NPC)context.ActorManager.Find((Actor x) => x is NPC && ((NPC)x).Name == npcName);
				Vector2f position;
				int direction;
				if (npc != null)
				{
					position = npc.Position;
					direction = npc.Direction;
					context.ActorManager.Remove(npc);
					context.CollisionManager.Remove(npc, false);
				}
				else
				{
					position = context.Player.Position;
					direction = context.Player.Direction;
				}
                PartyTrain partyTrain = ((OverworldScene)scene).PartyTrain;
                PartyFollower partyFollower = new PartyFollower(context.Pipeline, context.CollisionManager, partyTrain, option, position, direction, true);
                partyTrain.Add(partyFollower);
			}
			if (!value)
			{
				this.context = context;
				string text = StringFile.Instance.Get("system.partyJoin").Value;
				text = text.Replace("$newMember", CharacterNames.GetName(option));
				this.context.TextBox.OnTextboxComplete += this.ContinueAfterTextbox;
				this.context.TextBox.Reset(text, string.Empty, false, false);
				this.context.TextBox.Show();
				this.context.Player.MovementLocked = true;
				result = new ActionReturnContext
				{
					Wait = ScriptExecutor.WaitType.Event
				};
			}
			return result;
		}

		private void ContinueAfterTextbox()
		{
			this.context.TextBox.Hide();
			this.context.TextBox.OnTextboxComplete -= this.ContinueAfterTextbox;
			this.context.Player.MovementLocked = false;
			this.context.Executor.Continue();
		}

		private const string MSG_KEY = "system.partyJoin";

		private ExecutionContext context;
	}
}
