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
                    && !t.IsAbstract)
                .Select(t => Activator.CreateInstance(t))
                .OfType<IFilterOperator>()
                .ToList();
        }

        public IReadOnlyList<IFilterOperator> GetOperators()
        {
            return _operators.AsReadOnly();
        }
    }
}
