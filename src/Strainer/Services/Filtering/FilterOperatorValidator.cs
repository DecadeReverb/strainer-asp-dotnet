using Fluorite.Strainer.Models.Filter.Operators;
using System;
using System.Collections.Generic;
using System.Linq;

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

            if (!filterOperators.Any())
            {
                return;
            }

            foreach (var @operator in filterOperators)
            {
                Validate(@operator);
            }

            var duplicatedSymbols = filterOperators
                .GroupBy(f => f.Symbol)
                .FirstOrDefault(g => g.Count() > 1);
            if (duplicatedSymbols != null)
            {
                throw new InvalidOperationException(
                    $"More then one filter operator found with the same" +
                    $"symbol: \"{duplicatedSymbols.Key}\". " +
                    $"Symbol representing filter operator must be unique. " +
                    $"Please remove or change symbol for either of operators. " +
                    $"Conflicting filter operators:\n" +
                    $"{string.Join(Environment.NewLine, duplicatedSymbols.Select(f => f.ToString()))}");
            }

            var defaultOperators = filterOperators.Where(f => f.IsDefault);
            if (defaultOperators.Count() > 1)
            {
                throw new InvalidOperationException(
                    $"Too many default filter operators found. " +
                    $"Only one filter operator can be a default one. " +
                    $"Please remove the default flag from filter operators, " +
                    $"so only one is default. " +
                    $"Conflicting filter operators:\n" +
                    $"{string.Join(Environment.NewLine, defaultOperators.Select(f => f.ToString()))}");
            }

            if (!defaultOperators.Any())
            {
                throw new InvalidOperationException(
                    "No default filter operator found. " +
                    "Please add a filter operator with default flag " +
                    "set to true.");
            }
        }
    }
}
