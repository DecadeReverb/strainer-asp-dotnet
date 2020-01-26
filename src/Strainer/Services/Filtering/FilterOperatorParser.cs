using Fluorite.Strainer.Models.Filtering.Operators;
using System;
using System.Collections.Generic;

namespace Fluorite.Strainer.Services.Filtering
{
    public class FilterOperatorParser : IFilterOperatorParser
    {
        private readonly IReadOnlyDictionary<string, IFilterOperator> _filterOperators;

        public FilterOperatorParser(IReadOnlyDictionary<string, IFilterOperator> filterOperators)
        {
            _filterOperators = filterOperators ?? throw new ArgumentNullException(nameof(filterOperators));
        }

        public virtual IFilterOperator GetParsedOperator(string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                return null;
            }

            if (_filterOperators.TryGetValue(symbol, out var filterOperator))
            {
                return filterOperator;
            }

            return null;
        }
    }
}
