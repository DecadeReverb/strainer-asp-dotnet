using Fluorite.Strainer.Models.Filtering.Operators;
using Fluorite.Strainer.Models.Filtering.Terms;
using System.Linq;

namespace Fluorite.Strainer.Models.Filtering
{
    public interface ICustomFilterMethodContext<TEntity>
    {
        /// <summary>
        /// Gets the filter operator.
        /// </summary>
        IFilterOperator Operator { get; }

        IQueryable<TEntity> Source { get; }

        IFilterTerm Term { get; }
    }
}
