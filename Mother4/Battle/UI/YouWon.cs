using System;
using Carbine.Graphics;
using Mother4.Data;
using SFML.System;

namespace Mother4.Battle.UI
{
	internal class YouWon : IDisposable
	{
		public YouWon(RenderPipeline pipeline)
		{
			this.pipeline = pipeline;
			this.youWon = new IndexedColorGraphic(Paths.GRAPHICS + "youwon.dat", "youwon", YouWon.POSITION, 2147450980);
			this.pipeline.Add(this.youWon);
			this.frameCount = (this.youWon.Texture as IndexedTexture).PaletteCount - 1U;
		}

		~YouWon()
		{
			this.Dispose(false);
		}

		public void Update()
		{
			if (this.frame < this.frameCount)
			{
				this.frameTimer++;
				if (this.frameTimer > 4)
				{
					this.frame += 1U;
					this.youWon.CurrentPalette = this.frame;
					this.frameTimer = 0;
				}
			}
		}

		public void Remove()
		{
			this.pipeline.Remove(this.youWon);
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected void Dispose(bool disposing)
		{
			if (!this.disposed && disposing)
			{
				this.youWon.Dispose();
			}
			this.disposed = true;
		}

		private const int DEPTH = 2147450980;

		private const int FRAME_DELAY = 4;

		private static Vector2f POSITION = new Vector2f(160f, 18f);

		private bool disposed;

		private RenderPipeline pipeline;

		private IndexedColorGraphic youWon;

		private uint frameCount;

		private uint frame;

		private int frameTimer;
	}
}
