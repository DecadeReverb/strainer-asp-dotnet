using System.Collections;

namespace Fluorite.Strainer.Collections;

public interface IReadOnlySet<T> : IReadOnlyCollection<T>, IEnumerable<T>, IEnumerable
{
    /// <summary>
    /// Determines whether current hash set contains the specified element.
    /// </summary>
    /// <param name="item">
    /// The element to locate in the hash set.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the hash set contains the specified element;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    bool Contains(T item);

    /// <summary>
    /// Determines whether a hash set is a proper subset of the specified collection.
    /// </summary>
    /// <param name="other">
    /// The collection to compare to the current hash set.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the hash set is a proper subset of other; otherwise, <see langword="false"/>.
    /// </returns>
    bool IsProperSubsetOf(IEnumerable<T> other);

    /// <summary>
    /// Determines whether a hash set is a proper superset of the specified collection.
    /// </summary>
    /// <param name="other">
    /// The collection to compare to the current hash set.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the hash set is a proper superset of other; otherwise, <see langword="false"/>.
    /// </returns>
    bool IsProperSupersetOf(IEnumerable<T> other);

    /// <summary>
    /// Determines whether a hash set is a subset of the specified collection.
    /// </summary>
    /// <param name="other">
    /// The collection to compare to the current hash set.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the hash set is a subset of other; otherwise, <see langword="false"/>.
    /// </returns>
    bool IsSubsetOf(IEnumerable<T> other);

    /// <summary>
    /// Determines whether a hash set is a superset of the specified collection.
    /// </summary>
    /// <param name="other">
    /// The collection to compare to the current hash set.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the hash set is a superset of other; otherwise, <see langword="false"/>.
    /// </returns>
    bool IsSupersetOf(IEnumerable<T> other);

    /// <summary>
    /// Determines whether the current hash set and a specified collection share common elements.
    /// </summary>
    /// <param name="other">
    /// The collection to compare to the current hash set.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the hash set share at least one common element;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    bool Overlaps(IEnumerable<T> other);

    /// <summary>
    /// Determines whether a hash set and the specified collection contain the same elements.
    /// </summary>
    /// <param name="other">
    /// The collection to compare to the current hash set.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the hash set is equal to other; otherwise, <see langword="false"/>.
    /// </returns>
    bool SetEquals(IEnumerable<T> other);
}
