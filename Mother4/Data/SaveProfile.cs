using System;
using SFML.System;

namespace Mother4.Data
{
	internal struct SaveProfile
	{
		public bool IsValid;

		public int Index;

		public CharacterType[] Party;

		public string MapName;

		public Vector2f Position;

		public int Time;

		public int Flavor;
	}
}
