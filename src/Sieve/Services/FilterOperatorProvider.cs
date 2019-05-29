using System;
using System.Collections.Generic;
using System.Linq;
using Sieve.Models;

namespace Sieve.Services
{
    public class FilterOperatorProvider : IFilterOperatorProvider
    {
        private readonly List<IFilterOperator> _operators;

        public FilterOperatorProvider()
        {
            // TODO:
            // Change this to external provider, or move this logic it at all
            // to filter operator provider. DTO should not handle creating
            // the operators, it should just have it ready from the start.
            _operators = (typeof(FilterTerm))
                .Assembly
                .DefinedTypes
                .Where(t =>
                    t.ImplementedInterfaces.Contains(typeof(IFilterOperator))
                    && !t.IsAbstract
                    && t != typeof(FilterOperator))
                .Select(t => Activator.CreateInstance(t))
                .OfType<IFilterOperator>()
                .ToList();
        }

        public void AddOperator(IFilterOperator @operator)
        {
            if (@operator == null)
            {
                throw new ArgumentNullException(nameof(@operator));
            }

            _operators.Add(@operator);
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

        public IReadOnlyList<IFilterOperator> GetOperators()
        {
            return _operators.AsReadOnly();
        }
    }
}
