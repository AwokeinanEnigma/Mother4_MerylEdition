using System;
using System.Collections.Generic;
using Carbine.Utility;
using SFML.System;

namespace Mother4.Actors.NPCs.Movement
{
	internal class PathMover : Mover
	{
		public event PathMover.OnPathCompleteHandler OnPathComplete;

		public PathMover(float speed, int timeOut, bool loop, List<Vector2f> path)
		{
			this.speed = speed;
			this.timeOut = timeOut;
			this.loop = loop;
			this.path = new List<Vector2f>(path);
			this.nodeIndex = 0;
			this.target = path[this.nodeIndex];
			this.incr = 1;
		}

		private bool IsAtTarget(ref Vector2f position)
		{
			return Math.Abs(this.target.X - position.X) <= 1f && Math.Abs(this.target.Y - position.Y) <= 1f;
		}

		private void DoStep(ref Vector2f position, ref Vector2f velocity, ref int direction)
		{
			base.MovementStep(this.speed, ref position, ref this.target, ref velocity);
			this.changed = true;
			if (this.IsAtTarget(ref position))
			{
				this.targetReachedFlag = true;
				velocity = VectorMath.ZERO_VECTOR;
			}
		}

		public override bool GetNextMove(ref Vector2f position, ref Vector2f velocity, ref int direction)
		{
			this.changed = false;
			if (!this.targetReachedFlag)
			{
				this.DoStep(ref position, ref velocity, ref direction);
			}
			else if (this.timer < this.timeOut)
			{
				this.timer++;
			}
			else
			{
				if (this.nodeIndex + this.incr >= this.path.Count)
				{
					if (this.loop)
					{
						this.nodeIndex = 0;
					}
					else
					{
						this.incr = -this.incr;
						if (this.OnPathComplete != null)
						{
							this.OnPathComplete();
						}
					}
				}
				else if (this.nodeIndex + this.incr < 0)
				{
					if (this.loop)
					{
						this.nodeIndex = this.path.Count - 1;
					}
					else
					{
						this.incr = -this.incr;
						if (this.OnPathComplete != null)
						{
							this.OnPathComplete();
						}
					}
				}
				this.nodeIndex += this.incr;
				this.target = this.path[this.nodeIndex];
				this.timer = 0;
				this.targetReachedFlag = false;
			}
			return this.changed;
		}

		private List<Vector2f> path;

		private Vector2f target;

		private int nodeIndex;

		private int incr;

		private int timer;

		private int timeOut;

		private float speed;

		private bool loop;

		private bool changed;

		private bool targetReachedFlag;

		public delegate void OnPathCompleteHandler();
	}
}
