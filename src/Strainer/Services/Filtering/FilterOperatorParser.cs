using Fluorite.Strainer.Models.Filtering.Operators;
using System;
using System.Linq;

namespace Fluorite.Strainer.Services.Filtering
{
    public class FilterOperatorParser : IFilterOperatorParser
    {
        public FilterOperatorParser(IFilterOperatorMapper mapper)
        {
            Mapper = mapper;
        }

        protected IFilterOperatorMapper Mapper { get; }

        public virtual IFilterOperator GetParsedOperator(string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                return Mapper.GetDefault();
            }

            return Mapper.Find(symbol) ?? Mapper.GetDefault();
        }
    }
}
