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
    public interface ISortExpression<TEntity> : ISortExpression
    {
        /// <summary>
        /// Gets an expression which can be used as a functor argument
        /// for ordering functions.
        /// </summary>
        Expression<Func<TEntity, object>> Expression { get; }
    }
}