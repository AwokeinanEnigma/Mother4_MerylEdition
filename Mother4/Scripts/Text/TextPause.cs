using System;

namespace Mother4.Scripts.Text
{
	internal class TextPause : ITextCommand
	{
		public int Position
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

		public int Duration
		{
			get
			{
				return this.duration;
			}
		}

		public TextPause(int position, int duration)
		{
			this.position = position;
			this.duration = duration;
		}

		private int position;

		private int duration;
	}
}
