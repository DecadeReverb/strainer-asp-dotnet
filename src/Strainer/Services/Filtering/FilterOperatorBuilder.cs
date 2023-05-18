using Fluorite.Strainer.Models.Filtering.Operators;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Filtering
{
    public class FilterOperatorBuilder : IFilterOperatorBuilder
    {
        private readonly IDictionary<string, IFilterOperator> _filterOperators;

        public FilterOperatorBuilder(
            IDictionary<string, IFilterOperator> filterOperators,
            string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentException(
                    $"{nameof(symbol)} cannot be null, empty " +
                    $"or contain only whitespace characters.",
                    nameof(symbol));
            }

            _filterOperators = filterOperators;
            Symbol = symbol;

            Save(Build()); // Is this really needed?
        }

        protected Func<IFilterExpressionContext, Expression> Expression { get; set; }

        protected bool IsCaseInsensitive1 { get; set; }

        protected bool IsStringBased1 { get; set; }

        protected string Name { get; set; }

        protected string Symbol { get; set; }

        public IFilterOperator Build() => new FilterOperator
        {
            Expression = Expression,
            IsCaseInsensitive = IsCaseInsensitive1,
            IsStringBased = IsStringBased1,
            Name = Name,
            Symbol = Symbol,
        };

        public IFilterOperatorBuilder HasExpression(Func<IFilterExpressionContext, Expression> expression)
        {
            Expression = expression ?? throw new ArgumentNullException(nameof(expression));
            Save(Build());

            return this;
        }

        public IFilterOperatorBuilder HasName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(
                    $"{nameof(name)} cannot be null, empty " +
                    $"or contain only whitespace characters.",
                    nameof(name));
            }

            Name = name;
            Save(Build());

            return this;
        }

        public IFilterOperatorBuilder IsCaseInsensitive()
        {
            IsCaseInsensitive1 = true;
            Save(Build());

            return this;
        }

        public IFilterOperatorBuilder IsStringBased()
        {
            IsStringBased1 = true;
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

            _filterOperators[Symbol] = filterOperator;
        }
    }
}
