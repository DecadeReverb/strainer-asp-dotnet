using System.Collections.ObjectModel;

namespace Fluorite.Extensions
{
    public static class DictionaryExtensions
    {
        public static TDictionary MergeLeft<TDictionary, TKey, TValue>(
            this TDictionary source,
            params IDictionary<TKey, TValue>[] others)
            where TDictionary : IDictionary<TKey, TValue>, new()
        {
            var resultDictionary = new TDictionary();

            foreach (var dictionary in new List<IDictionary<TKey, TValue>> { source }.Concat(others))
            {
                foreach (var pair in dictionary)
                {
                    resultDictionary[pair.Key] = pair.Value;
                }
            }

            return resultDictionary;
        }

        public static IReadOnlyDictionary<TKey, TValue> ToReadOnly<TKey, TValue>(
            this IDictionary<TKey, TValue> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return new ReadOnlyDictionary<TKey, TValue>(source);
        }
    }
}
