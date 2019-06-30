using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Sorting;
using Fluorite.Strainer.Models.Sorting.Terms;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

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
    public class SortExpressionProvider : ISortExpressionProvider
    {
        private readonly IStrainerPropertyMapper _mapper;
        private readonly IStrainerPropertyMetadataProvider _metadataProvider;

        /// <summary>
        /// Initializes new instance of <see cref="SortExpressionProvider"/> class.
        /// </summary>
        public SortExpressionProvider(IStrainerPropertyMapper mapper, IStrainerPropertyMetadataProvider metadataProvider)
        {
            _mapper = mapper;
            _metadataProvider = metadataProvider;
        }

        public ISortExpression<TEntity> GetExpression<TEntity>(PropertyInfo propertyInfo, ISortTerm sortTerm, bool isFirst)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }

            if (sortTerm == null)
            {
                throw new ArgumentNullException(nameof(sortTerm));
            }

            var metadata = GetPropertyMetadata<TEntity>(
                isSortingRequired: true,
                isFilteringRequired: false,
                name: sortTerm.Name);

            if (metadata != null)
            {
                var parameter = Expression.Parameter(typeof(TEntity), "p");
                Expression propertyValue = parameter;

                if (metadata.Name.Contains("."))
                {
                    var parts = metadata.Name.Split('.');
                    for (var i = 0; i < parts.Length - 1; i++)
                    {
                        propertyValue = Expression.PropertyOrField(propertyValue, parts[i]);
                    }
                }

                var propertyAccess = Expression.MakeMemberAccess(propertyValue, propertyInfo);
                var conversion = Expression.Convert(propertyAccess, typeof(object));
                var orderExpression = Expression.Lambda<Func<TEntity, object>>(conversion, parameter);

                return new SortExpression<TEntity>
                {
                    Expression = orderExpression,
                    IsDescending = sortTerm.IsDescending,
                    IsSubsequent = !isFirst,
                };
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets a list of <see cref="ISortExpression{TEntity}"/> from
        /// list of sort terms used to associate names from <see cref="IStrainerPropertyMapper"/>.
        /// </summary>
        /// <typeparam name="TEntity">
        /// The type of entity for which the expression is for.
        /// </typeparam>
        /// <param name="sortTerms">
        /// A list of property info paired with sort terms.
        /// </param>
        /// <returns>
        /// A list of <see cref="ISortExpression{TEntity}"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sortTerms"/> is <see langword="null"/>.
        /// </exception>
        public IList<ISortExpression<TEntity>> GetExpressions<TEntity>(IEnumerable<KeyValuePair<PropertyInfo, ISortTerm>> sortTerms)
        {
            if (sortTerms == null)
            {
                throw new ArgumentNullException(nameof(sortTerms));
            }

            var expressions = new List<ISortExpression<TEntity>>();
            var isFirst = true;
            foreach (var pair in sortTerms)
            {
                var sortExpression = GetExpression<TEntity>(pair.Key, pair.Value, isFirst);
                if (sortExpression != null)
                {
                    expressions.Add(sortExpression);
                }
                else
                {
                    continue;
                }

                isFirst = false;
            }

            return expressions;
        }

        private IStrainerPropertyMetadata GetPropertyMetadata<TEntity>(bool isSortingRequired, bool isFilteringRequired, string name)
        {
            var metadata = _mapper.FindProperty<TEntity>(
                isSortingRequired,
                isFilteringRequired,
                name);

            if (metadata == null)
            {
                return _metadataProvider.GetMetadataFromAttribute<TEntity>(isSortingRequired, isFilteringRequired, name);
            }

            return metadata;
        }
    }
}
