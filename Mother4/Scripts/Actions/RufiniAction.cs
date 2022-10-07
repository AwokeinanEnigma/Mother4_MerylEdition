using System;
using System.Collections.Generic;

namespace Mother4.Scripts.Actions
{
	internal abstract class RufiniAction
	{
		public List<ActionParam> Params
		{
			get
			{
				return this.paramList;
			}
		}

		public RufiniAction()
		{
			this.paramValues = new Dictionary<string, object>();
		}

		public abstract ActionReturnContext Execute(ExecutionContext context);

		public void SetValue<T>(string param, T value)
		{
			ActionParam actionParam = this.paramList.Find((ActionParam x) => x.Name == param);
			if (actionParam == null)
			{
				throw new KeyNotFoundException("The key \"" + param + "\" was not found in the parameter list");
			}
			if (!(actionParam.Type == typeof(T)))
			{
				throw new InvalidOperationException(string.Concat(new object[]
				{
					"Tried to set incorrect type for \"",
					param,
					"\", got ",
					typeof(T),
					" but expected ",
					actionParam.Type
				}));
			}
			if (this.paramValues.ContainsKey(param))
			{
				this.paramValues[param] = value;
				return;
			}
			this.paramValues.Add(param, value);
		}

		public T GetValue<T>(string param)
		{
			T result = default(T);
			if (this.paramValues.ContainsKey(param))
			{
				object obj = this.paramValues[param];
				if (obj is T)
				{
					result = (T)((object)obj);
				}
			}
			return result;
		}

		public bool TryGetValue<T>(string param, out T value)
		{
			bool result = false;
			value = default(T);
			if (this.paramValues.ContainsKey(param))
			{
				object obj = this.paramValues[param];
				if (obj is T)
				{
					result = true;
					value = (T)((object)obj);
				}
			}
			return result;
		}

		protected List<ActionParam> paramList;

		private Dictionary<string, object> paramValues;
	}
}
