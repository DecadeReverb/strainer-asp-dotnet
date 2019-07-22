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
    public class CustomFilterMethodContext<TEntity> : ICustomFilterMethodContext<TEntity>
    {
        /// <summary>
        /// Initializes new instance of <see cref="CustomFilterMethodContext{TEntity}"/>
        /// class.
        /// </summary>
        public CustomFilterMethodContext()
        {

        }

        /// <summary>
        /// Gets the filter operator.
        /// </summary>
        public IFilterOperator Operator => Term.Operator;

        /// <summary>
        /// Gets or sets the source collection.
        /// </summary>
        public IQueryable<TEntity> Source { get; set; }

        /// <summary>
        /// Gets or sets the filter term.
        /// </summary>
        public IFilterTerm Term { get; set; }
    }
}
