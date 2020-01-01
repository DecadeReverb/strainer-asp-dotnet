using Fluorite.Strainer.Models.Sorting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public static IDictionary<TKey, TValue> Merge<TKey, TValue>(
            this IEnumerable<KeyValuePair<TKey, TValue>> keyValuePairs)
        {
            if (keyValuePairs is null)
            {
                throw new ArgumentNullException(nameof(keyValuePairs));
            }

            var result = new Dictionary<TKey, TValue>();

            foreach (var pair in keyValuePairs)
            {
                result[pair.Key] = pair.Value;
            }

            return result;
        }

        public static TDictionary MergeLeft<TDictionary, TKey, TValue>(
            this TDictionary source,
            params IDictionary<TKey, TValue>[] others)
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

        public static IDictionary<TKey, TValue> ToDictionary<TKey, TValue>(
            this IEnumerable<KeyValuePair<TKey, TValue>> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        public static IReadOnlyDictionary<TKey, TValue> ToReadOnly<TKey, TValue>(
            this IDictionary<TKey, TValue> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return new ReadOnlyDictionary<TKey, TValue>(source);
        }

        public static IReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TKey, TValue>(
            this IEnumerable<KeyValuePair<TKey, TValue>> source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return new ReadOnlyDictionary<TKey, TValue>(source.ToDictionary());
        }
    }
}
