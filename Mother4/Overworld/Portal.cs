using System;
using Carbine.Collision;
using Carbine.Utility;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Overworld
{
	internal class Portal : ICollidable
	{
		public Vector2f Position
		{
			get
			{
				return this.position;
			}
			set
			{
				this.position = value;
			}
		}

		public Vector2f Velocity
		{
			get
			{
				return VectorMath.ZERO_VECTOR;
			}
		}

		public AABB AABB
		{
			get
			{
				return this.mesh.AABB;
			}
		}

		public Mesh Mesh
		{
			get
			{
				return this.mesh;
			}
		}

		public bool Solid
		{
			get
			{
				return this.solid;
			}
			set
			{
				this.solid = value;
			}
		}

		public string Map
		{
			get
			{
				return this.map;
			}
		}

		public Vector2f PositionTo
		{
			get
			{
				return this.positionTo;
			}
		}

		public int DirectionTo
		{
			get
			{
				return this.directionTo;
			}
		}

		public VertexArray DebugVerts { get; private set; }

		public Portal(int x, int y, int width, int height, int xTo, int yTo, int dirTo, string map)
		{
			this.position = new Vector2f((float)x, (float)y);
			this.positionTo = new Vector2f((float)xTo, (float)yTo);
			this.directionTo = dirTo;
			this.mesh = new Mesh(new FloatRect(VectorMath.ZERO_VECTOR, new Vector2f((float)width, (float)height)));
			this.map = map;
			this.solid = true;
			VertexArray vertexArray = new VertexArray(PrimitiveType.LinesStrip, (uint)(this.mesh.Vertices.Count + 1));
			for (int i = 0; i < this.mesh.Vertices.Count; i++)
			{
				vertexArray[(uint)i] = new Vertex(this.mesh.Vertices[i], Color.Blue);
			}
			vertexArray[(uint)this.mesh.Vertices.Count] = new Vertex(this.mesh.Vertices[0], Color.Blue);
			this.DebugVerts = vertexArray;
		}

		public void Collision(CollisionContext context)
		{
		}

		private Vector2f position;

		private Vector2f positionTo;

		private int directionTo;

		private Mesh mesh;

		private bool solid;

		private string map;
	}
}
