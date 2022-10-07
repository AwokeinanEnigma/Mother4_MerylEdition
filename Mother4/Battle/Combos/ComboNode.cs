using System;

namespace Mother4.Battle.Combos
{
	internal class ComboNode : IComparable
	{
		public ComboType Type
		{
			get
			{
				return this.type;
			}
		}

		public uint Timestamp
		{
			get
			{
				return this.timestamp;
			}
		}

		public uint Duration
		{
			get
			{
				return this.duration;
			}
		}

		public float? BPM
		{
			get
			{
				return this.bpm;
			}
		}

		public ComboNode(ComboType type, uint timestamp, uint duration)
		{
			this.Initialize(type, timestamp, duration, null);
		}

		public ComboNode(ComboType type, uint timestamp, uint duration, float bpm)
		{
			this.Initialize(type, timestamp, duration, new float?(bpm));
		}

		private void Initialize(ComboType type, uint timestamp, uint duration, float? bpm)
		{
			this.type = type;
			this.timestamp = timestamp;
			this.duration = duration;
			this.bpm = bpm;
		}

		public int CompareTo(object obj)
		{
			if (obj is ComboNode)
			{
				ComboNode comboNode = (ComboNode)obj;
				return (int)(comboNode.timestamp - this.timestamp);
			}
			throw new ArgumentException(string.Format("Cannot compare between ComboNode and {0}", obj.GetType().Name));
		}

		private ComboType type;

		private uint timestamp;

		private uint duration;

		private float? bpm;
	}
}
