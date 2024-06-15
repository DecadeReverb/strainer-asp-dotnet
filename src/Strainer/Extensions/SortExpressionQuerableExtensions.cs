using Fluorite.Strainer.Models.Sorting;

namespace Fluorite.Extensions;

public static class SortExpressionQuerableExtensions
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

        if (sortExpression.IsSubsequent)
        {
            var orderedSource = source as IOrderedQueryable<TEntity>;
            if (sortExpression.IsDescending)
            {
                return orderedSource.ThenByDescending(sortExpression.Expression);
            }
            else
            {
                return orderedSource.ThenBy(sortExpression.Expression);
            }
        }
        else
        {
            if (sortExpression.IsDescending)
            {
                return source.OrderByDescending(sortExpression.Expression);
            }
            else
            {
                return source.OrderBy(sortExpression.Expression);
            }
        }
    }
}
