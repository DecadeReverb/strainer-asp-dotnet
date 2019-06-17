using Fluorite.Strainer.Models.Sorting;
using Fluorite.Strainer.Models.Sorting.Terms;
using System.Collections.Generic;

namespace Fluorite.Strainer.Services.Sorting
{
    /// <summary>
    /// Provides means of tranlating <see cref="ISortTerm"/> into
    /// <see cref="System.Linq.Expressions.Expression{TDelegate}"/> of
    /// <see cref="System.Func{T, TResult}"/>.
    /// <para/>
    /// In other words - provides list of expressions which later can be used
    /// as arguments for ordering <see cref="System.Linq.IQueryable{T}"/> functions.
    /// </summary>
    public interface ISortExpressionProvider
    {
        ISortExpression<TEntity> GetExpression<TEntity>(ISortTerm sortTerm, bool isFirst);

        /// <summary>
        /// Gets a list of <see cref="ISortExpression{TEntity}"/> from
        /// list of sort terms used to associate names from <see cref="IStrainerPropertyMapper"/>.
        /// </summary>
        /// <typeparam name="TEntity">
        /// The type of entity for which the expression is for.
        /// </typeparam>
        /// <param name="sortTerms">
        /// A list of sort terms.
        /// </param>
        /// <returns>
        /// A list of <see cref="ISortExpression{TEntity}"/>.
        /// </returns>
        IList<ISortExpression<TEntity>> GetExpressions<TEntity>(IList<ISortTerm> sortTerms);
    }
}
