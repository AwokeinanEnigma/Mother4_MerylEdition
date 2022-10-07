using System;
using Carbine;
using Carbine.Collision;
using Carbine.Graphics;
using Carbine.Utility;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Actors.NPCs.Movement
{
	internal class ZigZagMover : Mover
	{
		public ZigZagMover(CollisionManager collisionManager, ICollidable collidable, FloatRect area, float amplitude, float speed, float chaseThreshold)
		{
			this.collisionManager = collisionManager;
			this.collidable = collidable;
			this.amplitude = amplitude;
			this.speed = speed;
			this.chaseThreshold = chaseThreshold;
			this.area = area;
		}

		private void resetTimer()
		{
			this.timerEnd = Engine.Frame + 180L;
		}

		public override bool GetNextMove(ref Vector2f position, ref Vector2f velocity, ref int direction)
		{
			this.changed = false;
			if (Engine.Frame > this.timerEnd)
			{
				this.mode = ZigZagMover.Mode.Move;
				float num = 2.1474836E+09f;
				if (ViewManager.Instance.FollowActor != null)
				{
					num = VectorMath.Magnitude(ViewManager.Instance.FollowActor.Position - position);
				}
				if (num > this.chaseThreshold)
				{
					do
					{
						int num2 = (int)this.area.Left + Engine.Random.Next((int)this.area.Width);
						int num3 = (int)this.area.Top + Engine.Random.Next((int)this.area.Height);
						this.toPosition = new Vector2f((float)num2, (float)num3);
						if (VectorMath.Magnitude(position - this.toPosition) <= 30f)
						{
							break;
						}
					}
					while (!this.collisionManager.PlaceFree(this.collidable, this.toPosition));
				}
				else
				{
					this.toPosition = ViewManager.Instance.FollowActor.Position;
				}
				this.fromPosition = position;
				this.directionVector = VectorMath.Normalize(this.toPosition - position);
				this.normalVector = VectorMath.Normalize(VectorMath.LeftNormal(this.directionVector));
				this.resetTimer();
				velocity = VectorMath.ZERO_VECTOR;
			}
			if (this.mode == ZigZagMover.Mode.Move)
			{
				if (VectorMath.Magnitude(position - this.toPosition) > 1f)
				{
					float num4 = VectorMath.Magnitude(this.toPosition - this.fromPosition);
					float num5 = VectorMath.Magnitude(position - this.fromPosition) / num4;
					float x = (float)Math.Sin((double)num5 * 3.141592653589793 * 3.0) * (1f - num5) * (this.amplitude / 4f);
					velocity = this.directionVector * this.speed + this.normalVector * x;
				}
				else
				{
					this.mode = ZigZagMover.Mode.Wait;
				}
				this.changed = true;
			}
			return this.changed;
		}

		private const int TIMER_LENGTH = 180;

		private bool changed;

		private ZigZagMover.Mode mode;

		private long timerEnd;

		private Vector2f fromPosition;

		private Vector2f toPosition;

		private Vector2f directionVector;

		private Vector2f normalVector;

		private CollisionManager collisionManager;

		private ICollidable collidable;

		private float amplitude;

		private float chaseThreshold;

		private float speed;

		private FloatRect area;

		private enum Mode
		{
			Wait,
			Move
		}
	}
}
