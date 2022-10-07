using System;
using Carbine;
using SFML.System;

namespace Mother4.Actors.NPCs.Movement
{
	internal class RandomTurnMover : Mover
	{
		public RandomTurnMover(int time)
		{
			this.time = time;
			this.timer = 0;
		}

		public override bool GetNextMove(ref Vector2f position, ref Vector2f velocity, ref int direction)
		{
			this.changed = false;
			if (this.timer >= this.time)
			{
				this.timer = 0;
				direction = Engine.Random.Next(8);
				this.changed = true;
			}
			this.timer++;
			return this.changed;
		}

		private int time;

		private int timer;

		private bool changed;
	}
}
