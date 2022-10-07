using System;
using System.Collections.Generic;
using Carbine;
using Carbine.Graphics;
using Mother4.Battle.Combatants;
using Mother4.Data;
using SFML.System;

namespace Mother4.Battle.UI
{
	internal class ComboAnimator : IDisposable
	{
		public event ComboAnimator.AnimationCompleteHandler OnAnimationComplete;

		public Vector2f Position
		{
			get
			{
				return this.position;
			}
		}

		public int Depth
		{
			get
			{
				return this.depth;
			}
		}

		public bool Stopped
		{
			get
			{
				return this.state == ComboAnimator.State.Stopped;
			}
		}

		public ComboAnimator(RenderPipeline pipeline, int depth)
		{
			this.pipeline = pipeline;
			this.depth = depth;
			this.position = default(Vector2f);
			this.size = default(Vector2f);
			this.starGraphics = new IndexedColorGraphic[16];
			this.bounceFlag = new bool[16];
			this.starVelocity = new Vector2f[16];
			Vector2f vector2f = new Vector2f(-320f, -180f);

			for (int i = 0; i < this.starGraphics.Length; i++)
			{
				this.starGraphics[i] = new IndexedColorGraphic(HITSPARK_RESOURCE, "star", vector2f, depth);
				this.starGraphics[i].Visible = false;
				pipeline.Add(this.starGraphics[i]);
				this.starVelocity[i] = new Vector2f(0f, 0f);
			}
			this.damageNumbers = new List<DamageNumber>();
			this.damageNumbersToRemove = new List<DamageNumber>();
			this.totalDamageNumber = new TotalDamageNumber(pipeline, this.position, 0);
			this.totalDamageNumber.SetVisibility(false);
			this.hitsparks = new Graphic[2];
			for (int j = 0; j < this.hitsparks.Length; j++)
			{
				this.hitsparks[j] = new IndexedColorGraphic(HITSPARK_RESOURCE, "combohitspark", vector2f, depth + 20);
				this.hitsparks[j].Visible = false;
				pipeline.Add(this.hitsparks[j]);
			}
			this.state = ComboAnimator.State.Stopped;
		}

		~ComboAnimator()
		{
			this.Dispose(false);
		}

		public void Setup(Graphic graphic, PlayerCombatant playa)
		{
			string hitSrung = "travis";
			switch (playa.Character)
			{
				case CharacterType.Travis:
					//do nothing
					break;
				case CharacterType.Floyd:
					hitSrung = "";
					break;
				case CharacterType.Meryl:
					hitSrung = "meryl";
					break;
				case CharacterType.Leo:
					hitSrung = "leo";
					break;

			}
			Console.Write(playa.Character);
			string load = Paths.GRAPHICS + $"{hitSrung}hitsparks.dat";

            Console.WriteLine($"Enemy depth {graphic.Depth}");

			Vector2f vector2f = new Vector2f(-320f, -180f);
			this.starGraphics = new IndexedColorGraphic[16];
			for (int i = 0; i < this.starGraphics.Length; i++)
			{
				this.starGraphics[i] = new IndexedColorGraphic(load, "star", vector2f, depth);
				this.starGraphics[i].Visible = false;
				pipeline.Add(this.starGraphics[i]);
				this.starVelocity[i] = new Vector2f(0f, 0f);
			}

			this.hitsparks = new Graphic[2];
			for (int j = 0; j < this.hitsparks.Length; j++)
			{
				this.hitsparks[j] = new IndexedColorGraphic(load, "combohitspark", vector2f, depth + 20);
				this.hitsparks[j].Visible = false;
				pipeline.Add(this.hitsparks[j]);
			}

			this.wobble = 0f;
			this.wobbleSpeed = 0f;
			this.wobbleDamp = 0f;
			this.enemyGraphic = graphic;
			this.initialEnemyPosition = graphic.Position;
			this.bounce = 0f;
			this.bounceSpeed = 0f;
			this.position = this.enemyGraphic.Position - this.enemyGraphic.Origin + new Vector2f((float)this.enemyGraphic.TextureRect.Width / 2f, (float)this.enemyGraphic.TextureRect.Height / 6f);
			this.position.X = (float)((int)this.position.X);
			this.position.Y = (float)((int)this.position.Y);
			this.size = new Vector2f((float)this.enemyGraphic.TextureRect.Width * 0.6f, 2f);
			this.depth = this.enemyGraphic.Depth;
			this.starCount = 0;
			this.rotAngle = 0f;
			this.modAngle = 0f;
			for (int i = 0; i < this.starGraphics.Length; i++)
			{
				this.starGraphics[i].Visible = false;
				this.bounceFlag[i] = false;
			}
			Vector2f v = new Vector2f(0f, (float)this.enemyGraphic.TextureRect.Height * 0.9f);
			this.totalDamageNumber.Reset(this.position + v, 0);
			this.state = ComboAnimator.State.Circling;
		}

		public void Stop(bool explode)
		{
			this.state = ComboAnimator.State.Falling;
			float x = explode ? 7.5f : 100f;
			for (int i = 0; i < this.starGraphics.Length; i++)
			{
				this.starVelocity[i] = (this.starGraphics[i].Position - this.position) / x;
			}
			this.totalDamageNumber.Start();
		}

		private void End()
		{
			this.state = ComboAnimator.State.Stopped;
			this.enemyGraphic.Rotation = 0f;
			this.enemyGraphic = null;
			for (int i = 0; i < this.starGraphics.Length; i++)
			{
				this.starGraphics[i].Position = new Vector2f(-320f, -180f);
				this.starGraphics[i].Visible = false;
			}
			if (this.OnAnimationComplete != null)
			{
				this.OnAnimationComplete(this.starCount);
			}
			this.starCount = 0;
		}

		public void AddHit(int damage, bool smash)
		{
			this.AddStar();
			this.wobbleSpeed = (smash ? 1.2f : 0.7f);
			this.wobbleDamp = (smash ? 4f : 2f) * 3.14159f;
			if (smash)
			{
				this.bounceSpeed = -4f;
			}
			Vector2f vector2f = this.position - this.size / 2f + new Vector2f((float)((int)(Engine.Random.NextDouble() * (double)this.size.X)), (float)((int)(Engine.Random.NextDouble() * (double)this.size.Y)));
			DamageNumber damageNumber = new DamageNumber(this.pipeline, vector2f, new Vector2f(0f, -20f), 50, damage);
			damageNumber.SetVisibility(true);
			damageNumber.OnComplete += this.DamageNumberComplete;
			this.damageNumbers.Add(damageNumber);
			damageNumber.Start();
			this.totalDamageNumber.AddToNumber(damage);
			this.totalDamageNumber.SetVisibility(true);
			Vector2f vector2f2 = new Vector2f(vector2f.X, this.enemyGraphic.Position.Y - this.enemyGraphic.Origin.Y + (float)((int)(Engine.Random.NextDouble() * (double)this.enemyGraphic.TextureRect.Height)));
			this.hitsparks[this.hitsparkIndex].Position = vector2f2;
			this.hitsparks[this.hitsparkIndex].Visible = true;
			this.hitsparks[this.hitsparkIndex].Frame = 0f;
			this.hitsparks[this.hitsparkIndex].OnAnimationComplete += this.HitsparkAnimationComplete;
			this.hitsparkIndex = (this.hitsparkIndex + 1) % this.hitsparks.Length;
		}

		private void HitsparkAnimationComplete(AnimatedRenderable graphic)
		{
			graphic.Visible = false;
		}

		private void DamageNumberComplete(DamageNumber sender)
		{
			this.damageNumbersToRemove.Add(sender);
		}

		private void AddStar()
		{
			if (this.starCount < 16)
			{
				this.starGraphics[this.starCount].Visible = true;
				this.starCount++;
				this.rotAngle -= (float)(6.283185307179586 / (double)this.starCount);
				this.modAngle -= (float)(6.283185307179586 / (double)this.starCount) / 2f;
			}
		}

		public void Update()
		{
			if (this.state == ComboAnimator.State.Circling)
			{
				this.rotAngle += 0.07f;
				this.modAngle += 0.035f;
				for (int i = 0; i < this.starCount; i++)
				{
					float num = (float)(6.283185307179586 / (double)this.starCount * (double)i);
					int num2 = (int)(Math.Cos((double)(this.rotAngle + num)) * (double)this.size.X);
					int num3 = (int)(Math.Sin((double)(this.rotAngle + num)) * (double)this.size.Y + Math.Sin((double)(this.modAngle + num)) * 10.0);
					Vector2f vector2f = this.position + new Vector2f((float)num2, (float)(-(float)num3));
					this.starGraphics[i].Position = vector2f;
					this.starGraphics[i].Depth = this.depth - (int)(Math.Sin((double)(this.rotAngle + num)) * (double)((float)(this.starCount + 1))) + 1;
                    Console.WriteLine($"Star depth {starGraphics[i].Depth}, Enemy Depth is '{enemyGraphic.Depth}'");
				}
			}
			else if (this.state == ComboAnimator.State.Falling)
			{
				bool flag = true;
				for (int j = 0; j < this.starCount; j++)
				{
					if (this.starGraphics[j].Position.Y > 135f && !this.bounceFlag[j])
					{
						this.starVelocity[j].Y = -(this.starVelocity[j].Y * 0.45f);
						this.bounceFlag[j] = true;
					}
					Vector2f[] array = this.starVelocity;
					int num4 = j;
					array[num4].X = array[num4].X * 0.98f;
					Vector2f[] array2 = this.starVelocity;
					int num5 = j;
					array2[num5].Y = array2[num5].Y + 0.1f;
					this.starGraphics[j].Position += this.starVelocity[j];
					bool flag2 = this.starGraphics[j].Position.X < (float)this.starGraphics[j].TextureRect.Width || this.starGraphics[j].Position.Y < (float)this.starGraphics[j].TextureRect.Height || this.starGraphics[j].Position.X > (float)(320L + (long)this.starGraphics[j].TextureRect.Width) || this.starGraphics[j].Position.Y > (float)(180L + (long)this.starGraphics[j].TextureRect.Height);
					flag = (flag && flag2);
				}
				if (flag && this.totalDamageNumber.Done)
				{
					this.End();
				}
			}
			if (this.enemyGraphic != null && this.wobbleSpeed > 0f)
			{
				this.wobble += this.wobbleSpeed;
				this.wobbleDamp = ((this.wobbleDamp > 0.05f) ? (this.wobbleDamp * 0.9f) : 0f);
				this.enemyGraphic.Rotation = (float)(Math.Sin((double)this.wobble) * (double)this.wobbleDamp);
			}
			if (this.enemyGraphic != null)
			{
				this.bounce += this.bounceSpeed;
				if (this.initialEnemyPosition.Y + this.bounce < this.initialEnemyPosition.Y)
				{
					this.bounceSpeed += 0.5f;
					this.enemyGraphic.Position = this.initialEnemyPosition + new Vector2f(0f, this.bounce);
				}
				else
				{
					this.bounceSpeed = -this.bounceSpeed * 0.6f;
					this.enemyGraphic.Position = this.initialEnemyPosition;
				}
			}
			foreach (DamageNumber damageNumber in this.damageNumbers)
			{
				damageNumber.Update();
			}
			foreach (DamageNumber item in this.damageNumbersToRemove)
			{
				this.damageNumbers.Remove(item);
			}
			this.damageNumbersToRemove.Clear();
			this.totalDamageNumber.Update();
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed && disposing)
			{
				for (int i = 0; i < this.starGraphics.Length; i++)
				{
					if (this.starGraphics[i] != null)
					{
						this.starGraphics[i].Dispose();
					}
				}
			}
			this.disposed = true;
		}

		private const int MAX_STARS = 16;

		private const int MAX_DAMAGE_NUMBERS = 3;

		private const float ROT_SPEED = 0.07f;

		private const float MODULATION_SPEED = 0.035f;

		private const float GRAVITY = 0.1f;

		private const int GROUND_Y = 135;

		private const float EXPLODE_SUCCESS_FACTOR = 7.5f;

		private const float EXPLODE_FAIL_FACTOR = 100f;

		private string HITSPARK_RESOURCE = Paths.GRAPHICS + "hitsparks.dat";

		private bool disposed;

		private Vector2f position;

		private Vector2f size;

		private int depth;

		private RenderPipeline pipeline;

		private IndexedColorGraphic[] starGraphics;

		private ComboAnimator.State state;

		private int starCount;

		private float rotAngle;

		private float modAngle;

		private bool[] bounceFlag;

		private Vector2f[] starVelocity;

		private List<DamageNumber> damageNumbers;

		private List<DamageNumber> damageNumbersToRemove;

		private TotalDamageNumber totalDamageNumber;

		private Graphic[] hitsparks;

		private int hitsparkIndex;

		private Graphic enemyGraphic;

		private Vector2f initialEnemyPosition;

		private float wobble;

		private float wobbleSpeed;

		private float wobbleDamp;

		private float bounce;

		private float bounceSpeed;

		private enum State
		{
			Circling,
			Falling,
			Stopped
		}

		public delegate void AnimationCompleteHandler(int starCount);
	}
}