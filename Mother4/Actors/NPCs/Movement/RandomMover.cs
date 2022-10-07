using System;
using Carbine;
using SFML.System;

namespace Mother4.Actors.NPCs.Movement
{
	internal class RandomMover : Mover
	{
		public RandomMover(float speed, float distance, int timeOut)
		{
			this.speed = speed;
			this.distance = distance;
			this.timeOut = timeOut;
			this.target = default(Vector2f);
		}

		private void Randomize(ref Vector2f position)
		{
			this.target.X = position.X + (float)Math.Sign(Engine.Random.Next(3) - 1) * this.distance;
			this.target.Y = position.Y + (float)Math.Sign(Engine.Random.Next(3) - 1) * this.distance;
		}

		public override bool GetNextMove(ref Vector2f position, ref Vector2f velocity, ref int direction)
		{
			this.changed = false;
			bool flag = Math.Abs(this.target.X - position.X) <= 1f && Math.Abs(this.target.Y - position.Y) <= 1f;
			bool flag2 = this.timer <= 0;
			bool flag3 = this.timer > this.timeOut;
			if ((flag && flag3) || flag2 || flag3)
			{
				this.Randomize(ref position);
				this.timer = 0;
			}
			else
			{
				base.MovementStep(1f, ref position, ref this.target, ref velocity);
				this.changed = true;
			}
			this.timer++;
			return this.changed;
		}

		private float distance;

		private Vector2f target;

		private int timer;

		private int timeOut;

		private float speed;

		private bool changed;
	}
}
