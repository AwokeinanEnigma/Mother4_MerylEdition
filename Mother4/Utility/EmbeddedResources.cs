using System;
using System.IO;
using System.Reflection;

namespace Mother4.Utility
{
	internal static class EmbeddedResources
	{
		public static Stream GetStream(string resource)
		{
			if (EmbeddedResources.assembly == null)
			{
				EmbeddedResources.assembly = Assembly.GetExecutingAssembly();
			}
			return EmbeddedResources.assembly.GetManifestResourceStream(resource);
		}

		private static Assembly assembly;
	}
}
