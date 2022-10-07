using System;

namespace Mother4.Items
{
	internal class ItemPropertyNotFoundException : ApplicationException
	{
		public ItemPropertyNotFoundException(string name) : base(string.Format("\"{0}\" was not found in the item's properties.", name))
		{
		}
	}
}
