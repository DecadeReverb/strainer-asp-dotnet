using Fluorite.Strainer.Models.Filtering.Operators;
using System;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Filtering
{
    public class FilterOperatorBuilder : IFilterOperatorBuilder
    {
        protected Func<IFilterExpressionContext, Expression> _expression;
        protected string _name;
        protected bool _isCaseInsensitive;
        protected bool _isStringBased;
        protected string _symbol;

        public FilterOperatorBuilder(IFilterOperatorMapper mapper, string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentException(
                    $"{nameof(symbol)} cannot be null, empty " +
                    $"or contain only whitespace characaters.",
                    nameof(symbol));
            }

            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _symbol = symbol;

            UpdateMap();
        }

        protected IFilterOperatorMapper Mapper { get; }

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
            UpdateMap();

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
            UpdateMap();

            return this;
        }

        public IFilterOperatorBuilder IsCaseInsensitive()
        {
            _isCaseInsensitive = true;
            UpdateMap();

            return this;
        }

        public IFilterOperatorBuilder IsStringBased()
        {
            _isStringBased = true;
            UpdateMap();

            return this;
        }

        private void UpdateMap()
        {
            Mapper.AddMap(_symbol, Build());
        }
    }
}
