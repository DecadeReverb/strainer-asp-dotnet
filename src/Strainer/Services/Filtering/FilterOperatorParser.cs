using Fluorite.Strainer.Models.Filtering.Operators;
using System;
using System.Linq;

namespace Fluorite.Strainer.Services.Filtering
{
    public class FilterOperatorParser : IFilterOperatorParser
    {
        public FilterOperatorParser(IFilterOperatorProvider operatorProvider)
        {
            OperatorProvider = operatorProvider;
        }

        protected IFilterOperatorProvider OperatorProvider { get; }

        public virtual IFilterOperator GetParsedOperator(string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                return OperatorProvider.GetDefaultOperator();
            }

            return OperatorProvider.FirstOrDefault(op => op.Symbol.Equals(symbol, StringComparison.OrdinalIgnoreCase))
                ?? OperatorProvider.GetDefaultOperator();
        }
    }
}
