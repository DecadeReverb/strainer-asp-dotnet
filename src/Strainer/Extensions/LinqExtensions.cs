using Fluorite.Strainer.Models.Sorting;
using System;
using System.Linq;

namespace Fluorite.Extensions
{
    public static partial class LinqExtentions
    {
        public static IQueryable<TEntity> OrderWithSortExpression<TEntity>(
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
    }
}
