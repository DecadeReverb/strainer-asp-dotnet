using Fluorite.Strainer.Models.Filter.Operators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fluorite.Strainer.Services.Filter
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

            if (string.IsNullOrWhiteSpace(filterOperator.Symbol))
            {
                throw new InvalidOperationException(
                    $"{nameof(IFilterOperator.Symbol)} for filter operator " +
                    $"\"{filterOperator}\" cannot be null, empty or contain " +
                    $"only whitespace characters.");
            }

            if (filterOperator.Expression == null)
            {
                throw new InvalidOperationException(
                    $"{nameof(IFilterOperator.Expression)} for filter operator " +
                    $"\"{filterOperator}\" cannot be null.");
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

            var symbolDuplicate = filterOperators
                .GroupBy(f => f.Symbol)
                .FirstOrDefault(f => f.Count() > 1)
                ?.FirstOrDefault();
            if (symbolDuplicate != null)
            {
                throw new InvalidOperationException(
                    $"Symbol used in filter operator \"{symbolDuplicate}\" " +
                    $"occurs more then once.\n" +
                    $"Symbol for filter operator must be unique.\n" +
                    $"Please remove or change symbol for either of operators.");
            }
        }
    }
}
