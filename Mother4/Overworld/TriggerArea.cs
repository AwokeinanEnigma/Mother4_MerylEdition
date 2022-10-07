using System;
using System.Collections.Generic;
using Carbine.Collision;
using Carbine.Utility;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Overworld
{
	internal class TriggerArea : ICollidable
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

		public int Flag
		{
			get
			{
				return this.flag;
			}
		}

		public string Script
		{
			get
			{
				return this.script;
			}
		}

		public VertexArray DebugVerts { get; private set; }

		public TriggerArea(Vector2f position, List<Vector2f> points, int flag, string script)
		{
			this.position = position;
			this.mesh = new Mesh(points);
			this.flag = flag;
			this.script = script;
			this.solid = true;
			VertexArray vertexArray = new VertexArray(PrimitiveType.LinesStrip, (uint)(this.mesh.Vertices.Count + 1));
			for (int i = 0; i < this.mesh.Vertices.Count; i++)
			{
				vertexArray[(uint)i] = new Vertex(this.mesh.Vertices[i], Color.Magenta);
			}
			vertexArray[(uint)this.mesh.Vertices.Count] = new Vertex(this.mesh.Vertices[0], Color.Magenta);
			this.DebugVerts = vertexArray;
		}

		public void Collision(CollisionContext context)
		{
		}

		private Vector2f position;

		private Mesh mesh;

		private bool solid;

		private int flag;

		private string script;
	}
}
