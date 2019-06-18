using Fluorite.Strainer.Models.Filtering.Operators;
using Fluorite.Strainer.Models.Filtering.Terms;
using System.Linq;

namespace Fluorite.Strainer.Models.Filtering
{
    public class CustomFilterMethodContext<TEntity> : ICustomFilterMethodContext<TEntity>
    {
        public CustomFilterMethodContext()
        {

        }

        /// <summary>
        /// Gets the filter operator.
        /// </summary>
        public IFilterOperator Operator => Term.Operator;

        public IQueryable<TEntity> Source { get; set; }

        public IFilterTerm Term { get; set; }
    }
}
