using System;
using System.Collections.Generic;
using Carbine.Utility;
using Mother4.Actors;
using Mother4.Data;
using SFML.System;

namespace Mother4.Overworld
{
	internal class PartyTrain
	{
		public bool Running
		{
			get
			{
				return this.running;
			}
			set
			{
				this.running = value;
			}
		}

		public bool Crouching
		{
			get
			{
				return this.crouching;
			}
			set
			{
				this.crouching = value;
			}
		}

		public bool MovementLocked
		{
			get
			{
				return this.movementLocked;
			}
			set
			{
				this.movementLocked = value;
			}
		}

		public event PartyTrain.OnResetHandler OnReset;

		public PartyTrain(Vector2f initialPosition, int direction, TerrainType initialTerrain, bool extend)
		{
			this.followers = new List<PartyFollower>();
			this.recordPoints = new PartyTrain.RecordPoint[1];
			this.Reset(initialPosition, direction, initialTerrain, extend);
		}

		public void Reset(Vector2f position, int direction, TerrainType terrain)
		{
			this.Reset(position, direction, terrain, false);
		}

		public void Reset(Vector2f position, int direction, TerrainType terrain, bool extend)
		{
			Vector2f v = VectorMath.DirectionToVector((direction + 4) % 8);
			for (int i = 0; i < this.recordPoints.Length; i++)
			{
				this.recordPoints[i].position = ((!extend) ? position : (position + (float)(this.recordPoints.Length - i) * v));
				this.recordPoints[i].velocity = VectorMath.DirectionToVector(direction);
				this.recordPoints[i].terrain = terrain;
			}
			if (this.OnReset != null)
			{
				this.OnReset(position, direction);
			}
		}

		private int FindNextPlace()
		{
			int num = 0;
			for (int i = 0; i < this.followers.Count; i++)
			{
				PartyFollower partyFollower = this.followers[i];
				num += (int)partyFollower.Width + 2;
			}
			return num;
		}

		private int GetRecordPointIndexFromPlace(int place)
		{
			int i;
			for (i = this.pos - place; i < 0; i += this.recordPoints.Length)
			{
			}
			return i;
		}

		private void ResizeRecording()
		{
			int num = this.recordPoints.Length;
			int num2 = 0;
			foreach (PartyFollower partyFollower in this.followers)
			{
				num2 += (int)partyFollower.Width + 2;
			}
			num2++;
			if (num2 != num)
			{
				PartyTrain.RecordPoint[] array = new PartyTrain.RecordPoint[num2];
				int num3 = (this.pos + 1) % this.recordPoints.Length;
				for (int i = 0; i < array.Length; i++)
				{
					if (i < this.recordPoints.Length)
					{
						int num4 = (this.pos + 1 + i) % this.recordPoints.Length;
						array[i] = this.recordPoints[num4];
					}
					else
					{
						array[i] = this.recordPoints[num3];
					}
				}
				if (num2 > num)
				{
					this.pos = this.recordPoints.Length - 1;
				}
				this.recordPoints = array;
			}
		}

		public void Add(PartyFollower follower)
		{
			this.followers.Add(follower);
			this.ResizeRecording();
			follower.Place = this.FindNextPlace();
			float width = follower.Width;
			int num = (this.pos + 1) % this.recordPoints.Length;
			int num2 = this.recordPoints.Length - ((this.followers.Count == 1) ? 0 : 1);
			Vector2f position = this.recordPoints[num].position;
			Vector2f position2 = follower.Position;
			int direction = VectorMath.VectorToDirection(position - position2);
			Vector2f velocity = VectorMath.DirectionToVector(direction);
			for (int i = num; i <= num2; i++)
			{
				float num3 = (float)(i - num) / (float)(num2 - num);
				int num4 = i % this.recordPoints.Length;
				this.recordPoints[num4].position.X = (float)((int)(position2.X + (position.X - position2.X) * num3));
				this.recordPoints[num4].position.Y = (float)((int)(position2.Y + (position.Y - position2.Y) * num3));
				this.recordPoints[num4].velocity = velocity;
			}
		}

		public PartyFollower Remove(PartyFollower follower)
		{
			float width = follower.Width;
			int place = follower.Place;
			int num = this.followers.IndexOf(follower);
			this.followers.Remove(follower);
			follower.Place = -1;
			int num2 = (num - 1 < 0) ? 0 : this.followers[num - 1].Place;
			for (int i = num; i < this.followers.Count; i++)
			{
				int num3 = (int)this.followers[i].Width + 2;
				int place2 = this.followers[i].Place;
				int num4 = (i - 1 < 0) ? 0 : this.followers[i - 1].Place;
				int num5 = num4 + num3;
				this.followers[i].Place = num5;
				for (int j = num4; j <= num5; j++)
				{
					int recordPointIndexFromPlace = this.GetRecordPointIndexFromPlace(j);
					float num6 = (float)(j - num4) / (float)(num5 - num4);
					int place3 = num2 + (int)((float)(place2 - num2) * num6);
					int recordPointIndexFromPlace2 = this.GetRecordPointIndexFromPlace(place3);
					this.recordPoints[recordPointIndexFromPlace] = this.recordPoints[recordPointIndexFromPlace2];
				}
				num2 = place2;
			}
			this.ResizeRecording();
			return follower;
		}

		public PartyFollower Remove(int index)
		{
			PartyFollower result = null;
			if (index >= 0 && index < this.followers.Count)
			{
				result = this.Remove(this.followers[index]);
			}
			return result;
		}

		public IList<PartyFollower> Remove(CharacterType character)
		{
			IList<PartyFollower> list = this.followers.FindAll((PartyFollower x) => x.Character == character);
			foreach (PartyFollower follower in list)
			{
				this.Remove(follower);
			}
			return list;
		}

		public void Record(Vector2f position, Vector2f velocity, TerrainType terrain)
		{
			this.pos = (this.pos + 1) % this.recordPoints.Length;
			this.recordPoints[this.pos].position = position;
			this.recordPoints[this.pos].velocity = velocity;
			this.recordPoints[this.pos].terrain = terrain;
		}

		public void Update()
		{
			for (int i = 0; i < this.followers.Count; i++)
			{
				int place = this.followers[i].Place;
				this.followers[i].Update(this.GetPosition(place), this.GetVelocity(place), this.GetTerrain(place));
			}
		}

		public ICollection<PartyFollower> GetFollowers(CharacterType character)
		{
			return this.followers.FindAll((PartyFollower x) => x.Character == character);
		}

		public Vector2f GetPosition(int place)
		{
			int i;
			for (i = this.pos - place; i < 0; i += this.recordPoints.Length)
			{
			}
			return this.recordPoints[i].position;
		}

		public Vector2f GetVelocity(int place)
		{
			int i;
			for (i = this.pos - place; i < 0; i += this.recordPoints.Length)
			{
			}
			return this.recordPoints[i].velocity;
		}

		public TerrainType GetTerrain(int place)
		{
			int i;
			for (i = this.pos - place; i < 0; i += this.recordPoints.Length)
			{
			}
			return this.recordPoints[i].terrain;
		}

		private const int INITIAL_LENGTH = 1;

		private PartyTrain.RecordPoint[] recordPoints;

		private int pos;

		private bool running;

		private bool crouching;

		private bool movementLocked;

		private List<PartyFollower> followers;

		private struct RecordPoint
		{
			public Vector2f position;

			public Vector2f velocity;

			public TerrainType terrain;
		}

		public delegate void OnResetHandler(Vector2f position, int direction);
	}
}
