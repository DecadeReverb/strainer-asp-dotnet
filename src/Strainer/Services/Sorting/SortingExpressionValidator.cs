using Fluorite.Strainer.Exceptions;
using Fluorite.Strainer.Models.Sorting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fluorite.Strainer.Services.Sorting
{
    public class SortingExpressionValidator : ISortingExpressionValidator
    {
        public SortingExpressionValidator()
        {

        }

        public void Validate<TEntity>(IEnumerable<ISortExpression<TEntity>> sortExpressions)
        {
            if (sortExpressions == null)
            {
                throw new ArgumentNullException(nameof(sortExpressions));
            }

            if (!sortExpressions.Any())
            {
                return;
            }

            if (!sortExpressions.Any(s => s.IsDefault))
            {
                throw new StrainerSortExpressionValidatorException(
                    typeof(TEntity),
                    $"No default sort expression found for type {typeof(TEntity)}.\n" +
                    $"Mark a property as default sorting to enable fallbacking " +
                    $"to it when no sorting information is provided.");
            }

            if (sortExpressions.Count(s => s.IsDefault) > 1){
                var defaultProperties = string.Join(
                    Environment.NewLine,
                    sortExpressions.Where(s => s.IsDefault)
                        .Select(s => s.Expression.ToString()));
                throw new StrainerSortExpressionValidatorException(
                    typeof(TEntity),
                    $"Too many default sort expression found for type {typeof(TEntity)}.\n" +
                    $"Only one property can be marked as default.\n" +
                    $"Default properties:\n" +
                    $"{defaultProperties}");
            }
        }
    }
}
