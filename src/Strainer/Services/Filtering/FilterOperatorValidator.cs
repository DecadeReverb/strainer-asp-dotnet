using System;
using System.Collections.Generic;
using System.Linq;
using Fluorite.Strainer.Models;

namespace Fluorite.Strainer.Services.Filtering
{
    public class FilterOperatorValidator : IFilterOperatorValidator
    {
        public FilterOperatorValidator()
        {

        }

        public void Validate(IFilterOperator filterOperator)
        {
            if (filterOperator == null)
            {
                throw new ArgumentNullException(nameof(filterOperator));
            }

            if (string.IsNullOrWhiteSpace(filterOperator.Name))
            {
                throw new InvalidOperationException(
                    $"{nameof(IFilterOperator.Name)}] of filter operator " +
                    $"cannot be null, empty or contain only whitespace characaters.");
            }

            if (string.IsNullOrWhiteSpace(filterOperator.Operator))
            {
                throw new InvalidOperationException(
                    $"{nameof(IFilterOperator.Operator)}] of filter operator " +
                    $"cannot be null, empty or contain only whitespace characaters.");
            }
        }

        public void Validate(IEnumerable<IFilterOperator> filterOperators)
        {
            if (filterOperators == null)
            {
                throw new ArgumentNullException(nameof(filterOperators));
            }

            foreach (var @operator in filterOperators)
            {
                Validate(@operator);
            }

            var nameDuplicate = filterOperators
                .GroupBy(f => f.Name)
                .FirstOrDefault(f => f.Count() > 1)
                ?.FirstOrDefault()?.Name;
            if (nameDuplicate != null)
            {
                throw new InvalidOperationException(
                    $"Filter operator with name {nameDuplicate} occurs more then ");
            }

            if (filterOperators.GroupBy(f => f.Name).Any(f => f.Count() > 1))
            {

            }
        }
    }
}
