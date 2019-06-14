using Fluorite.Strainer.Models.Filtering.Operators;
using System;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Filtering
{
    public class FilterOperatorBuilder : IFilterOperatorBuilder
    {
        private readonly FilterOperator _filterOperator;

        public FilterOperatorBuilder(IFilterOperatorMapper mapper, string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentException(
                    $"{nameof(symbol)} cannot be null, empty " +
                    $"or contain only whitespace characaters.",
                    nameof(symbol));
            }

            _filterOperator = new FilterOperator
            {
                Symbol = symbol,
            };
            Mapper = mapper;
        }

        protected IFilterOperatorMapper Mapper { get; }

        public IFilterOperator Build() => _filterOperator;

        public IFilterOperatorBuilder HasExpression(Func<IFilterExpressionContext, Expression> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            _filterOperator.Expression = expression;
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

            _filterOperator.Name = name;
            UpdateMap();

            return this;
        }

        public IFilterOperatorBuilder IsCaseInsensitive()
        {
            _filterOperator.IsCaseInsensitive = true;
            UpdateMap();

            return this;
        }

        public IFilterOperatorBuilder IsDefault()
        {
            _filterOperator.IsDefault = true;
            UpdateMap();

            return this;
        }

        public IFilterOperatorBuilder NegateExpression()
        {
            _filterOperator.NegateExpression = true;
            UpdateMap();

            return this;
        }

        private void UpdateMap()
        {
            Mapper.AddMap(_filterOperator.Symbol, _filterOperator);
        }
    }
}
