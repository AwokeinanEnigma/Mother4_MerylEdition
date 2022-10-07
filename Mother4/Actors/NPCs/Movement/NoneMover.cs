using System;
using SFML.System;

namespace Mother4.Actors.NPCs.Movement
{
	internal class NoneMover : Mover
	{
		public override bool GetNextMove(ref Vector2f position, ref Vector2f velocity, ref int direction)
		{
			return false;
		}
	}
}
