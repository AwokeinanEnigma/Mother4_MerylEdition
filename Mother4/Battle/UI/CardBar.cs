using System;
using Carbine.Actors;
using Carbine.Graphics;
using Mother4.Data;
using SFML.System;

namespace Mother4.Battle.UI
{
	internal class CardBar : Actor
	{
		public int SelectedIndex
		{
			get
			{
				return this.selIndex;
			}
			set
			{
				this.lastSelIndex = this.selIndex;
				this.selIndex = Math.Max(-1, Math.Min(this.cards.Length - 1, value));
			}
		}
        public void SetGlow(int index, BattleCard.GlowType type)
        {
            this.cards[index].Glow = type;
        }

        public BattleInterfaceController controller;

		public CardBar(RenderPipeline pipeline, CharacterType[] party, BattleInterfaceController controller)
		{
			int num = party.Length;
            this.controller = controller;
			this.leftMargin = 160 - 63 * num / 2;
			this.idleY = (int)ViewManager.Instance.FinalTopLeft.Y + 136;
			this.cards = new BattleCard[num];
			for (int i = 0; i < this.cards.Length; i++)
			{
				Vector2f position = ViewManager.Instance.FinalTopLeft + new Vector2f((float)(this.leftMargin + 63 * i), (float)this.idleY);
				StatSet stats = CharacterStats.GetStats(party[i]);
				this.cards[i] = new BattleCard(pipeline, position, 2000, CharacterNames.GetName(party[i]), stats.HP, stats.MaxHP, stats.PP, stats.MaxPP, stats.Meter, party[i], this);
				this.PopCard(i, 0);
			}
			this.selIndex = -1;
		}

        public void Mortis(BattleCard card)
        {
            controller.KillCharacter(card);

        }

        public void PopCard(int index, int height)
		{
			this.cards[index].SetTargetY((float)(this.idleY - height));
		}

		public void SetHP(int index, int newHP)
		{
			this.cards[index].SetHP(newHP);
		}

		public void SetPP(int index, int newPP)
		{
			this.cards[index].SetPP(newPP);
		}

        public void KillCard(int index)
        {
            this.cards[index].Death();
        }

        public void SetMeter(int index, float newFill)
		{
			this.cards[index].SetMeter(newFill);
		}

		public Vector2f GetCardTopMiddle(int index)
		{
			return this.cards[index].Position + new Vector2f(31f, 0f);
		}

		public Graphic GetCardGraphic(int index)
		{
			return this.cards[index].CardGraphic;
		}

		public void SetGroovy(int index, bool groovy)
		{
			this.cards[index].SetGroovy(groovy);
		}

		public void SetSpring(int index, BattleCard.SpringMode mode, Vector2f amplitude, Vector2f speed, Vector2f decay)
		{
			this.cards[index].SetSpring(mode, amplitude, speed, decay);
		}

		public void AddSpring(int index, Vector2f amplitude, Vector2f speed, Vector2f decay)
		{
			this.cards[index].AddSpring(amplitude, speed, decay);
		}

		private void SetTargetY(float y, bool instant)
		{
			for (int i = 0; i < this.cards.Length; i++)
			{
				this.cards[i].SetTargetY(y, instant);
				if (instant)
				{
					this.cards[i].Update();
				}
			}
		}

		public void Show()
		{
			this.Show(false);
		}

		public void Show(bool instant)
		{
			this.idleY = (int)ViewManager.Instance.FinalTopLeft.Y + 136;
			this.SetTargetY((float)this.idleY, instant);
		}

		public void Hide()
		{
			this.Hide(false);
		}

		public void Hide(bool instant)
		{
			this.SetTargetY(ViewManager.Instance.FinalTopLeft.Y + 180f, instant);
		}

        public override void Update()
		{
			base.Update();
			for (int i = 0; i < this.cards.Length; i++)
			{
				this.cards[i].Update();
			}
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			for (int i = 0; i < this.cards.Length; i++)
			{
				this.cards[i].Dispose();
			}
		}

		private const int SPACING = 63;

		private const int IDLE_Y_OFFSET = 136;

		private BattleCard[] cards;

		private readonly int leftMargin;

		private int idleY;

		private int selIndex;

		private int lastSelIndex;
	}
}
