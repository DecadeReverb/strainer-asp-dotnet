using System.Collections;

namespace Fluorite.Strainer.Collections;

/// <summary>
/// Represents a read-only set of values.
/// </summary>
/// <typeparam name="T">
/// The type of elements in the hash set.
/// </typeparam>
public class ReadOnlyHashSet<T> : IReadOnlySet<T>, IReadOnlyCollection<T>, IEnumerable<T>, IEnumerable
{
    private readonly HashSet<T> _set;

    /// <summary>
    /// Initializes a new instance of the <see cref="ReadOnlyHashSet{T}"/> class
    /// with elements copied from specified collection.
    /// </summary>
    /// <param name="collection">
    /// The collection of values that will create base for this hash set.
    /// </param>
    public ReadOnlyHashSet(IEnumerable<T> collection)
    {
        _set = new HashSet<T>(Guard.Against.Null(collection));
    }

    /// <inheritdoc/>
    public int Count => _set.Count;

    /// <inheritdoc/>
    public bool Contains(T item) => _set.Contains(item);

    /// <inheritdoc/>
    public IEnumerator GetEnumerator() => _set.GetEnumerator();

    /// <inheritdoc/>
    public bool IsProperSubsetOf(IEnumerable<T> other) => _set.IsProperSubsetOf(other);

    /// <inheritdoc/>
    public bool IsProperSupersetOf(IEnumerable<T> other) => _set.IsProperSupersetOf(other);

    /// <inheritdoc/>
    public bool IsSubsetOf(IEnumerable<T> other) => _set.IsSubsetOf(other);

    /// <inheritdoc/>
    public bool IsSupersetOf(IEnumerable<T> other) => _set.IsSupersetOf(other);

    /// <inheritdoc/>
    public bool Overlaps(IEnumerable<T> other) => _set.Overlaps(other);

    /// <inheritdoc/>
    public bool SetEquals(IEnumerable<T> other) => _set.SetEquals(other);

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => _set.GetEnumerator();
}
