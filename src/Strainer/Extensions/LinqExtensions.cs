﻿using Fluorite.Strainer.Models.Sorting;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Fluorite.Strainer.Extensions
{
    public static partial class LinqExtentions
    {
        public static IQueryable<TEntity> OrderByDynamic<TEntity>(
            this IQueryable<TEntity> source,
            string fullPropertyName,
            PropertyInfo propertyInfo,
            bool desc,
            bool useThenBy)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (fullPropertyName == null)
            {
                throw new ArgumentNullException(nameof(fullPropertyName));
            }

            var command = desc ?
                ( useThenBy ? "ThenByDescending" : "OrderByDescending") :
                ( useThenBy ? "ThenBy" : "OrderBy");
            var type = typeof(TEntity);
            var parameter = Expression.Parameter(type, "p");

            Expression propertyValue = parameter;
            if (fullPropertyName.Contains("."))
            {
                var parts = fullPropertyName.Split('.');
                for (var i = 0; i < parts.Length - 1; i++)
                {
                    propertyValue = Expression.PropertyOrField(propertyValue, parts[i]);
                }
            }

            var propertyAccess = Expression.MakeMemberAccess(propertyValue, propertyInfo);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(
                typeof(Queryable),
                command,
                new Type[] { type, propertyInfo.PropertyType },
                source.Expression,
                Expression.Quote(orderByExpression));

            return source.Provider.CreateQuery<TEntity>(resultExpression);
        }

        private static IQueryable<TEntity> OrderBySortExpression<TEntity>(
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
                return source.OrderByDescending(sortExpression.Expression);
            }
            else
            {
                return source.OrderBy(sortExpression.Expression);
            }
        }

        private static IOrderedQueryable<TEntity> ThenBySortExpression<TEntity>(
            this IOrderedQueryable<TEntity> source,
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
                return source.ThenByDescending(sortExpression.Expression);
            }
            else
            {
                return source.ThenBy(sortExpression.Expression);
            }
        }
    }
}
