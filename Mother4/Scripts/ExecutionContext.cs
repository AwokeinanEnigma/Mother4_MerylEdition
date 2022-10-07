using System;
using System.Collections.Generic;
using Carbine.Actors;
using Carbine.Collision;
using Carbine.Graphics;
using Carbine.Maps;
using Mother4.Actors;
using Mother4.Actors.NPCs;
using Mother4.GUI;

namespace Mother4.Scripts
{
	internal class ExecutionContext
	{
		public ScriptExecutor Executor { get; set; }

		public RenderPipeline Pipeline { get; set; }

		public ActorManager ActorManager { get; set; }

		public CollisionManager CollisionManager { get; set; }

		public TextBox TextBox { get; set; }

		public QuestionBox QuestionBox { get; set; }

		public Player Player { get; set; }

		public NPC ActiveNPC { get; set; }

		public NPC CheckedNPC { get; set; }

		public string Nametag { get; set; }

		public ICollection<Map.Path> Paths { get; set; }

        public ICollection<Map.Area> Areas { get; set; }

        public ExecutionContext()
		{
		}

		public ExecutionContext(ExecutionContext source)
		{
			this.Executor = source.Executor;
			this.Pipeline = source.Pipeline;
			this.ActorManager = source.ActorManager;
			this.CollisionManager = source.CollisionManager;
			this.TextBox = source.TextBox;
			this.QuestionBox = source.QuestionBox;
			this.Player = source.Player;
			this.ActiveNPC = source.ActiveNPC;
			this.CheckedNPC = source.CheckedNPC;
			this.Nametag = source.Nametag;
			this.Paths = source.Paths;
			this.Areas = source.Areas;
		}

		public NPC GetNpcByName(string npcName)
		{
			return (NPC)this.ActorManager.Find((Actor n) => n is NPC && ((NPC)n).Name == npcName);
		}
	}
}
