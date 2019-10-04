using System;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Models.Sorting
{
    /// <summary>
    /// Provides information about strongly typed expression used for sorting.
    /// </summary>
    /// <typeparam name="TEntity">
    /// The type of entity for which the expression is for.
    /// </typeparam>
    public class SortExpression<TEntity> : SortExpression, ISortExpression<TEntity>, ISortExpression
    {
        /// <summary>
        /// Initializes new instance of <see cref="SortExpression{TEntity}"/> class.
        /// </summary>
        public SortExpression()
        {

        }

        /// <summary>
        /// Gets or sets an expression which can be used as a functor argument
        /// for ordering functions.
        /// </summary>
        public Expression<Func<TEntity, object>> Expression { get; set; }
    }
}
