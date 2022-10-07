using System;
using Carbine.Utility;
using SFML.System;

namespace Mother4.Actors.NPCs.Movement
{
	internal class PointMover : Mover
	{
		public event PointMover.OnMoveCompleteHandler OnMoveComplete;

		public PointMover(Vector2f position, float speed)
		{
			this.target = position;
			this.speed = speed;
		}

		public override bool GetNextMove(ref Vector2f position, ref Vector2f velocity, ref int direction)
		{
			bool result = false;
			if (!this.done)
			{
				if (Math.Abs(this.target.X - position.X) > this.speed || Math.Abs(this.target.Y - position.Y) > this.speed)
				{
					base.MovementStep(this.speed, ref position, ref this.target, ref velocity);
					direction = VectorMath.VectorToDirection(velocity);
					result = true;
				}
				else
				{
					position = this.target;
					velocity = VectorMath.ZERO_VECTOR;
					this.done = true;
					result = true;
					if (this.OnMoveComplete != null)
					{
						this.OnMoveComplete(this);
					}
				}
			}
			return result;
		}

		private Vector2f target;

		private float speed;

		private bool done;

		public delegate void OnMoveCompleteHandler(PointMover sender);
	}
}
