using System;
using Sieve.Models;

namespace Sieve.Services
{
    public class FilterOperatorBuilder : IFilterOperatorBuilder
    {
        private readonly FilterOperator _filterOperator;

        public FilterOperatorBuilder(IFilterOperatorProvider provider)
        {
            OperatorProvider = provider;
            _filterOperator = new FilterOperator();
        }

        protected IFilterOperatorProvider OperatorProvider { get; }

        public IFilterOperator Build() => _filterOperator;

        public IFilterOperatorBuilder CaseInsensitive()
        {
            _filterOperator.IsCaseInsensitive = true;

            return this;
        }

        public IFilterOperatorBuilder Default()
        {
            _filterOperator.IsDefault = true;

            return this;
        }

        public IFilterOperatorBuilder HasCaseSensitiveVersion(IFilterOperator filterOperator)
        {
            if (filterOperator == null)
            {
                throw new ArgumentNullException(nameof(filterOperator));
            }

            if (filterOperator.IsCaseInsensitive)
            {
                throw new ArgumentException(
                    "Case sensitive version of current operator cannot be case insensitive. " +
                    "Please provide case insensitive operator.");
            }

            _filterOperator.CaseSensitiveVersion = filterOperator;

            return this;
        }

        public IFilterOperatorBuilder HasName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(
                    $"{nameof(name)} cannot be null, empty " +
                    $"or contain only whitespace characaters.",
                    nameof(name));
            }

            _filterOperator.Name = name;

            return this;
        }

        public IFilterOperatorBuilder HasUnnegatedVersion(IFilterOperator filterOperator)
        {
            if (filterOperator == null)
            {
                throw new ArgumentNullException(nameof(filterOperator));
            }

            if (filterOperator.IsNegated)
            {
                throw new ArgumentException(
                    "Unnegated version of current operator cannot be negated. " +
                    "Please provide unnegated operator.");
            }

            _filterOperator.UnnegatedVersion = filterOperator;

            return this;
        }

        public IFilterOperatorBuilder Operator(string @operator)
        {
            if (string.IsNullOrWhiteSpace(@operator))
            {
                throw new ArgumentException(
                    $"{nameof(@operator)} cannot be null, empty " +
                    $"or contain only whitespace characaters.",
                    nameof(@operator));
            }

            _filterOperator.Operator = @operator;

            return this;
        }
    }
}
