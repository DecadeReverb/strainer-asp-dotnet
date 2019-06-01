using System;
using System.Collections.Generic;
using System.Linq;
using Strainer.Models;
using Strainer.Models.Filtering.Operators;

namespace Strainer.Services.Filtering
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

        public IReadOnlyList<IFilterOperator> Operators => _operators.AsReadOnly();

        public void AddOperator(IFilterOperator @operator)
        {
            if (@operator == null)
            {
                throw new ArgumentNullException(nameof(@operator));
            }

            // TODO:
            // Perform validation checks before adding new operator.

            _operators.Add(@operator);
        }

        public IFilterOperator GetDefaultOperator()
        {
            return _operators.FirstOrDefault(f => f.IsDefault);
        }

        public IFilterOperator GetFirstOrDefault(string @operator)
        {
            if (string.IsNullOrWhiteSpace(@operator))
            {
                throw new ArgumentException(
                    $"{nameof(@operator)} cannot be null, empty " +
                    $"or contain only whitespace characaters.",
                    nameof(@operator));
            }

            return _operators.FirstOrDefault(f => f.Operator == @operator);
        }
    }
}
