using System;

namespace Mother4.Items
{
	internal class InvalidPropertyType : ApplicationException
	{
		public InvalidPropertyType(Type expectedType, Type valueType) : base(string.Format("Expected {0}, but found {1}.", expectedType.Name, valueType.Name))
		{
		}
	}
}
