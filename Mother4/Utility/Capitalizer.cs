using System;
using System.Globalization;

namespace Mother4.Utility
{
	internal class Capitalizer
	{
		public static string Capitalize(string word)
		{
			if (word != null && word.Length > 0)
			{
				string str = word.Substring(0, 1).ToUpper(CultureInfo.CurrentCulture);
				string str2 = word.Substring(1);
				return str + str2;
			}
			return string.Empty;
		}
	}
}
