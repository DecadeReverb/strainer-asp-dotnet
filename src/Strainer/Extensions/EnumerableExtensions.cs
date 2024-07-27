using System.Collections.ObjectModel;

namespace Fluorite.Extensions;

public static class EnumerableExtensions
{
    public static IEnumerable<T> Concat<T>(
        this IEnumerable<T> source,
        params IEnumerable<T>[] sequences)
    {
        Guard.Against.Null(source);
        Guard.Against.Null(sequences);

        return Enumerable.Concat(source, sequences.SelectMany(x => x));
    }

    public static Dictionary<TKey, TValue> Merge<TKey, TValue>(
        this IEnumerable<KeyValuePair<TKey, TValue>> keyValuePairs)
    {
        Guard.Against.Null(keyValuePairs);

        var result = new Dictionary<TKey, TValue>();

        foreach (var pair in keyValuePairs)
        {
            result.Add(pair.Key, pair.Value);
        }

        return result;
    }

    public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(
        this IEnumerable<KeyValuePair<TKey, TValue>> source)
    {
        Guard.Against.Null(source);

        return source.ToDictionary(pair => pair.Key, pair => pair.Value);
    }

    public static ReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TKey, TValue>(
        this IEnumerable<KeyValuePair<TKey, TValue>> source)
    {
        Guard.Against.Null(source);

        return new ReadOnlyDictionary<TKey, TValue>(source.ToDictionary());
    }
}
