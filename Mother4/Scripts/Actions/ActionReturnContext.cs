using System;
using System.Collections.Generic;

namespace Mother4.Scripts.Actions
{
	internal struct ActionReturnContext
	{
		public ScriptExecutor.WaitType Wait { get; set; }

		public Dictionary<string, object> Data { get; set; }
	}
}
