using Fluorite.Strainer.Models.Sorting.Terms;
using System.Linq;

namespace Fluorite.Strainer.Models.Sorting
{
    /// <summary>
    /// Represents information context for custom sort method.
    /// </summary>
    /// <typeparam name="TEntity">
    /// The type of entity being processed.
    /// </typeparam>
    public interface ICustomSortMethodContext<TEntity>
    {
        /// <summary>
        /// Gets a <see cref="bool"/> value indictating whether current sorting
        /// direction is descending.
        /// </summary>
        bool IsDescending { get; }

        /// <summary>
        /// Gets a <see cref="bool"/> value indictating whether current context
        /// for sorting is a subsequent one - not a first one.
        /// <para/>
        /// If <see langword="true"/>, then a subsequent ordering calls
        /// should be executed on source collection using ThenBy()
        /// or ThenByDescending().
        /// </summary>
        bool IsSubsequent { get; }

        /// <summary>
        /// Gets the ordered source collection for subsequent calls.
        /// <para/>
        /// Use this for subsequent calls when <see cref="IsSubsequent"/>
        /// is <see langword="true"/>.
        /// </summary>
        IOrderedQueryable<TEntity> OrderedSource { get; }

        /// <summary>
        /// Gets the source collection.
        /// </summary>
        IQueryable<TEntity> Source { get; }

        /// <summary>
        /// Gets the sort term.
        /// </summary>
        ISortTerm Term { get; }
    }
}
