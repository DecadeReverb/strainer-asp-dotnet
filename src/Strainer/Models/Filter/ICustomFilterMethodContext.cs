using Fluorite.Strainer.Models.Filter.Operators;
using Fluorite.Strainer.Models.Filtering.Terms;
using System.Linq;

namespace Fluorite.Strainer.Models.Filtering
{
    /// <summary>
    /// Represents information context for custom filter method.
    /// </summary>
    /// <typeparam name="TEntity">
    /// The type of entity being processed.
    /// </typeparam>
    public interface ICustomFilterMethodContext<TEntity>
    {
        /// <summary>
        /// Gets the filter operator.
        /// </summary>
        IFilterOperator Operator { get; }

        /// <summary>
        /// Gets the source collection.
        /// </summary>
        IQueryable<TEntity> Source { get; }

        /// <summary>
        /// Gets the filter term.
        /// </summary>
        IFilterTerm Term { get; }
    }
}
