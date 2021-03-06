using System;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Models.Sorting
{
    /// <summary>
    /// Provides information about strongly typed expression used for sorting.
    /// </summary>
    /// <typeparam name="T">
    /// The type of entity for which the expression is for.
    /// </typeparam>
    public interface ISortExpression<T> : ISortExpression
    {
        /// <summary>
        /// Gets an expression which can be used as a functor argument
        /// for ordering functions.
        /// </summary>
        Expression<Func<T, object>> Expression { get; }
    }
}