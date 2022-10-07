using System;
using System.Collections.Generic;
using Carbine;
using Carbine.Graphics;
using Mother4.Data;
using Mother4.Utility;
using SFML.Graphics;
using SFML.System;

namespace Mother4.Overworld
{
	internal class BattleSwirlOverlay : Renderable
	{
		public event BattleSwirlOverlay.AnimationCompleteHandler OnAnimationComplete;

		public BattleSwirlOverlay(BattleSwirlOverlay.Style style, int depth, float speed)
		{
			this.depth = depth;
			string file = BattleSwirlOverlay.RESOURCES[style];
			this.textures = TextureManager.Instance.UseMultipart(file);
			this.gradMap = TextureManager.Instance.UseUnprocessed(Paths.BATTLE_SWIRL + "gradmap.png");
			Random random = new Random(5551247);
			int num = (Engine.Random.Next(100) >= 50) ? -1 : 1;
			int num2 = (Engine.Random.Next(100) >= 50) ? -1 : 1;
			int num3 = num * 160;
			int num4 = num2 * 90;
			this.layers = new VertexArray[this.textures.Length - 1];
			this.delta = new float[this.layers.Length];
			this.speed = new float[this.layers.Length];
			for (int i = 0; i < this.layers.Length; i++)
			{
				this.speed[i] = speed + (float)((random.NextDouble() - 0.5) * 0.002);
				this.delta[i] = 0f;
				this.layers[i] = new VertexArray(PrimitiveType.Quads, 4U);
				this.layers[i][0U] = new Vertex(new Vector2f((float)(-(float)num3), (float)(-(float)num4)), new Vector2f(0f, 0f));
				this.layers[i][1U] = new Vertex(new Vector2f((float)num3, (float)(-(float)num4)), new Vector2f(1f, 0f));
				this.layers[i][2U] = new Vertex(new Vector2f((float)num3, (float)num4), new Vector2f(1f, 1f));
				this.layers[i][3U] = new Vertex(new Vector2f((float)(-(float)num3), (float)num4), new Vector2f(0f, 1f));
				this.textures[1 + i].CurrentPalette = BattleSwirlOverlay.PALETTES[style];
			}
			this.scale = new Vector2f(1f, 1f);
			this.position = this.position;
			this.origin = this.textures[0].GetSpriteDefinition("default").Origin;
			this.depth = depth;
			this.size = Engine.SCREEN_SIZE;
			this.blend = new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue, 160);
			this.shader = new Shader(EmbeddedResources.GetStream("Mother4.Resources.bbg.vert"), EmbeddedResources.GetStream("Mother4.Resources.gradmap.frag"));
			this.shader.SetParameter("gradmap", this.gradMap.Image);
			this.shader.SetParameter("image", this.textures[1].Image);
			this.shader.SetParameter("palette", this.textures[1].Palette);
			this.shader.SetParameter("palIndex", this.textures[1].CurrentPaletteFloat);
			this.shader.SetParameter("palSize", this.textures[1].PaletteSize);
			this.shader.SetParameter("blend", this.blend);
			this.shader.SetParameter("blendMode", 1f);
			this.shader.SetParameter("delta", 0f);
			this.renderStates = new RenderStates(BlendMode.Alpha, Transform.Identity, null, this.shader);
			this.UpdatePosition(ViewManager.Instance.FinalCenter);
		}

		public void Reset()
		{
			for (int i = 0; i < this.delta.Length; i++)
			{
				this.delta[i] = 0f;
			}
			this.isComplete = false;
		}

		private void UpdatePosition(Vector2f position)
		{
			this.position = position;
			this.transform = Transform.Identity;
			this.transform.Translate(this.position);
			this.renderStates.Transform = this.transform;
		}

		public override void Draw(RenderTarget target)
		{
			if (this.visible)
			{
				this.UpdatePosition(ViewManager.Instance.FinalCenter);
				bool flag = true;
				for (int i = 0; i < this.layers.Length; i++)
				{
					if (!this.isComplete)
					{
						this.delta[i] = Math.Min(1f, this.delta[i] + this.speed[i]);
						this.shader.SetParameter("delta", this.delta[i]);
					}
					flag &= (this.delta[i] >= 1f);
					this.shader.SetParameter("image", this.textures[1 + i].Image);
					this.shader.SetParameter("palette", this.textures[1 + i].Palette);
					this.shader.SetParameter("palIndex", this.textures[1 + i].CurrentPaletteFloat);
					this.shader.SetParameter("palSize", this.textures[1 + i].PaletteSize);
					target.Draw(this.layers[i], this.renderStates);
				}
				if (!this.isComplete && flag)
				{
					this.isComplete = true;
					if (this.OnAnimationComplete != null)
					{
						this.OnAnimationComplete(this);
					}
				}
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					for (int i = 0; i < this.layers.Length; i++)
					{
						this.layers[i].Dispose();
					}
				}
				TextureManager.Instance.Unuse(this.textures);
				TextureManager.Instance.Unuse(this.gradMap);
			}
			base.Dispose(disposing);
		}

		private const int RANDOM_SEED = 5551247;

		private const string SPRITE_NAME = "default";

		private static readonly Dictionary<BattleSwirlOverlay.Style, string> RESOURCES = new Dictionary<BattleSwirlOverlay.Style, string>
		{
			{
				BattleSwirlOverlay.Style.Blue,
				Paths.BATTLE_SWIRL + "green.sdat"
			},
			{
				BattleSwirlOverlay.Style.Green,
				Paths.BATTLE_SWIRL + "green.sdat"
			},
			{
				BattleSwirlOverlay.Style.Red,
				Paths.BATTLE_SWIRL + "green.sdat"
			},
			{
				BattleSwirlOverlay.Style.Boss,
				Paths.BATTLE_SWIRL + "green.sdat"
			}
		};

		private static readonly Dictionary<BattleSwirlOverlay.Style, uint> PALETTES = new Dictionary<BattleSwirlOverlay.Style, uint>
		{
			{
				BattleSwirlOverlay.Style.Blue,
				1U
			},
			{
				BattleSwirlOverlay.Style.Green,
				0U
			},
			{
				BattleSwirlOverlay.Style.Red,
				2U
			},
			{
				BattleSwirlOverlay.Style.Boss,
				0U
			}
		};

		private VertexArray[] layers;

		private IndexedTexture[] textures;

		private FullColorTexture gradMap;

		private Shader shader;

		private RenderStates renderStates;

		private Transform transform;

		private Vector2f scale;

		private Color blend;

		private float[] speed;

		private float[] delta;

		private bool isComplete;

		public enum Style
		{
			Blue,
			Green,
			Red,
			Boss
		}

		public delegate void AnimationCompleteHandler(BattleSwirlOverlay anim);
	}
}
