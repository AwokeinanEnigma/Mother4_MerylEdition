using System;
using Carbine.Graphics;
using Carbine.Utility;
using Mother4.Data;
using SFML.System;

namespace Mother4.Battle.UI
{
	internal class TotalDamageNumber : IDisposable
	{
		public event TotalDamageNumber.CompletionHandler OnComplete;

		public bool Done
		{
			get
			{
				return this.state == TotalDamageNumber.State.Finished;
			}
		}

		public TotalDamageNumber(RenderPipeline pipeline, Vector2f position, int number)
		{
			this.pipeline = pipeline;
			this.Reset(position, number);
			this.state = TotalDamageNumber.State.Waiting;
		}

		~TotalDamageNumber()
		{
			this.Dispose(false);
		}

		public void AddToNumber(int add)
		{
			this.number += add;
			this.Reset(this.position, this.number);
		}

		public void Reset(Vector2f position, int number)
		{
			this.position = position;
			this.position.Y = Math.Max(12f, Math.Min(115f, this.position.Y));
			this.translation = default(Vector2f);
			this.number = number;
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
				this.numbers[j] = new IndexedColorGraphic(TotalDamageNumber.YELLOW_RESOURCE, "numbers", default(Vector2f), 32767);
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
			if (this.state == TotalDamageNumber.State.Finished)
			{
				this.timer = 0;
				this.paused = true;
				this.state = TotalDamageNumber.State.Waiting;
			}
		}

		public void SetVisibility(bool visible)
		{
			for (int i = 0; i < this.numbers.Length; i++)
			{
				this.numbers[i].Visible = visible;
			}
		}

		public void Start()
		{
			this.paused = false;
			this.goal = this.position + TotalDamageNumber.UP_OFFSET;
			this.goal.Y = Math.Max(12f, Math.Min(115f, this.goal.Y));
			this.state = TotalDamageNumber.State.Rising;
		}

		public void Update()
		{
			if (!this.paused)
			{
				if (this.state == TotalDamageNumber.State.Rising)
				{
					if (this.position.Y > this.goal.Y)
					{
						this.translation.Y = this.translation.Y - 0.2f;
						this.UpdatePosition();
						return;
					}
					this.timer = 0;
					this.translation = default(Vector2f);
					this.state = TotalDamageNumber.State.Hanging;
					return;
				}
				else if (this.state == TotalDamageNumber.State.Hanging)
				{
					if (this.timer < 38)
					{
						this.timer++;
						return;
					}
					this.timer = 0;
					this.goal = this.position + TotalDamageNumber.RIGHT_OFFSET;
					this.state = TotalDamageNumber.State.CleanUp;
					return;
				}
				else if (this.state == TotalDamageNumber.State.Exiting)
				{
					if (this.position.X < this.goal.X)
					{
						this.translation.X = this.translation.X + 0.2f;
						this.UpdatePosition();
						return;
					}
					this.timer = 0;
					this.translation = default(Vector2f);
					this.state = TotalDamageNumber.State.CleanUp;
					return;
				}
				else if (this.state == TotalDamageNumber.State.CleanUp)
				{
					this.timer = 0;
					this.translation = default(Vector2f);
					this.state = TotalDamageNumber.State.Finished;
					for (int i = 0; i < this.numbers.Length; i++)
					{
						this.pipeline.Remove(this.numbers[i]);
					}
					if (this.OnComplete != null)
					{
						this.OnComplete(this);
					}
				}
			}
		}

		private void UpdatePosition()
		{
			this.position += this.translation;
			for (int i = 0; i < this.numbers.Length; i++)
			{
				this.numbers[i].Position = new Vector2f((float)((int)(this.numbers[i].Position.X + this.translation.X)), (float)((int)(this.numbers[i].Position.Y + this.translation.Y)));
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

		private const int RISE_HEIGHT = 24;

		private const int HANG_TIME = 38;

		private const float ACCELERATION = 0.2f;

		private const float TOP_Y_BOUND = 12f;

		private const float BOTTOM_Y_BOUND = 115f;

		private static readonly string YELLOW_RESOURCE = Paths.GRAPHICS + "numberset2.dat";

		private static readonly Vector2f UP_OFFSET = new Vector2f(0f, -32f);

		private static readonly Vector2f RIGHT_OFFSET = new Vector2f(320f, 0f);

		private bool disposed;

		private Vector2f position;

		private Vector2f goal;

		private Vector2f translation;

		private Graphic[] numbers;

		private RenderPipeline pipeline;

		private TotalDamageNumber.State state;

		private int timer;

		private int number;

		private bool paused;

		private enum State
		{
			Waiting,
			Rising,
			Hanging,
			Exiting,
			CleanUp,
			Finished
		}

		public delegate void CompletionHandler(TotalDamageNumber sender);
	}
}
