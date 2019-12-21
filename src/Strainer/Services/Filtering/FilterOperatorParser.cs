using Fluorite.Strainer.Models.Filtering.Operators;
using System;

namespace Fluorite.Strainer.Services.Filtering
{
    public class FilterOperatorParser : IFilterOperatorParser
    {
        public FilterOperatorParser(IFilterOperatorMapper mapper)
        {
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        protected IFilterOperatorMapper Mapper { get; }

        public virtual IFilterOperator GetParsedOperator(string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                return null;
            }

            return Mapper.Find(symbol);
        }
    }
}
