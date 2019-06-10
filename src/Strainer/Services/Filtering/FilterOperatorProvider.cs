using Fluorite.Strainer.Models.Filtering.Operators;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Fluorite.Strainer.Services.Filtering
{
    public class FilterOperatorProvider : IFilterOperatorProvider, IEnumerable<IFilterOperator>, IEnumerable
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
                new DoesNotContainOperator(),
                new StartsWithOperator(),
                new DoesNotStartWithOperator(),

                new EqualsCaseInsensitiveOperator(),
                new ContainsCaseInsensitiveOperator(),
                new DoesNotContainCaseInsensitiveOperator(),
                new StartsWithCaseInsensitiveOperator(),
                new DoesNotStartWithCaseInsensitiveOperator(),
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
