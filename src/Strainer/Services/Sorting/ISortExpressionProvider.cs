using Fluorite.Strainer.Models.Sorting;
using Fluorite.Strainer.Models.Sorting.Terms;
using Fluorite.Strainer.Services.Metadata;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Fluorite.Strainer.Services.Sorting
{
    /// <summary>
    /// Provides means of tranlating <see cref="ISortTerm"/> into
    /// <see cref="Expression{TDelegate}"/> of <see cref="Func{T, TResult}"/>.
    /// <para/>
    /// In other words - provides list of expressions which later can be used
    /// as arguments for ordering <see cref="IQueryable{T}"/>.
    /// </summary>
    public interface ISortExpressionProvider
    {
        ISortExpression<TEntity> GetDefaultExpression<TEntity>();

        ISortExpression<TEntity> GetExpression<TEntity>(
            PropertyInfo propertyInfo,
            ISortTerm sortTerm,
            bool isSubsequent);

        /// <summary>
        /// Gets a list of <see cref="ISortExpression{TEntity}"/> based on
        /// list of sort terms connected with property metadata from
        /// <see cref="IPropertyMetadataProvider"/>s.
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
        IReadOnlyCollection<ISortExpression<TEntity>> GetExpressions<TEntity>(
            IEnumerable<KeyValuePair<PropertyInfo, ISortTerm>> sortTerms);
    }
}
