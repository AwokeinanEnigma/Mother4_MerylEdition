using System;
using Carbine;
using Carbine.Graphics;
using Carbine.Scenes.Transitions;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Scenes.Transitions
{
	internal class BattleFadeTransition : ITransition
	{
		public bool IsComplete
		{
			get
			{
				return this.isComplete;
			}
		}

		public float Progress
		{
			get
			{
				return this.progress;
			}
		}

		public bool ShowNewScene
		{
			get
			{
				return this.progress > 1f - this.speed;
			}
		}

		public bool Blocking { get; set; }

		public BattleFadeTransition(float duration, Color color)
		{
			float num = 60f * duration;
			this.speed = 1f / num;
			this.color = color;
			this.isComplete = false;
			this.progress = 0f;
			this.target = Engine.FrameBuffer;
			float num2 = 160f;
			float num3 = 90f;
			this.verts = new Vertex[4];
			this.verts[0] = new Vertex(new Vector2f(-num2, -num3), color);
			this.verts[1] = new Vertex(new Vector2f(num2, -num3), color);
			this.verts[2] = new Vertex(new Vector2f(num2, num3), color);
			this.verts[3] = new Vertex(new Vector2f(-num2, num3), color);
			Transform transform = new Transform(1f, 0f, ViewManager.Instance.FinalCenter.X, 0f, 1f, ViewManager.Instance.FinalCenter.Y, 0f, 0f, 1f);
			this.renderStates = new RenderStates(transform);
		}

		public void Update()
		{
			this.progress += this.speed;
			this.isComplete = (this.progress > 1f);
			byte b;
			if (this.progress < 0.333f)
			{
				b = (byte)(255.0 * (Math.Cos((double)(this.progress * 3f) * 3.141592653589793 + 3.141592653589793) / 4.0 + 0.25));
			}
			else if (this.progress < 0.666f)
			{
				b = 128;
			}
			else if (this.progress < 1f)
			{
				b = (byte)(255.0 * (Math.Cos((double)((this.progress - 0.666f) * 3f) * 3.141592653589793 + 3.141592653589793) / 4.0 + 0.75));
			}
			else
			{
				b = (byte)(255f * (1f - (this.progress - 1f)));
			}
			b /= 12;
			b *= 12;
			byte b2;
			byte b3;
			byte b4;
			if (this.progress < 0.6f)
			{
				b2 = this.color.R;
				b3 = this.color.G;
				b4 = this.color.B;
			}
			else if (this.progress < 1f)
			{
				float num = 0.7f - (this.progress - 0.6f) / 0.4f;
				num = Math.Max(0f, num);
				b2 = (byte)((float)this.color.R * num);
				b3 = (byte)((float)this.color.G * num);
				b4 = (byte)((float)this.color.B * num);
				b2 /= 12;
				b2 *= 12;
				b3 /= 12;
				b3 *= 12;
				b4 /= 12;
				b4 *= 12;
			}
			else
			{
				float num2 = 1f - (this.progress - 1f);
				num2 = Math.Max(0f, num2);
				b2 = (byte)((float)this.color.R * num2);
				b3 = (byte)((float)this.color.G * num2);
				b4 = (byte)((float)this.color.B * num2);
				b2 /= 12;
				b2 *= 12;
				b3 /= 12;
				b3 *= 12;
				b4 /= 12;
				b4 *= 12;
			}
			this.SetVertColor(0, b2, b3, b4, b);
			this.SetVertColor(1, b2, b3, b4, b);
			this.SetVertColor(2, b2, b3, b4, b);
			this.SetVertColor(3, b2, b3, b4, b);
		}

		private void SetVertColor(int index, byte R, byte G, byte B, byte A)
		{
			this.verts[index].Color.R = R;
			this.verts[index].Color.G = G;
			this.verts[index].Color.B = B;
			this.verts[index].Color.A = A;
		}

		public void Draw()
		{
			this.renderStates.Transform = new Transform(1f, 0f, ViewManager.Instance.FinalCenter.X, 0f, 1f, ViewManager.Instance.FinalCenter.Y, 0f, 0f, 1f);
			this.target.Draw(this.verts, PrimitiveType.Quads, this.renderStates);
		}

		public void Reset()
		{
			this.isComplete = false;
			this.progress = 0f;
		}

		private const int STEPS = 20;

		private float speed;

		private bool isComplete;

		private float progress;

		private RenderTarget target;

		private Vertex[] verts;

		private RenderStates renderStates;

		private Color color;
	}
}
