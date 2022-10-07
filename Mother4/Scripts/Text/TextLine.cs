using System;

namespace Mother4.Scripts.Text
{
	internal class TextLine
	{
		public string Text
		{
			get
			{
				return this.text;
			}
		}

		public bool HasBullet
		{
			get
			{
				return this.bullet;
			}
		}

		public ITextCommand[] Commands
		{
			get
			{
				return this.commands;
			}
		}

		public TextLine(bool bullet, ITextCommand[] commands, string text)
		{
			this.bullet = bullet;
			this.commands = commands;
			this.text = text;
		}

		private string text;

		private bool bullet;

		private ITextCommand[] commands;
	}
}
