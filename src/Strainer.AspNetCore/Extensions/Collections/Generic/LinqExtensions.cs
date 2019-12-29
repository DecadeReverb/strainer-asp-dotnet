using System;
using System.Collections.Generic;

namespace Fluorite.Extensions.Collections.Generic
{
    public static class LinqExtensions
    {
        public static IDictionary<TKey, TValue> Merge<TKey, TValue>(
            this IEnumerable<KeyValuePair<TKey, TValue>> keyValuePairs)
        {
            if (keyValuePairs is null)
            {
                throw new ArgumentNullException(nameof(keyValuePairs));
            }

            var result = new Dictionary<TKey, TValue>();

            foreach (var pair in keyValuePairs)
            {
                result[pair.Key] = pair.Value;
            }

            return result;
        }

        public static IDictionary<TKey, TValue> Merge<TKey, TValue>(
            this IEnumerable<IDictionary<TKey, TValue>> dictionaries)
        {
            if (dictionaries is null)
            {
                throw new ArgumentNullException(nameof(dictionaries));
            }

            var result = new Dictionary<TKey, TValue>();

            foreach (var currentDictionary in dictionaries)
            {
                foreach (var pair in currentDictionary)
                {
                    result[pair.Key] = pair.Value;
                }
            }

            return result;
        }
    }
}
