using System;
using System.Collections.Generic;
using System.Linq;
using Carbine.Maps;
using Mother4.Actors.NPCs.Movement;

namespace Mother4.Scripts.Actions.Types
{
	internal class PlayerPathMoveAction : RufiniAction
	{
		public PlayerPathMoveAction()
		{
			this.paramList = new List<ActionParam>
			{
				new ActionParam
				{
					Name = "cnstr",
					Type = typeof(string)
				},
				new ActionParam
				{
					Name = "spd",
					Type = typeof(int)
				},
				new ActionParam
				{
					Name = "snp",
					Type = typeof(bool)
				},
				new ActionParam
				{
					Name = "ext",
					Type = typeof(bool)
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
			this.context = context;
			string pathName = base.GetValue<string>("cnstr");
			int value = base.GetValue<int>("spd");
			bool value2 = base.GetValue<bool>("snp");
			this.blocking = base.GetValue<bool>("blk");
			bool value3 = base.GetValue<bool>("ext");
			Map.Path path = this.context.Paths.FirstOrDefault((Map.Path x) => x.Name == pathName);
			ActionReturnContext result;
			if (path.Points != null && path.Points.Count > 0)
			{
				if (value2)
				{
					this.context.Player.SetPosition(path.Points[0], value3);
				}
				this.mover = new PathMover((float)value, 0, false, path.Points);
				this.mover.OnPathComplete += this.PathComplete;
				this.context.Player.SetMover(this.mover);
				result = new ActionReturnContext
				{
					Wait = (this.blocking ? ScriptExecutor.WaitType.Event : ScriptExecutor.WaitType.Frame)
				};
			}
			else
			{
				Console.WriteLine("No path with name \"{0}\" exists.", pathName);
				result = default(ActionReturnContext);
			}
			return result;
		}

		private void PathComplete()
		{
			this.mover.OnPathComplete -= this.PathComplete;
			this.context.Player.ClearMover();
			if (this.blocking)
			{
				this.context.Executor.Continue();
			}
		}

		private ExecutionContext context;

		private PathMover mover;

		private bool blocking;
	}
}
