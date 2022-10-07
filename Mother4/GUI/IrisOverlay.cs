using System;
using Carbine.Graphics;
using Mother4.Utility;
using SFML.Graphics;
using SFML.System;

namespace Mother4.GUI
{
	internal class IrisOverlay : Renderable
	{
		public float Progress
		{
			get
			{
				return this.progress;
			}
			set
			{
				this.SetProgress(value);
			}
		}

		public float Speed
		{
			get
			{
				return this.speed;
			}
			set
			{
				this.speed = value;
			}
		}

		public event IrisOverlay.AnimationCompleteHandler OnAnimationComplete;

		public IrisOverlay(Vector2f position, Vector2f origin, float progress)
		{
			this.position = position;
			this.origin = origin;
			this.progress = progress;
			this.animationDone = true;
			this.size = new Vector2f(320f, 180f);
			this.depth = 2147450880;
			int num = 160;
			int num2 = 90;
			this.verts = new VertexArray(PrimitiveType.Quads, 4U);
			this.verts[0U] = new Vertex(new Vector2f((float)(-(float)num), (float)(-(float)num2)), new Vector2f(0f, 0f));
			this.verts[1U] = new Vertex(new Vector2f((float)num, (float)(-(float)num2)), new Vector2f(1f, 0f));
			this.verts[2U] = new Vertex(new Vector2f((float)num, (float)num2), new Vector2f(1f, 1f));
			this.verts[3U] = new Vertex(new Vector2f((float)(-(float)num), (float)num2), new Vector2f(0f, 1f));
			this.shader = new Shader(EmbeddedResources.GetStream("Mother4.Resources.bbg.vert"), EmbeddedResources.GetStream("Mother4.Resources.iris.frag"));
			this.shader.SetParameter("progress", this.progress);
			this.shader.SetParameter("size", this.size);
			this.transform = Transform.Identity;
			this.transform.Translate(this.position);
			this.states = new RenderStates(BlendMode.Alpha, this.transform, null, this.shader);
		}

		private void UpdatePosition(Vector2f position)
		{
			this.position = position;
			this.transform = Transform.Identity;
			this.transform.Translate(this.position);
			this.states.Transform = this.transform;
		}

		private void UpdateProgress(float progress)
		{
			float num = this.progress;
			this.progress = progress;
			if (this.progress != num)
			{
				this.shader.SetParameter("progress", this.progress);
			}
		}

		private void SetProgress(float progress)
		{
			if (this.speed > 0f)
			{
				this.targetProgress = progress;
				this.animationDone = false;
				return;
			}
			this.UpdateProgress(progress);
		}

		private void UpdateAnimation()
		{
			if (!this.animationDone && this.speed > 0f)
			{
				if (Math.Abs(this.targetProgress - this.progress) > 0.01f)
				{
					float num = this.progress + (float)Math.Sign(this.targetProgress - this.progress) * this.speed;
					this.UpdateProgress(num);
					return;
				}
				this.animationDone = true;
				this.progress = this.targetProgress;
				if (this.OnAnimationComplete != null)
				{
					this.OnAnimationComplete(this);
				}
			}
		}

		public override void Draw(RenderTarget target)
		{
			this.UpdatePosition(ViewManager.Instance.FinalCenter);
			this.UpdateAnimation();
			target.Draw(this.verts, this.states);
		}

		private const string PARAM_PROGRESS = "progress";

		private const string PARAM_SIZE = "size";

		private float progress;

		private float targetProgress;

		private float speed;

		private bool animationDone;

		private Shader shader;

		private Transform transform;

		private RenderStates states;

		private VertexArray verts;

		public delegate void AnimationCompleteHandler(IrisOverlay sender);
	}
}
