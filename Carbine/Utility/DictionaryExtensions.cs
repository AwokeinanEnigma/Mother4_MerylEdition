using System.Collections.Generic;

namespace Carbine.Utility
{
    public static class DictionaryExtensions
    {
        public static void AddReplace<K, V>(this Dictionary<K, V> dictionary, K key, V value)
        {
            if (dictionary.ContainsKey(key))
            {
                dictionary[key] = value;
                return;
            }
            dictionary.Add(key, value);
        }
    }
}
