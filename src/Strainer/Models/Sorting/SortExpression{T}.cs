using System.Linq.Expressions;

namespace Fluorite.Strainer.Models.Sorting
{
    /// <summary>
    /// Provides information about strongly typed expression used for sorting.
    /// </summary>
    /// <typeparam name="T">
    /// The type of entity for which the expression is for.
    /// </typeparam>
    public class SortExpression<T> : SortExpression, ISortExpression<T>, ISortExpression
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SortExpression{TEntity}"/> class.
        /// </summary>
        public SortExpression()
        {

        }

        /// <summary>
        /// Gets or sets an expression which can be used as a functor argument
        /// for ordering functions.
        /// </summary>
        public Expression<Func<T, object>> Expression { get; set; }
    }
}
