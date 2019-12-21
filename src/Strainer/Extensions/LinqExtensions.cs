using Fluorite.Strainer.Models.Sorting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fluorite.Extensions
{
    /// <summary>
    /// Provides extension methods for LINQ methods.
    /// </summary>
    public static class LinqExtentions
    {
        /// <summary>
        /// Sorts the elements of a sequence in an order according to
        /// <see cref="ISortExpression{TEntity}"/>.
        /// </summary>
        /// <typeparam name="TEntity">
        /// The type of entity stored in a sequence.
        /// </typeparam>
        /// <param name="source">
        /// The source sequence of elements.
        /// </param>
        /// <param name="sortExpression">
        /// The sort expression providing information about sorting order and key.
        /// </param>
        /// <returns>
        /// An instance of ordered queryable sequence.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sortExpression"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> is <see langword="null"/>.
        /// </exception>
        public static IOrderedQueryable<TEntity> OrderWithSortExpression<TEntity>(
            this IQueryable<TEntity> source,
            ISortExpression<TEntity> sortExpression)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (sortExpression == null)
            {
                throw new ArgumentNullException(nameof(sortExpression));
            }

            if (sortExpression.IsDescending)
            {
                if (sortExpression.IsSubsequent)
                {
                    return (source as IOrderedQueryable<TEntity>).ThenByDescending(sortExpression.Expression);
                }
                else
                {
                    return source.OrderByDescending(sortExpression.Expression);
                }
            }
            else
            {
                if (sortExpression.IsSubsequent)
                {
                    return (source as IOrderedQueryable<TEntity>).ThenBy(sortExpression.Expression);
                }
                else
                {
                    return source.OrderBy(sortExpression.Expression);
                }
            }
        }

        public static TDictionary MergeLeft<TDictionary, TKey, TValue>(this TDictionary source, params IDictionary<TKey, TValue>[] others)
            where TDictionary : IDictionary<TKey, TValue>, new()
        {
            var resultDictionary = new TDictionary();

            foreach (var dictionary in new List<IDictionary<TKey, TValue>> { source }.Concat(others))
            {
                foreach (var pair in dictionary)
                {
                    resultDictionary[pair.Key] = pair.Value;
                }
            }

            return resultDictionary;
        }
    }
}
