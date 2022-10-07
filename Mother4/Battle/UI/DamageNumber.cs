using System;
using Carbine.Graphics;
using Carbine.Utility;
using Mother4.Data;
using SFML.System;

namespace Mother4.Battle.UI
{
	internal class DamageNumber : IDisposable
	{
		public event DamageNumber.CompletionHandler OnComplete;

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

		public Vector2f Goal
		{
			get
			{
				return this.goal;
			}
			set
			{
				this.goal = value;
			}
		}

		public DamageNumber(RenderPipeline pipeline, Vector2f position, Vector2f offset, int hangTime, int number, string customNumberset = "")
		{
			this.pipeline = pipeline;
			this.position = position;
			this.goal = position + offset;
			this.hangTime = hangTime;
			this.number = number;
			this.Reset(this.position, number, customNumberset);
		}

		~DamageNumber()
		{
			this.Dispose(false);
		}

		public void AddToNumber(int add)
		{
			this.number += add;
			this.Reset(this.position, this.number);
		}

		public void Reset(Vector2f position, int number, string customNumberset = "")
		{
			this.position = position;
			this.number = number;

			string numberSet = DamageNumber.RESOURCE;
			if (customNumberset != string.Empty) {
				numberSet = customNumberset;
			}
			if (this.numbers != null && this.numbers.Length > 0)
			{
				for (int i = 0; i < this.numbers.Length; i++)
				{
					this.pipeline.Remove(this.numbers[i]);
					this.numbers[i].Dispose();
				}
			}
			int num = Digits.CountDigits(number);
			this.numbers = new Graphic[num];
			int num2 = 0;
			int num3 = 0;
			for (int j = 0; j < this.numbers.Length; j++)
			{
				this.numbers[j] = new IndexedColorGraphic(numberSet, "numbers", default(Vector2f), 32767);
				this.numbers[j].Frame = (float)Digits.Get(number, this.numbers.Length - j);
				this.numbers[j].Visible = false;
				num2 += this.numbers[j].TextureRect.Width + -1;
				num3 = Math.Max(num3, this.numbers[j].TextureRect.Height);
				this.pipeline.Add(this.numbers[j]);
			}
			num2 -= -1;
			int num4 = num2 / 2;
			int num5 = num3 / 2;
			for (int k = 0; k < this.numbers.Length; k++)
			{
				this.numbers[k].Position = this.position - new Vector2f((float)num4, (float)num5);
				num4 -= this.numbers[k].TextureRect.Width + -1;
			}
			this.state = DamageNumber.State.Moving;
			this.timer = 0;
			this.paused = true;
		}

		public void SetVisibility(bool visible)
		{
			for (int i = 0; i < this.numbers.Length; i++)
			{
				this.numbers[i].Visible = visible;
			}
		}

		public void Pause()
		{
			this.paused = true;
		}

		public void Start()
		{
			this.paused = false;
		}

		public virtual void Update()
		{
			if (!this.paused)
			{
				if (this.state == DamageNumber.State.Moving)
				{
					this.translation = new Vector2f(Math.Min(25f, this.goal.X - this.position.X), Math.Min(25f, this.goal.Y - this.position.Y));
					if (this.translation.X > 1f || this.translation.X < -1f || this.translation.Y > 1f || this.translation.Y < -1f)
					{
						this.position += this.translation * 0.25f;
						for (int i = 0; i < this.numbers.Length; i++)
						{
							this.numbers[i].Position += this.translation * 0.25f;
						}
						return;
					}
					this.state = DamageNumber.State.Waiting;
					return;
				}
				else if (this.state == DamageNumber.State.Waiting)
				{
					this.timer++;
					if (this.timer > this.hangTime)
					{
						this.state = DamageNumber.State.CleanUp;
						return;
					}
				}
				else if (this.state == DamageNumber.State.CleanUp)
				{
					for (int j = 0; j < this.numbers.Length; j++)
					{
						this.pipeline.Remove(this.numbers[j]);
					}
					if (this.OnComplete != null)
					{
						this.OnComplete(this);
					}
					this.state = DamageNumber.State.Done;
				}
			}
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
				for (int i = 0; i < this.numbers.Length; i++)
				{
					this.numbers[i].Dispose();
				}
			}
		}

		private const int PADDING = -1;

		private static readonly string RESOURCE = Paths.GRAPHICS + "numberset1.dat";

		private bool disposed;

		private int number;

		private int timer;

		private int hangTime;

		private Vector2f position;

		private Vector2f goal;

		private Vector2f translation;

		private Graphic[] numbers;

		private RenderPipeline pipeline;

		private DamageNumber.State state;

		private bool paused;

		private enum State
		{
			Moving,
			Waiting,
			CleanUp,
			Done
		}

		public delegate void CompletionHandler(DamageNumber sender);
	}
}
