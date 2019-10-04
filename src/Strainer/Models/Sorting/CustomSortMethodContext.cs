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
    public class CustomSortMethodContext<TEntity> : ICustomSortMethodContext<TEntity>
    {
        /// <summary>
        /// Initializes new instance of <see cref="CustomSortMethodContext{TEntity}"/>
        /// class.
        /// </summary>
        public CustomSortMethodContext()
        {

        }

        /// <summary>
        /// Gets or sets a <see cref="bool"/> value indictating whether current sorting
        /// direction is descending.
        /// </summary>
        public bool IsDescending { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="bool"/> value indictating whether current
        /// context for sorting is a subsequent one - not a first one.
        /// <para/>
        /// If <see langword="true"/>, then a subsequent ordering calls
        /// should be executed on source collection using ThenBy()
        /// or ThenByDescending().
        /// </summary>
        public bool IsSubsequent { get; set; }

        /// <summary>
        /// Gets or sets the source collection.
        /// </summary>
        public IQueryable<TEntity> Source { get; set; }

        /// <summary>
        /// Gets or sets the sort term.
        /// </summary>
        public ISortTerm Term { get; set; }
    }
}
