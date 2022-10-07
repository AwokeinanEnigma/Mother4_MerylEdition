using System;
using System.Collections.Generic;
using Carbine;
using Carbine.Graphics;
using Carbine.Scenes.Transitions;
using Carbine.Utility;
using Mother4.Overworld;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Scenes.Transitions
{
	internal class BattleSwirlTransition : ITransition
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
				return this.isComplete;
			}
		}

		public bool Blocking { get; set; }

		public BattleSwirlTransition(BattleSwirlOverlay.Style style)
		{
			this.overlay = new BattleSwirlOverlay(style, 0, 0.015f);
			this.overlay.OnAnimationComplete += this.overlay_OnAnimationComplete;
			int num = 160;
			int num2 = 90;
			this.fadeColor = BattleSwirlTransition.COLOR_MAP[style];
			this.fadeStartColor = new Color(this.fadeColor);
			this.fadeStartColor.A = 0;
			this.fadeVerts = new Vertex[4];
			this.fadeVerts[0] = new Vertex(new Vector2f((float)(-(float)num), (float)(-(float)num2)), this.fadeStartColor);
			this.fadeVerts[1] = new Vertex(new Vector2f((float)num, (float)(-(float)num2)), this.fadeStartColor);
			this.fadeVerts[2] = new Vertex(new Vector2f((float)num, (float)num2), this.fadeStartColor);
			this.fadeVerts[3] = new Vertex(new Vector2f((float)(-(float)num), (float)num2), this.fadeStartColor);
			this.fadeStates = new RenderStates(BlendMode.Alpha, Transform.Identity, null, null);
			this.UpdateTransform();
		}

		private void overlay_OnAnimationComplete(BattleSwirlOverlay anim)
		{
			this.overlay.OnAnimationComplete -= this.overlay_OnAnimationComplete;
			this.isSwirlComplete = true;
		}

		private void UpdateTransform()
		{
			this.fadeStates.Transform = new Transform(1f, 0f, ViewManager.Instance.FinalCenter.X, 0f, 1f, ViewManager.Instance.FinalCenter.Y, 0f, 0f, 1f);
		}

		public void Update()
		{
			if (!this.isComplete)
			{
				if (this.isSwirlComplete)
				{
					this.progress += 0.024f;
					Color color = (this.progress < 0.7f) ? ColorHelper.BlendAlpha(this.fadeStartColor, this.fadeColor, this.progress / 0.7f) : ColorHelper.BlendAlpha(this.fadeColor, Color.Black, (this.progress - 0.7f) / 0.3f);
					for (int i = 0; i < this.fadeVerts.Length; i++)
					{
						this.fadeVerts[i].Color = color;
					}
				}
				if (this.progress >= 1f)
				{
					this.overlay.Dispose();
					this.overlay = null;
					this.fadeVerts = null;
					this.progress = 1f;
					this.isComplete = true;
				}
			}
		}

		public void Draw()
		{
			if (!this.isComplete)
			{
				this.UpdateTransform();
				this.overlay.Draw(Engine.FrameBuffer);
				Engine.FrameBuffer.Draw(this.fadeVerts, PrimitiveType.Quads, this.fadeStates);
			}
		}

		public void Reset()
		{
			this.overlay.Reset();
		}

		private const float SWIRL_SPEED = 0.015f;

		private const float FADE_SPEED = 0.024f;

		private static Dictionary<BattleSwirlOverlay.Style, Color> COLOR_MAP = new Dictionary<BattleSwirlOverlay.Style, Color>
		{
			{
				BattleSwirlOverlay.Style.Green,
				new Color(148, 214, 161, byte.MaxValue)
			},
			{
				BattleSwirlOverlay.Style.Blue,
				new Color(160, 234, 250, byte.MaxValue)
			},
			{
				BattleSwirlOverlay.Style.Red,
				new Color(250, 160, 167, byte.MaxValue)
			},
			{
				BattleSwirlOverlay.Style.Boss,
				new Color(242, 220, 179, byte.MaxValue)
			}
		};

		private bool isComplete;

		private bool isSwirlComplete;

		private float progress;

		private BattleSwirlOverlay overlay;

		private Vertex[] fadeVerts;

		private RenderStates fadeStates;

		private Color fadeColor;

		private Color fadeStartColor;
	}
}
