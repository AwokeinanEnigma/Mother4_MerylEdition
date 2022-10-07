using System;
using Carbine;
using Carbine.Graphics;
using Carbine.Utility;
using Mother4.Data;
using SFML.Graphics;
using SFML.System;
using Mother4.Scenes;

namespace Mother4.Overworld
{
	// Token: 0x02000102 RID: 258
	internal class RainOverlay
	{
		// Token: 0x060005F7 RID: 1527 RVA: 0x00023378 File Offset: 0x00021578
		public RainOverlay()
		{
			this.drops = new RainOverlay.Droplet[50];
			this.splashes = new IndexedColorGraphic[this.drops.Length];
			for (int i = 0; i < this.drops.Length; i++)
			{
				int num = Engine.Random.Next(RainOverlay.COLOR_CHOICES.Length);
				this.drops[i] = new RainOverlay.Droplet(RainOverlay.COLOR_CHOICES[num]);
				int num2 = Engine.Random.Next(RainOverlay.SPLASH_CHOICES.Length);
				this.splashes[i] = new IndexedColorGraphic(Paths.GRAPHICS + "rainsplash.dat", RainOverlay.SPLASH_CHOICES[num2], new Vector2f(-9999f, -9999f), 0);
				this.splashes[i].Visible = false;
			}
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x00023450 File Offset: 0x00021650
		public void Update()
		{
			for (int i = 0; i < this.drops.Length; i++)
			{
				bool flag = this.drops[i].Update();
				if (flag)
				{
					IndexedColorGraphic splash = this.splashes[i];
				//	Console.WriteLine(OverworldScene.instance.MapGroups[0].(splash.Position));
					splash.Position = VectorMath.Truncate(this.drops[i].Position);
					splash.Depth = (int)this.drops[i].Position.Y;
					splash.Frame = 0f;
					splash.Visible = true;
					splash.OnAnimationComplete += this.OnAnimationComplete;
				}
			}
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x0002350E File Offset: 0x0002170E
		private void OnAnimationComplete(AnimatedRenderable graphic)
		{
			graphic.Visible = false;
			graphic.OnAnimationComplete -= this.OnAnimationComplete;
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x0002352C File Offset: 0x0002172C
		public void Draw(RenderTarget target)
		{
			for (int i = 0; i < this.drops.Length; i++)
			{
				this.drops[i].Draw(target);
				if (this.splashes[i].Visible)
				{
					this.splashes[i].Draw(target);
				}
			}
		}

		// Token: 0x040007B8 RID: 1976
		private static readonly Color[] COLOR_CHOICES = new Color[]
		{
			new Color(203, 219, 252),
			new Color(142, 177, 248),
			new Color(151, 170, 210)
		};

		// Token: 0x040007B9 RID: 1977
		private static readonly string[] SPLASH_CHOICES = new string[]
		{
			"splash1",
			"splash2",
			"splash3"
		};

		private RainOverlay.Droplet[] drops;
		private IndexedColorGraphic[] splashes;

		private struct Droplet
		{
			public Vector2f Position
			{
				get
				{
					return this.position;
				}
			}

			// Token: 0x060005FD RID: 1533 RVA: 0x00023624 File Offset: 0x00021824
			public Droplet(Color color)
			{
				this.position = VectorMath.ZERO_VECTOR;
				this.verts = new Vertex[]
				{
					new Vertex(this.position, color),
					new Vertex(this.position - RainOverlay.Droplet.DROP_SIZE, color)
				};
				this.position = new Vector2f(ViewManager.Instance.Viewrect.Left + (float)Engine.Random.Next(320), ViewManager.Instance.Viewrect.Top + (float)Engine.Random.Next(180));
				this.endY = Math.Min(ViewManager.Instance.Viewrect.Top + 180f, this.position.Y + (float)Engine.Random.Next(180));
			}

			private void ResetPosition()
			{
				this.position = new Vector2f(ViewManager.Instance.Viewrect.Left + (float)Engine.Random.Next(320), ViewManager.Instance.Viewrect.Top - (float)Engine.Random.Next(180));
				this.endY = this.position.Y + 180f;
				this.UpdateVertices();
			}

			private void UpdateVertices()
			{
				this.verts[0].Position = this.position;
				this.verts[1].Position = this.position - RainOverlay.Droplet.DROP_SIZE;
			}

			public bool Update()
			{
				bool result = false;
				this.position += RainOverlay.Droplet.DROP_VELOCITY;
				this.UpdateVertices();
				if (this.position.Y > this.endY)
				{
					this.ResetPosition();
				}
				else if (this.position.Y + RainOverlay.Droplet.DROP_VELOCITY.Y > this.endY)
				{
					result = true;
				}
				return result;
			}

			// Token: 0x06000601 RID: 1537 RVA: 0x00023823 File Offset: 0x00021A23
			public void Draw(RenderTarget target)
			{
				target.Draw(this.verts, PrimitiveType.Lines);
			}

			// Token: 0x040007BC RID: 1980
			private static readonly Vector2f DROP_SIZE = new Vector2f(0f, 24f);

			// Token: 0x040007BD RID: 1981
			private static readonly Vector2f DROP_VELOCITY = new Vector2f(0f, 8f);

			// Token: 0x040007BE RID: 1982
			private float endY;

			// Token: 0x040007BF RID: 1983
			private Vector2f position;

			// Token: 0x040007C0 RID: 1984
			private Vertex[] verts;
		}
	}
}
