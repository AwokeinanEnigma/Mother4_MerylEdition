using System;
using System.Collections.Generic;
using Carbine;
using Carbine.Graphics;
using Carbine.GUI;
using Carbine.Utility;
using Mother4.Data;
using Mother4.GUI;
using Mother4.Psi;
using SFML.Graphics;
using SFML.System;
using Carbine.Audio;

namespace Mother4.Battle.UI
{
	internal class BattleCard : IDisposable
	{
        private struct GlowSettings
        {
            // Token: 0x060004EE RID: 1262 RVA: 0x0001F593 File Offset: 0x0001D793
            public GlowSettings(Color color, ColorBlendMode mode)
            {
                this.Color = color;
                this.BlendMode = mode;
            }

            // Token: 0x040006CD RID: 1741
            public Color Color;

            // Token: 0x040006CE RID: 1742
            public ColorBlendMode BlendMode;
        }
		public BattleCard.GlowType Glow
        {
            get
            {
                return this.glowType;
            }
            set
            {
                this.SetGlow(value);
            }
		}

        // Token: 0x04000695 RID: 1685
        private const float GLOW_SPEED = 0.05f;

        // Token: 0x04000696 RID: 1686
        private const float GLOW_INTENSITY = 0.75f;

		private static readonly Dictionary<BattleCard.GlowType, BattleCard.GlowSettings> GLOW_COLORS = new Dictionary<BattleCard.GlowType, BattleCard.GlowSettings>
        {
            {
                BattleCard.GlowType.None,
                new BattleCard.GlowSettings(Color.White, ColorBlendMode.Multiply)
            },
            {
                BattleCard.GlowType.Shield,
                new BattleCard.GlowSettings(new Color(206, 226, 234), ColorBlendMode.Multiply)
            },
            {
                BattleCard.GlowType.Counter,
                new BattleCard.GlowSettings(new Color(byte.MaxValue, 249, 119), ColorBlendMode.Multiply)
            },
            {
                BattleCard.GlowType.PsiSheild,
                new BattleCard.GlowSettings(new Color(120, 232, 252), ColorBlendMode.Multiply)
            },
            {
                BattleCard.GlowType.PsiCounter,
                new BattleCard.GlowSettings(new Color(219, 121, 251), ColorBlendMode.Multiply)
            },
            {
                BattleCard.GlowType.Eraser,
                new BattleCard.GlowSettings(new Color(247, 136, 136), ColorBlendMode.Multiply)
            }
        };
		public void SetGlow(BattleCard.GlowType type)
        {
			this.glowType = type;
			BattleCard.GlowSettings glowSettings = BattleCard.GLOW_COLORS[this.glowType];
			this.glowColor = ColorHelper.Blend(glowSettings.Color, Color.Black, 0.75f);
			this.glowSpeed = 0.05f + (float)(Engine.Random.Next(20) - 10) / 1000f;
			this.glowDelta = 0f;
			this.card.ColorBlendMode = glowSettings.BlendMode;
		}
        private BattleCard.GlowType glowType;
        // Token: 0x040006BC RID: 1724
        private Color glowColor;

        // Token: 0x040006BD RID: 1725
        private float glowSpeed;

        // Token: 0x040006BE RID: 1726
        private float glowDelta; 
        public enum GlowType
        {
            // Token: 0x040006C4 RID: 1732
            None,
            // Token: 0x040006C5 RID: 1733
            Shield,
            // Token: 0x040006C6 RID: 1734
            Counter,
            // Token: 0x040006C7 RID: 1735
            PsiSheild,
            // Token: 0x040006C8 RID: 1736
            PsiCounter,
            // Token: 0x040006C9 RID: 1737
            Eraser
        }
		public Vector2f Position
		{
			get
			{
				return this.position;
			}
		}

		public Graphic CardGraphic
		{
			get
			{
				return this.card;
			}
		}

        private void UpdateGlow()
        {
            if (this.glowType != BattleCard.GlowType.None)
            {
                this.glowDelta += this.glowSpeed;
                float amount = (float)Math.Sin((double)this.glowDelta) / 2f + 0.5f;
                this.card.Color = ColorHelper.Blend(Color.Blue, this.glowColor, amount);
				//Console.WriteLine($"color is {card.Color}");
            }
        }
		public void Death()
		{
            this.card.Visible = false;
            deadCard.Visible = true;
			odoHP.SetValue(-1);
			this.hitsparks = new Graphic[16];
			for (int j = 0; j < this.hitsparks.Length; j++)
			{
				this.hitsparks[j] = new IndexedColorGraphic(Paths.GRAPHICS + "hitsparks.dat", "combohitspark", new Vector2f(-320f, -180f), 2003);
				this.hitsparks[j].Visible = false;
				pipeline.Add(this.hitsparks[j]);
			}
		}

		public void Pop()
		{
			
			Vector2f vector2f = (this.card.Position - this.card.Origin + new Vector2f((float)this.card.TextureRect.Width / 2f, (float)this.card.TextureRect.Height / 6f)) - (new Vector2f((float)this.card.TextureRect.Width * 0.6f, 2f)) / 2f + new Vector2f((float)((int)(Engine.Random.NextDouble() * (double)(new Vector2f((float)this.card.TextureRect.Width * 0.6f, 2f)).X)), (float)((int)(Engine.Random.NextDouble() * (double)(new Vector2f((float)this.card.TextureRect.Width * 0.6f, 2f)).Y)));

			// this.enemyGraphic.Position - this.enemyGraphic.Origin + new Vector2f((float)this.enemyGraphic.TextureRect.Width / 2f, (float)this.enemyGraphic.TextureRect.Height / 6f);
			Vector2f vector2f2 = new Vector2f(vector2f.X, this.card.Position.Y - this.card.Origin.Y + (float)((int)(Engine.Random.NextDouble() * (double)this.card.TextureRect.Height)));

			this.hitsparks[this.hitsparkIndex].Position = vector2f2;
			this.hitsparks[this.hitsparkIndex].Visible = true;
			this.hitsparks[this.hitsparkIndex].Frame = 0f;
			this.hitsparks[this.hitsparkIndex].OnAnimationComplete += this.HitsparkAnimationComplete;
			this.hitsparkIndex = (this.hitsparkIndex + 1) % this.hitsparks.Length;
			//Console.WriteLine(vector2f2);

			//uhohsound.Play();
		}
		public int hitsparkIndex;
		private void HitsparkAnimationComplete(AnimatedRenderable graphic)
		{
			graphic.Visible = false;
		}

		private Graphic[] hitsparks;

		public CardBar index;

		public RenderPipeline pipeline;
		private CarbineSound uhohsound;

		public BattleCard(RenderPipeline pipeline, Vector2f position, int depth, string name, int hp, int maxHp, int pp, int maxPp, float meterFill, CharacterType type, CardBar index)
		{
			this.pipeline = pipeline;


			this.position = position;
            this.index = index;
            this.card = new IndexedColorGraphic(BattleCard.BATTLEUI_DAT, "altcard", position, depth);
            this.deadCard = new IndexedColorGraphic(BattleCard.BATTLEUI_DAT, "carddead", position, depth);
			this.card.CurrentPalette = Settings.WindowFlavor;
			this.hpLabel = new IndexedColorGraphic(BattleCard.BATTLEUI_DAT, "hp", position + BattleCard.HPLABEL_POSITION, depth + 2);
			this.hpLabel.CurrentPalette = Settings.WindowFlavor;
			this.ppLabel = new IndexedColorGraphic(BattleCard.BATTLEUI_DAT, "pp", position + BattleCard.PPLABEL_POSITION, depth + 2);
			this.ppLabel.CurrentPalette = Settings.WindowFlavor;
			this.nameTag = new TextRegion(position, depth + 2, Fonts.Main, /* "tom"); */ name);
			this.nameTag.Color = Color.Black;
			this.nametagX = (int)((float)(this.card.TextureRect.Width / 2) - this.nameTag.Size.X / 2f);
			this.nameTag.Position = position + new Vector2f((float)this.nametagX, 4f) + BattleCard.NAME_POSITION;
			this.uhohsound = AudioManager.Instance.Use(Paths.SFXBATTLE + "smaaash.wav", AudioType.Sound);


			pipeline.Add(this.card);
            pipeline.Add(this.deadCard);
			pipeline.Add(this.hpLabel);
			pipeline.Add(this.ppLabel);
			pipeline.Add(this.nameTag);

			clock = new Clock();
            cType = type;

			deadCard.Visible = false;
			this.meter = new BattleMeter(pipeline, position + BattleCard.METER_OFFSET, meterFill, depth + 1);
			this.odoHP = new Odometer(pipeline, position + BattleCard.HPODO_POSITION, depth + 2, 3, hp, maxHp, this);
            if (PsiManager.Instance.CharacterHasPsi(type) == false)
            {
                ppLabel.Visible = false;
            }
            else
            {
                this.odoPP = new Odometer(pipeline, position + BattleCard.PPODO_POSITION, depth + 2, 3, pp, maxPp, this);

            }
			this.springMode = BattleCard.SpringMode.Normal;
			
			//Death();
		}

		public CharacterType cType;

		~BattleCard()
		{
			this.Dispose(false);
		}

        public void Mortis()
        {
            index.Mortis(this);
        }

        public void SetHP(int newHP)
		{
			this.odoHP.SetValue(newHP);
		}

        public void SetPP(int newPP)
        {
            if (odoPP != null)
            {
                this.odoPP.SetValue(newPP);
            }
        }

        public void SetMeter(float newFill)
		{
			this.meter.SetFill(newFill);
		}

		public void SetGroovy(bool groovy)
		{
			this.meter.SetGroovy(groovy);
		}

		public void SetTargetY(float newTargetY)
		{
			this.SetTargetY(newTargetY, false);
		}

		public void SetTargetY(float newTargetY, bool instant)
		{
			this.targetY = newTargetY;
			if (instant)
			{
				this.position.Y = this.targetY;
			}
		}

		public void SetSpring(BattleCard.SpringMode mode, Vector2f amplitude, Vector2f speed, Vector2f decay)
		{
			this.springMode = mode;
			this.xSpring = 0f;
			this.xDampTarget = amplitude.X;
			this.xSpeedTarget = speed.X;
			this.xDecayTarget = decay.X;
			this.ySpring = 0f;
			this.yDampTarget = amplitude.Y;
			this.ySpeedTarget = speed.Y;
			this.yDecayTarget = decay.Y;
			this.ramping = true;
		}

		public void AddSpring(Vector2f amplitude, Vector2f speed, Vector2f decay)
		{
			this.xDampTarget += amplitude.X;
			this.xSpeedTarget += speed.X;
			this.xDecayTarget += decay.X;
			this.yDampTarget += amplitude.Y;
			this.ySpeedTarget += speed.Y;
			this.yDecayTarget += decay.Y;
			this.ramping = true;
		}

		private void UpdateSpring()
		{
			if (this.ramping)
			{
				this.xDamp += (this.xDampTarget - this.xDamp) / 2f;
				this.xSpeed += (this.xSpeedTarget - this.xSpeed) / 2f;
				this.xDecay += (this.xDecayTarget - this.xDecay) / 2f;
				this.yDamp += (this.yDampTarget - this.yDamp) / 2f;
				this.ySpeed += (this.ySpeedTarget - this.ySpeed) / 2f;
				this.yDecay += (this.yDecayTarget - this.yDecay) / 2f;
				if ((int)this.xDamp == (int)this.xDampTarget && (int)this.xSpeed == (int)this.xSpeedTarget && (int)this.xDecay == (int)this.xDecayTarget && (int)this.yDamp == (int)this.yDampTarget && (int)this.ySpeed == (int)this.ySpeedTarget && (int)this.yDecay == (int)this.yDecayTarget)
				{
					this.ramping = false;
				}
			}
			else
			{
				this.xDamp = ((this.xDamp > 0.5f) ? (this.xDamp * this.xDecay) : 0f);
				this.yDamp = ((this.yDamp > 0.5f) ? (this.yDamp * this.yDecay) : 0f);
			}
			this.xSpring += this.xSpeed;
			this.ySpring += this.ySpeed;
			this.offset.X = (float)Math.Sin((double)this.xSpring) * this.xDamp;
			this.offset.Y = (float)Math.Sin((double)this.ySpring) * this.yDamp;
			if (this.springMode == BattleCard.SpringMode.BounceUp)
			{
				this.offset.Y = -Math.Abs(this.offset.Y);
				return;
			}
			if (this.springMode == BattleCard.SpringMode.BounceDown)
			{
				this.offset.Y = Math.Abs(this.offset.Y);
			}
		}

		private void UpdatePosition()
		{
			if (this.position.Y < this.targetY - 0.5f)
			{
				this.position.Y = this.position.Y + (this.targetY - this.position.Y) / 2f;
				return;
			}
			if (this.position.Y > this.targetY + 0.5f)
			{
				this.position.Y = this.position.Y + (this.targetY - this.position.Y) / 2f;
				return;
			}
			if ((int)this.position.Y != (int)this.targetY)
			{
				this.position.Y = this.targetY;
			}
		}

        private void MoveGraphics(Vector2f gPosition)
        {
            gPosition.X = (float) ((int) gPosition.X);
            gPosition.Y = (float) ((int) gPosition.Y);
            this.card.Position = gPosition;
            deadCard.Position = gPosition;
            this.hpLabel.Position = gPosition + BattleCard.HPLABEL_POSITION;
            this.ppLabel.Position = gPosition + BattleCard.PPLABEL_POSITION;
            this.nameTag.Position = gPosition + new Vector2f((float) this.nametagX, 6f) + BattleCard.NAME_POSITION;
            this.meter.Position = gPosition + BattleCard.METER_OFFSET;
            this.odoHP.Position = gPosition + BattleCard.HPODO_POSITION;
            if (odoPP != null)
            {
                this.odoPP.Position = gPosition + BattleCard.PPODO_POSITION;
            }
        }

		public Clock clock;

        public void Update()
		{
            this.UpdateGlow(); 
            this.UpdateSpring();
			this.UpdatePosition();
			this.MoveGraphics(this.position + this.offset);
			this.odoHP.Update();
			
			//int a = TimerManager.Instance.StartTimer(2);

			if (clock.ElapsedTime.AsSeconds() > 0.3f && deadCard.Visible) {
				Pop();
				clock.Restart();
			}

			if (odoPP != null)
            {
                this.odoPP.Update();
            }

            this.meter.Update();
		}

        

        public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		public void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					this.card.Dispose();
					this.hpLabel.Dispose();
					this.ppLabel.Dispose();
					this.odoHP.Dispose();
                    if (odoPP != null)
					{
						this.odoPP.Dispose();

					}

					this.meter.Dispose();
					this.nameTag.Dispose();
				}
				this.disposed = true;
			}
		}

		private const float DAMP_HIGHPASS = 0.5f;

		private static readonly string BATTLEUI_DAT = Paths.GRAPHICS + "battleui2.dat";


		// these determine where the odometer and the label for the PP bar is.
		// keep the y the same.
		private static readonly Vector2f PPLABEL_POSITION = new Vector2f(10f, 43f);
		private static readonly Vector2f PPODO_POSITION = new Vector2f(28f, 43f);

		// these determine where the odometer and the label for the HP bar is.
		// keep the y the same.
		private static readonly Vector2f HPODO_POSITION = new Vector2f(28f, 29f);
		private static readonly Vector2f HPLABEL_POSITION = new Vector2f(10f, 29f);



		private static readonly Vector2f METER_OFFSET = new Vector2f(-0, 0f);

		private static readonly Vector2f NAME_POSITION = new Vector2f(0f, 9f);

		private bool disposed;

        private IndexedColorGraphic card;
        private IndexedColorGraphic deadCard;

		private IndexedColorGraphic hpLabel;

		private IndexedColorGraphic ppLabel;

		private TextRegion nameTag;

		private int nametagX;

		private BattleMeter meter;

		private Odometer odoHP;

		private Odometer odoPP;

		private BattleCard.SpringMode springMode;

		private Vector2f position;

		private Vector2f offset;

		private float xSpring;

		private float ySpring;

		private float xSpeed;

		private float xSpeedTarget;

		private float ySpeed;

		private float ySpeedTarget;

		private float xDamp;

		private float xDampTarget;

		private float yDamp;

		private float yDampTarget;

		private float xDecay;

		private float xDecayTarget;

		private float yDecay;

		private float yDecayTarget;

		private bool ramping;

		private float targetY;

		public enum SpringMode
		{
			Normal,
			BounceUp,
			BounceDown
		}
	}
}
