using Fluorite.Strainer.Exceptions;
using Fluorite.Strainer.Models.Sorting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fluorite.Strainer.Services.Sorting
{
    public class SortExpressionValidator : ISortExpressionValidator
    {
        public SortExpressionValidator()
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
                var exceptionMessage =
                    $"No default sort expression found for type {typeof(TEntity)}.\n" +
                    $"Mark a property as default sorting to enable fallbacking " +
                    $"to it when no sorting information is provided.";

                throw new StrainerSortExpressionValidatorException(
                    typeof(TEntity),
                    exceptionMessage);
            }

            if (sortExpressions.Count(s => s.IsDefault) > 1)
            {
                var defaultProperties = string.Join(
                    Environment.NewLine,
                    sortExpressions.Where(s => s.IsDefault)
                        .Select(s => s.Expression.ToString()));
                var exceptionMessage =
                    $"Too many default sort expression found for type {typeof(TEntity)}.\n" +
                    $"Only one property can be marked as default.\n" +
                    $"Default properties:\n" +
                    $"{defaultProperties}";

                throw new StrainerSortExpressionValidatorException(
                    typeof(TEntity),
                    exceptionMessage);
            }
        }
    }
}
