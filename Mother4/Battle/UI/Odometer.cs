using System;
using System.Linq;
using FMOD;
using Carbine.Graphics;
using Carbine.Utility;
using Mother4.Data;
using SFML.System;

namespace Mother4.Battle.UI
{
	internal class Odometer : IDisposable
	{
		public Vector2f Position
		{
			get
			{
				return this.position;
			}
			set
			{
				this.position = value;
				this.posDirty = true;
			}
		}

		public int TargetValue
		{
			get
			{
				return this.rollers[0].Number;
			}
		}

		public Odometer(RenderPipeline pipeline, Vector2f position, int depth, int places, int initialValue, int maxValue, BattleCard outer)
        {
            card = outer;

			this.rollerContainer = new IndexedColorGraphic(Paths.GRAPHICS + "battleui2.dat", "odometer", position - new Vector2f(1f, 1f), depth - 1);
			pipeline.Add(this.rollerContainer);
			this.rollers = new OdometerRoller[places];
			this.hidden = new bool[this.rollers.Length];
			for (int i = 0; i < this.rollers.Length; i++)
			{
				int num = (int)Math.Pow(10.0, (double)i);
				int initialNumber = initialValue / num;
				this.rollers[i] = new OdometerRoller(pipeline, initialNumber, position + new Vector2f((float)(8 * (places - 1 - i)), 0f), depth);
                    //rollers[i].OnRollover += Counter;
				if (i > 0)
				{
                    this.rollers[i - 1].OnRollover += this.rollers[i].StepRoll;
                    this.rollers[i - 1].OnRollover += this.rollers[i].StepRoll;

				}
			}
			this.holdPlaces = Digits.CountDigits(maxValue) - ((maxValue == 0) ? 1 : 0);
			this.places = places;
			this.position = position;
			this.Update();
		}


        public int dead;
        private void Kill()
        {

                Console.WriteLine($"we're dead");
                card.Mortis();
            
		}

		private BattleCard card;

        ~Odometer()
		{
			this.Dispose(false);
		}

		public void Update()
		{
			for (int i = 0; i < this.rollers.Length; i++)
			{
				if (this.posDirty)
				{
					this.rollers[i].Position = this.Position + new Vector2f((float)(8 * (this.places - 1 - i)), 0f);
					this.rollerContainer.Position = this.Position - new Vector2f(1f, 1f);
				}
				this.rollers[i].Update();
				if (i >= this.holdPlaces && this.rollers[i].CurrentNumber == 0 && this.rollers[i].Number == 0 && (i + 1 >= this.rollers.Length || this.hidden[i + 1]))
				{
					if (!this.hidden[i])
					{
						this.hidden[i] = true;
						this.rollers[i].Hide();
					}
				}
				else if (this.hidden[i])
				{
					this.hidden[i] = false;
					this.rollers[i].Show();
				}
			}

            if (rollers.All(x => x.rolling == false && x.CurrentNumber <= 0) && killed == false)
            {
                killed = true;
				Kill();
            }

            //	Console.WriteLine($"is dead = {IsRolling}");
			this.posDirty = false;
		}

        private bool killed;

        public int num;

		public void SetValue(int newValue)
		{
			Console.Write("odometer setvalue: ");
			
			for (int i = 0; i < this.rollers.Length; i++)
			{
				int num = newValue / (int)Math.Pow(10.0, (double)i);
				this.rollers[i].Number = num;
				Console.Write("{0} ", num);
			}

            num = newValue;
			Console.WriteLine();
			int num2 = Digits.CountDigits(newValue) - ((newValue == 0) ? 1 : 0);
			if (num2 > this.holdPlaces)
			{
				this.holdPlaces = num2;
			}
			this.rollers[0].DoEntireRoll();
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
					this.rollerContainer.Dispose();
					foreach (OdometerRoller odometerRoller in this.rollers)
					{
						odometerRoller.Dispose();
					}
				}
				this.disposed = true;
			}
		}


        private bool disposed;

		private Graphic rollerContainer;

		private OdometerRoller[] rollers;

		private int places;

		private int holdPlaces;

		private Vector2f position;

		private bool posDirty;

		private bool[] hidden;
	}
}
