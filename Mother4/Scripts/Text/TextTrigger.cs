using System;

namespace Mother4.Scripts.Text
{
	internal class TextTrigger : ITextCommand
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

		public int Type
		{
			get
			{
				return this.type;
			}
		}

		public string[] Data
		{
			get
			{
				return this.data;
			}
		}

		public TextTrigger(int position, int type, string[] data)
		{
			this.position = position;
			this.type = type;
			this.data = data;
		}

		private int position;

		private int type;

		private string[] data;
	}
}
