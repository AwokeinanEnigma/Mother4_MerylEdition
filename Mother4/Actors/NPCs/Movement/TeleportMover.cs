using System;
using Carbine;
using Carbine.Collision;
using Carbine.Graphics;
using Carbine.Utility;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Actors.NPCs.Movement
{
	internal class TeleportMover : Mover
	{
		public TeleportMover(CollisionManager collisionManager, ICollidable collidable, FloatRect area, float maxDistance, float chaseThreshold)
		{
			this.collisionManager = collisionManager;
			this.collidable = collidable;
			this.area = area;
			this.maxDistance = maxDistance;
			this.chaseThreshold = chaseThreshold;
			this.resetTimer();
		}

		private void resetTimer()
		{
			this.timerEnd = Engine.Frame + 33L;
		}

		public override bool GetNextMove(ref Vector2f position, ref Vector2f velocity, ref int direction)
		{
			this.changed = false;
			if (this.mode == TeleportMover.Mode.Wait)
			{
				if (Engine.Frame > this.timerEnd)
				{
					this.mode = TeleportMover.Mode.Teleport;
					this.resetTimer();
				}
				velocity = VectorMath.ZERO_VECTOR;
			}
			if (this.mode == TeleportMover.Mode.Teleport)
			{
				float num = 2.1474836E+09f;
				if (ViewManager.Instance.FollowActor != null)
				{
					num = VectorMath.Magnitude(ViewManager.Instance.FollowActor.Position - position);
				}
				if (num < this.chaseThreshold)
				{
					if (ViewManager.Instance.FollowActor is Player)
					{
						Player player = (Player)ViewManager.Instance.FollowActor;
						position = ViewManager.Instance.FollowActor.Position - VectorMath.DirectionToVector(player.Direction) * 4f;
					}
					else
					{
						position = ViewManager.Instance.FollowActor.Position;
					}
					direction = VectorMath.VectorToDirection(ViewManager.Instance.FollowActor.Position - position);
				}
				else
				{
					Vector2f vector2f;
					do
					{
						int num2 = (int)this.area.Left + Engine.Random.Next((int)this.area.Width);
						int num3 = (int)this.area.Top + Engine.Random.Next((int)this.area.Height);
						vector2f = new Vector2f((float)num2, (float)num3);
					}
					while (!this.collisionManager.PlaceFree(this.collidable, vector2f));
					position = vector2f;
					direction = 6;
				}
				velocity = VectorMath.ZERO_VECTOR;
				this.changed = true;
				this.mode = TeleportMover.Mode.Wait;
			}
			return this.changed;
		}

		private const int TIMER_LENGTH = 33;

		private CollisionManager collisionManager;

		private ICollidable collidable;

		private FloatRect area;

		private float maxDistance;

		private float chaseThreshold;

		private long timerEnd;

		private TeleportMover.Mode mode;

		private bool changed;

		private enum Mode
		{
			Wait,
			Teleport
		}
	}
}
