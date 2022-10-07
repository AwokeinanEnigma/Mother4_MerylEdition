using System;
using Mother4.Data;

namespace Mother4.Battle.Combos
{
	internal class ComboController : IDisposable
	{
		public ComboSet ComboSet
		{
			get
			{
				return this.combos;
			}
		}

		public ComboController(ComboSet combos, CharacterType[] party)
		{
			this.combos = combos;
			this.currentNode = combos.GetFirstCombo();
		}

		~ComboController()
		{
			this.Dispose(false);
		}

		public bool IsCombo(uint time)
		{
			if (this.currentNode.Timestamp + this.currentNode.Duration + 225U < time - 225U)
			{
				this.currentNode = this.combos.GetNextCombo(this.currentNode);
			}
			bool result = false;
			switch (this.currentNode.Type)
			{
			case ComboType.BPMRange:
				result = this.IsBPMCombo(time);
				break;
			case ComboType.Point:
				result = this.IsPointCombo(time);
				break;
			}
			return result;
		}

		private bool IsPointCombo(uint time)
		{
			return time >= this.currentNode.Timestamp - 225U && time < this.currentNode.Timestamp + this.currentNode.Duration + 225U;
		}

		private bool IsBPMCombo(uint time)
		{
			bool flag = time >= this.currentNode.Timestamp - 225U && time < this.currentNode.Timestamp + this.currentNode.Duration + 225U;
			uint num = (uint)(60000f / this.currentNode.BPM.Value);
			uint num2 = time - this.currentNode.Timestamp;
			uint num3 = num2 / num;
			uint num4 = this.currentNode.Timestamp + num * num3;
			bool flag2 = time >= num4 - 225U && time < num4 + 225U;
			bool result = false;
			if (flag && flag2)
			{
				result = true;
			}
			return result;
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
				this.disposed = true;
			}
		}

		public const uint MS_PER_MIN = 60000U;

		public const uint TOLERANCE = 225U;

		protected bool disposed;

		private ComboSet combos;

		private ComboNode currentNode;
	}
}
