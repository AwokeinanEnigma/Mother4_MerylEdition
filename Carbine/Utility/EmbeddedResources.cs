using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Carbine.Utility
{
    public static class EmbeddedResources
    {
        public static Stream GetStream(string resource)
        {
            if (EmbeddedResources.assembly == null)
            {
                EmbeddedResources.assembly = Assembly.GetExecutingAssembly();
            }
            if (EmbeddedResources.streamDict == null)
            {
                EmbeddedResources.streamDict = new Dictionary<string, Stream>();
            }
            if (!EmbeddedResources.streamDict.TryGetValue(resource, out Stream manifestResourceStream))
            {
                manifestResourceStream = EmbeddedResources.assembly.GetManifestResourceStream(resource);
            }
            return manifestResourceStream;
        }

        private static Assembly assembly;

        private static IDictionary<string, Stream> streamDict;
    }
}
