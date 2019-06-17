using Fluorite.Strainer.Attributes;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Sorting;
using Fluorite.Strainer.Models.Sorting.Terms;
using Microsoft.Extensions.Options;
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
        private readonly StrainerOptions _options;

        /// <summary>
        /// Initializes new instance of <see cref="SortExpressionProvider"/> class.
        /// </summary>
        public SortExpressionProvider(IStrainerPropertyMapper mapper, IOptions<StrainerOptions> options)
        {
            _mapper = mapper;
            _options = options.Value;
        }

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
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sortTerms"/> is <see langword="null"/>.
        /// </exception>
        public IList<ISortExpression<TEntity>> GetExpressions<TEntity>(IList<ISortTerm> sortTerms)
        {
            if (sortTerms == null)
            {
                throw new ArgumentNullException(nameof(sortTerms));
            }

            var expressionInfoList = new List<ISortExpression<TEntity>>();
            var isSubsequent = false;
            foreach (var sortTerm in sortTerms)
            {
                var (fullName, propertyInfo) = GetStrainerProperty<TEntity>(
                    isSortingRequired: true,
                    ifFileringRequired: false,
                    name: sortTerm.Name);

                if (propertyInfo != null)
                {
                    var parameter = Expression.Parameter(typeof(TEntity), "p");
                    Expression propertyValue = parameter;

                    if (fullName.Contains("."))
                    {
                        var parts = fullName.Split('.');
                        for (var i = 0; i < parts.Length - 1; i++)
                        {
                            propertyValue = Expression.PropertyOrField(propertyValue, parts[i]);
                        }
                    }

                    var propertyAccess = Expression.MakeMemberAccess(propertyValue, propertyInfo);
                    var conversion = Expression.Convert(propertyAccess, typeof(object));
                    var orderExpression = Expression.Lambda<Func<TEntity, object>>(conversion, parameter);
                    var expressionInfo = new SortExpression<TEntity>
                    {
                        Expression = orderExpression,
                        IsDescending = sortTerm.IsDescending,
                        IsSubsequent = isSubsequent,
                    };

                    expressionInfoList.Add(expressionInfo);
                }
                else
                {
                    continue;
                }

                isSubsequent = true;
            }

            return expressionInfoList;
        }

        private (string, PropertyInfo) GetStrainerProperty<TEntity>(
            bool isSortingRequired,
            bool ifFileringRequired,
            string name)
        {
            var (fullName, propertyInfo) = _mapper.FindProperty<TEntity>(
                isSortingRequired,
                ifFileringRequired,
                name,
                _options.CaseSensitive);

            if (fullName == null || propertyInfo == null)
            {
                propertyInfo = FindPropertyByStrainerAttribute<TEntity>(
                    isSortingRequired,
                    ifFileringRequired,
                    name,
                    _options.CaseSensitive);

                return (propertyInfo?.Name, propertyInfo);
            }
            else
            {
                return (fullName, propertyInfo);
            }
        }

        private PropertyInfo FindPropertyByStrainerAttribute<TEntity>(
            bool isSortingRequired,
            bool isFilteringRequired,
            string name,
            bool isCaseSensitive)
        {
            var stringComparisonMethod = isCaseSensitive
                ? StringComparison.Ordinal
                : StringComparison.OrdinalIgnoreCase;
            var properties = typeof(TEntity).GetProperties();

            return Array.Find(properties, propertyInfo =>
            {
                var strainerAttribute = propertyInfo.GetCustomAttribute<StrainerAttribute>(inherit: true);

                return strainerAttribute != null
                    && (isSortingRequired ? strainerAttribute.IsSortable : true)
                    && (isFilteringRequired ? strainerAttribute.IsFilterable : true)
                    && ((strainerAttribute.DisplayName ?? propertyInfo.Name).Equals(name, stringComparisonMethod));
            });
        }
    }
}
