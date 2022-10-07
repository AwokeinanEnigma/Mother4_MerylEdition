using System;
using SFML.System;

namespace Mother4.Actors.NPCs.Movement
{
	internal abstract class Mover
	{
        protected void MovementStep(float stepSize, ref Vector2f position, ref Vector2f target, ref Vector2f velocity)
		{
			velocity.X = (float)Math.Sign(target.X - position.X) * Math.Min(stepSize, Math.Abs(target.X - position.X));
			velocity.Y = (float)Math.Sign(target.Y - position.Y) * Math.Min(stepSize, Math.Abs(target.Y - position.Y));
		}

		public abstract bool GetNextMove(ref Vector2f position, ref Vector2f velocity, ref int direction);

		protected const float TOLERANCE = 1f;
	}
}
