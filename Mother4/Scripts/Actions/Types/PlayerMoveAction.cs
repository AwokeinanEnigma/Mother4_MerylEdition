using System;
using System.Collections.Generic;
using Mother4.Actors;
using Mother4.Actors.NPCs.Movement;
using SFML.System;

namespace Mother4.Scripts.Actions.Types
{
	internal class PlayerMoveAction : RufiniAction
	{
		public PlayerMoveAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "x",
					Type = typeof(int)
				},
				new ActionParam
				{
					Name = "y",
					Type = typeof(int)
				},
				new ActionParam
				{
					Name = "rel",
					Type = typeof(bool)
				},
				new ActionParam
				{
					Name = "spd",
					Type = typeof(int)
				},
				new ActionParam
				{
					Name = "sub",
					Type = typeof(string)
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
			base.GetValue<string>("name");
			int value = base.GetValue<int>("x");
			int value2 = base.GetValue<int>("y");
			bool value3 = base.GetValue<bool>("rel");
			int value4 = base.GetValue<int>("spd");
			string value5 = base.GetValue<string>("sub");
			this.blocking = base.GetValue<bool>("blk");
			Player player = context.Player;
			if (player != null)
			{
				Vector2f vector2f = new Vector2f((float)value, (float)value2);
				if (value3)
				{
					vector2f += player.Position;
				}
				if (value4 > 0)
				{
					PointMover pointMover = new PointMover(vector2f, (float)value4);
					pointMover.OnMoveComplete += this.OnMoveComplete;
					player.SetMover(pointMover);
					this.context = context;
					if (this.blocking)
					{
						result.Wait = ScriptExecutor.WaitType.Event;
					}
				}
				else
				{
					player.Position = vector2f;
				}
				if (value5.Length > 0)
				{
					player.OverrideSubsprite(value5);
				}
				else
				{
					player.ClearOverrideSubsprite();
				}
			}
			else
			{
				Console.WriteLine("No player object in this scene");
			}
			return result;
		}

		private void OnMoveComplete(PointMover sender)
		{
			sender.OnMoveComplete -= this.OnMoveComplete;
			this.context.Player.ClearMover();
			if (this.blocking)
			{
				this.context.Executor.Continue();
			}
		}

		private ExecutionContext context;

		private bool blocking;
	}
}
