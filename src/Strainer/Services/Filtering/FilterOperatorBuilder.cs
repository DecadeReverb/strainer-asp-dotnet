using Fluorite.Strainer.Models.Filtering.Operators;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Filtering
{
    public class FilterOperatorBuilder : IFilterOperatorBuilder
    {
        private readonly IDictionary<string, IFilterOperator> _filterOperators;

        protected Func<IFilterExpressionContext, Expression> _expression;
        protected string _name;
        protected bool _isCaseInsensitive;
        protected bool _isStringBased;
        protected string _symbol;

        public FilterOperatorBuilder(
            IDictionary<string, IFilterOperator> filterOperators,
            string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentException(
                    $"{nameof(symbol)} cannot be null, empty " +
                    $"or contain only whitespace characaters.",
                    nameof(symbol));
            }

            _filterOperators = filterOperators;;
            _symbol = symbol;

            Save(Build()); // Is this really needed?
        }

        public IFilterOperator Build() => new FilterOperator
        {
            Expression = _expression,
            IsCaseInsensitive = _isCaseInsensitive,
            IsStringBased = _isStringBased,
            Name = _name,
            Symbol = _symbol,
        };

        public IFilterOperatorBuilder HasExpression(Func<IFilterExpressionContext, Expression> expression)
        {
            _expression = expression ?? throw new ArgumentNullException(nameof(expression));
            Save(Build());

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

            _name = name;
            Save(Build());

            return this;
        }

        public IFilterOperatorBuilder IsCaseInsensitive()
        {
            _isCaseInsensitive = true;
            Save(Build());

            return this;
        }

        public IFilterOperatorBuilder IsStringBased()
        {
            _isStringBased = true;
            Save(Build());

            return this;
        }

        protected void Save(IFilterOperator filterOperator)
        {
            if (_filterOperators is null)
            {
                return;
            }

            if (filterOperator is null)
            {
                throw new ArgumentNullException(nameof(filterOperator));
            }

            _filterOperators[_symbol] = filterOperator;
        }
    }
}
