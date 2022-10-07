using System;
using Carbine;
using Carbine.Graphics;
using Carbine.Utility;
using SFML.System;

namespace Mother4.Actors.NPCs.Movement
{
	internal class LookForTroubleMover : Mover
	{
		public LookForTroubleMover(float chaseTreshold, float speed)
		{
			this.chaseThreshold = chaseTreshold;
			this.speed = speed;
			this.resetTimer();
			this.oldDirection = -1;
		}

		private void resetTimer()
		{
			this.timerEnd = Engine.Frame + 10L + (long)Engine.Random.Next(20);
		}

		public override bool GetNextMove(ref Vector2f position, ref Vector2f velocity, ref int direction)
		{
			this.oldDirection = direction;
			this.changed = false;
			if (ViewManager.Instance.FollowActor != null)
			{
                float num = VectorMath.Magnitude(ViewManager.Instance.FollowActor.Position - position);
				this.mode = ((num < this.chaseThreshold) ? LookForTroubleMover.Mode.Chase : LookForTroubleMover.Mode.Wait);
			}
			if (this.mode == LookForTroubleMover.Mode.Wait)
			{
				if (Engine.Frame > this.timerEnd)
				{
					direction = ((Engine.Random.Next(100) < 50) ? 0 : 4);
					this.resetTimer();
				}
				velocity = VectorMath.ZERO_VECTOR;
				this.changed = true;
			}
			else if (this.mode == LookForTroubleMover.Mode.Chase && ViewManager.Instance.FollowActor != null)
			{
				direction = VectorMath.VectorToDirection((ViewManager.Instance.FollowActor.Position - position) * 2);
				velocity = VectorMath.DirectionToVector(direction) * this.speed;
				this.changed = true;
			}
			return this.changed;
		}

		private const int TIMER_MAX = 30;

		private const int TIMER_MIN = 10;

        private float chaseThreshold;

		private float speed;

		private long timerEnd;

		private LookForTroubleMover.Mode mode;

		private int oldDirection;

		private bool changed;

		private enum Mode
		{
			Wait,
			Chase
		}
	}
}
