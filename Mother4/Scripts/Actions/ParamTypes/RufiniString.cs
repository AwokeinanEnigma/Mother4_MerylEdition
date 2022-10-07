using System;

namespace Mother4.Scripts.Actions.ParamTypes
{
	internal struct RufiniString
	{
		public string QualifiedName
		{
			get
			{
				return string.Join('.'.ToString(), this.nameParts);
			}
		}

		public string[] Names
		{
			get
			{
				return this.nameParts;
			}
		}

		public string Value
		{
			get
			{
				return this.value;
			}
		}

		public RufiniString(string qualifiedName, string value)
		{
			this.nameParts = qualifiedName.Split(new char[]
			{
				'.'
			});
			this.value = value;
		}

		public RufiniString(string[] nameParts, string value)
		{
			this.nameParts = new string[nameParts.Length];
			Array.Copy(nameParts, this.nameParts, nameParts.Length);
			this.value = value;
		}

		public override string ToString()
		{
			string text;
			if (this.Value != null)
			{
				text = (this.value ?? string.Empty).Replace("\n", "");
				if (text.Length > 50)
				{
					int val = text.Substring(0, 50).LastIndexOf(' ');
					int length = Math.Max(50, val);
					text = text.Substring(0, length) + "…";
				}
			}
			else
			{
				text = this.QualifiedName;
			}
			return text;
		}

		public const char SEPARATOR = '.';

		private const int MAX_LENGTH = 50;

		private const string TRAIL = "…";

		private string[] nameParts;

		private string value;
	}
}
