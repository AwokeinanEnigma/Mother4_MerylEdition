using System;
using Carbine.Graphics;
using Carbine.Utility;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Utility
{
	internal class ScreenDimmer : IDisposable
	{
		public event ScreenDimmer.OnFadeCompleteHandler OnFadeComplete;

		public Color TargetColor
		{
			get
			{
				return this.targetColor;
			}
		}

		public ScreenDimmer(RenderPipeline pipeline, Color color, int duration, int depth)
		{
			this.pipeline = pipeline;
			this.ChangeColor(Color.Transparent, color, duration);
			this.rect = new RectangleShape(new Vector2f(320f, 180f));
			this.shape = new ShapeGraphic(this.rect, ViewManager.Instance.FinalCenter, new Vector2f((float)((int)(this.rect.Size.X / 2f)), (float)((int)(this.rect.Size.Y / 2f))), this.rect.Size, depth);
			this.rect.FillColor = this.currentColor;
			this.pipeline.Add(this.shape);
			ViewManager.Instance.OnMove += this.OnViewMove;
		}

		~ScreenDimmer()
		{
			this.Dispose(false);
		}

		private void ChangeColor(Color fromColor, Color toColor, int duration)
		{
			if (duration > 0)
			{
				this.currentColor = fromColor;
				this.targetColor = toColor;
				this.speed = 1f / (float)duration;
				this.progress = 0f;
				this.isTransitioning = true;
				return;
			}
			this.currentColor = toColor;
			this.targetColor = toColor;
			this.speed = 0f;
			this.progress = 1f;
			this.isTransitioning = false;
		}

		public void ChangeColor(Color color, int duration)
		{
			this.ChangeColor(ColorHelper.BlendAlpha(this.currentColor, this.targetColor, this.progress), color, duration);
		}

		private void OnViewMove(ViewManager sender, Vector2f newCenter)
		{
			this.rect.Position = sender.FinalCenter;
		}

		public void Update()
		{
			if (this.progress < 1f)
			{
				this.rect.FillColor = ColorHelper.BlendAlpha(this.currentColor, this.targetColor, this.progress);
				this.progress += this.speed;
				return;
			}
			if (this.isTransitioning)
			{
				this.rect.FillColor = this.targetColor;
				this.isTransitioning = false;
				if (this.OnFadeComplete != null)
				{
					this.OnFadeComplete(this);
				}
			}
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					this.shape.Dispose();
					this.rect.Dispose();
				}
				ViewManager.Instance.OnMove -= this.OnViewMove;
				this.disposed = true;
			}
		}

		private bool disposed;

		private RenderPipeline pipeline;

		private ShapeGraphic shape;

		private RectangleShape rect;

		private bool isTransitioning;

		private float progress;

		private float speed;

		private Color currentColor;

		private Color targetColor;

		public delegate void OnFadeCompleteHandler(ScreenDimmer sender);
	}
}
