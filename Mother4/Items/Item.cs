using System;
using System.Collections.Generic;

namespace Mother4.Items
{
	internal class Item
	{
		public bool Key
		{
			get
			{
				return this.isKey;
			}
		}

		public Item(bool isKey)
		{
			this.isKey = isKey;
			this.properties = new Dictionary<int, object>();
		}

		public void Set(string property, object value)
		{
			int hashCode = property.GetHashCode();
			if (this.properties.ContainsKey(hashCode))
			{
				this.properties[hashCode] = value;
				return;
			}
			this.properties.Add(hashCode, value);
		}

		public T Get<T>(string name)
		{
			int hashCode = name.GetHashCode();
			T result;
			try
			{
				T t = (T)((object)this.properties[hashCode]);
				result = t;
			}
			catch (KeyNotFoundException)
			{
				throw new ItemPropertyNotFoundException(name);
			}
			catch (InvalidCastException)
			{
				throw new InvalidPropertyType(typeof(T), this.properties[hashCode].GetType());
			}
			return result;
		}

		public const string NAME = "name";

		private bool isKey;

		private Dictionary<int, object> properties;
	}
}
