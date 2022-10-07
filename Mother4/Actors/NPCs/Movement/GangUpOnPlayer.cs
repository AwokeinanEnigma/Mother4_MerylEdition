using System;
using Carbine.Graphics;
using Carbine.Utility;
using SFML.System;

namespace Mother4.Actors.NPCs.Movement
{
	// Token: 0x02000008 RID: 8
	internal class GangUpOnPlayer : Mover
	{
		// Token: 0x0600000A RID: 10 RVA: 0x00002E77 File Offset: 0x00001077
		public GangUpOnPlayer(float speed)
		{
            this.speed = speed;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002E94 File Offset: 0x00001094
		public override bool GetNextMove(ref Vector2f position, ref Vector2f velocity, ref int direction)
		{
//			this.changed = false;

			//Console.WriteLine("Coming for the player!");
            if (ViewManager.Instance.FollowActor != null)
            {


                direction = VectorMath.VectorToDirection(ViewManager.Instance.FollowActor.Position - position);
                velocity = VectorMath.DirectionToVector(direction) * this.speed;
                this.changed = true;

            }

            return true;
        }

        private bool changed;
        private float speed;
    }
}
