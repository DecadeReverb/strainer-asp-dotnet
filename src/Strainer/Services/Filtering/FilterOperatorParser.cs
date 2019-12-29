using Fluorite.Strainer.Models.Filtering.Operators;
using System;

namespace Fluorite.Strainer.Services.Filtering
{
    public class FilterOperatorParser : IFilterOperatorParser
    {
        private readonly IFilterOperatorDictionary _filterOperators;

        public FilterOperatorParser(IFilterOperatorDictionary filterOperators)
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
