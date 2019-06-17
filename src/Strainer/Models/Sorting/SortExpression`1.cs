using System;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Models.Sorting
{
    /// <summary>
    /// Provides additional information about expression used for sorting
    /// and strongly typed expression over <see cref="Func{T, TResult}"/>.
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
