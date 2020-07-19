using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Fluorite.Strainer.Extensions
{
    public static class EnumerableExtensions
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

        public static IDictionary<TKey, TValue> ToDictionary<TKey, TValue>(
            this IEnumerable<KeyValuePair<TKey, TValue>> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.ToDictionary(pair => pair.Key, pair => pair.Value);
        }


        public static IReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TKey, TValue>(
            this IEnumerable<KeyValuePair<TKey, TValue>> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return new ReadOnlyDictionary<TKey, TValue>(source.ToDictionary());
        }
    }
}
