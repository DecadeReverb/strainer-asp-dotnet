using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Filtering.Operators;

namespace Fluorite.Strainer.Services.Filtering
{
    public class FilterOperatorProvider : IFilterOperatorProvider
    {
        private readonly List<IFilterOperator> _operators;

        public FilterOperatorProvider()
        {
            _operators = new List<IFilterOperator>
            {
                new EqualsOperator(),
                new NotEqualsOperator(),
                new LessThanOperator(),
                new LessThanOrEqualToOperator(),
                new GreaterThanOperator(),
                new GreaterThanOrEqualToOperator(),
                new ContainsOperator(),
                new StartsWithOperator(),
            };
        }

        public IFilterOperator GetDefaultOperator()
        {
            return _operators.FirstOrDefault(f => f.IsDefault);
        }

        public IEnumerator<IFilterOperator> GetEnumerator() => _operators.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _operators.GetEnumerator();
    }
}
