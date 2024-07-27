using System.Collections.ObjectModel;

namespace Fluorite.Extensions;

public static class DictionaryExtensions
{
    public static Dictionary<TKey, TValue> MergeLeft<TKey, TValue>(
        this IReadOnlyDictionary<TKey, TValue> source,
        params IReadOnlyDictionary<TKey, TValue>[] others)
    {
        Guard.Against.Null(source);
        Guard.Against.Null(others);

        var resultDictionary = new Dictionary<TKey, TValue>();

        foreach (var dictionary in new[] { source }.Concat(others))
        {
            foreach (var pair in dictionary)
            {
                resultDictionary[pair.Key] = pair.Value;
            }
        }

        return resultDictionary;
    }

    public static ReadOnlyDictionary<TKey, TValue> ToReadOnly<TKey, TValue>(
        this IDictionary<TKey, TValue> source)
    {
        Guard.Against.Null(source);

        return new ReadOnlyDictionary<TKey, TValue>(source);
    }
}
