using System;
using System.Collections.Generic;

namespace Mother4.Scripts.Text
{
	internal class TextBlock
	{
		public List<TextLine> Lines
		{
			get
			{
				return this.lines;
			}
		}

		public TextBlock(List<TextLine> lines)
		{
			this.lines = new List<TextLine>(lines);
		}

		private List<TextLine> lines;
	}
}
