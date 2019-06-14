using Fluorite.Strainer.Models.Filtering.Operators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Filtering
{
    public class FilterOperatorMapper : IFilterOperatorMapper
    {
        private readonly Dictionary<string, IFilterOperator> _operators;
        private readonly IFilterOperatorValidator _validator;

        public FilterOperatorMapper(IFilterOperatorValidator validator)
        {
            _operators = new Dictionary<string, IFilterOperator>();
            _validator = validator;

            AddInitialFilterOperators();
        }

        public IReadOnlyCollection<IFilterOperator> Operators => _operators.Values;

        public IReadOnlyCollection<string> Symbols => _operators.Keys;

        public void AddMap(string symbol, IFilterOperator filterOperator)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentException(
                    $"{nameof(symbol)} cannot be null, empty " +
                    $"or contain only whitespace characaters.",
                    nameof(symbol));
            }

            if (filterOperator == null)
            {
                throw new ArgumentNullException(nameof(filterOperator));
            }

            _operators[symbol] = filterOperator;
        }

        public IFilterOperatorBuilder AddOperator(string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentException(
                    $"{nameof(symbol)} cannot be null, empty " +
                    $"or contain only whitespace characaters.",
                    nameof(symbol));
            }

            if (Symbols.Contains(symbol, StringComparer.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    $"There is already existing operator with a symbol {symbol} " +
                    $"in use.\n" +
                    $"Use {nameof(UpdateOperator)} to overwrite existing operator.");
            }

            return new FilterOperatorBuilder(this, symbol);
        }

        public IFilterOperator Find(string symbol)
        {
            if (symbol == null)
            {
                throw new ArgumentNullException(nameof(symbol));
            }

            return _operators.Values.FirstOrDefault(f => f.Symbol.Equals(symbol, StringComparison.OrdinalIgnoreCase));
        }

        public IFilterOperator GetDefault()
        {
            return _operators.Values.FirstOrDefault(f => f.IsDefault);
        }

        public IFilterOperatorBuilder UpdateOperator(string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentException(
                    $"{nameof(symbol)} cannot be null, empty " +
                    $"or contain only whitespace characaters.",
                    nameof(symbol));
            }

            if (!Symbols.Contains(symbol, StringComparer.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException(
                    $"There is no registered operator with a symbol {symbol}." +
                    $"Use {nameof(AddOperator)} to add new operator.");
            }

            return new FilterOperatorBuilder(this, symbol);
        }

        private void AddInitialFilterOperators()
        {
            new FilterOperatorBuilder(this, symbol: "==")
                .HasName("equal to")
                .HasExpression((context) => Expression.Equal(context.FilterValue, context.PropertyValue))
                .IsDefault();
            new FilterOperatorBuilder(this, symbol: "!=")
                .HasName("not equal to")
                .HasExpression((context) => Expression.NotEqual(context.FilterValue, context.PropertyValue));


            new FilterOperatorBuilder(this, symbol: "<")
                .HasName("less then")
                .HasExpression((context) => Expression.LessThan(context.FilterValue, context.PropertyValue));
            new FilterOperatorBuilder(this, symbol: "<=")
                .HasName("less then or equal to")
                .HasExpression((context) => Expression.LessThanOrEqual(context.FilterValue, context.PropertyValue));


            new FilterOperatorBuilder(this, symbol: ">")
                .HasName("greater then")
                .HasExpression((context) => Expression.GreaterThan(context.FilterValue, context.PropertyValue));
            new FilterOperatorBuilder(this, symbol: ">=")
                .HasName("greater then or equal to")
                .HasExpression((context) => Expression.GreaterThanOrEqual(context.FilterValue, context.PropertyValue));


            new FilterOperatorBuilder(this, symbol: "@=")
                .HasName("contains")
                .HasExpression((context) => Expression.Call(
                    context.PropertyValue,
                    typeof(string).GetMethod(nameof(string.Contains), new Type[] { typeof(string) }),
                    context.FilterValue));
            new FilterOperatorBuilder(this, symbol: "_=")
                .HasName("starts with")
                .HasExpression((context) => Expression.Call(
                    context.PropertyValue,
                    typeof(string).GetMethod(nameof(string.StartsWith), new Type[] { typeof(string) }),
                    context.FilterValue));


            new FilterOperatorBuilder(this, symbol: "!@=")
                .HasName("does not contain")
                .HasExpression((context) => Expression.Call(
                    context.PropertyValue,
                    typeof(string).GetMethod(nameof(string.Contains), new Type[] { typeof(string) }),
                    context.FilterValue))
                .NegateExpression();
            new FilterOperatorBuilder(this, symbol: "!_=")
                .HasName("does not start with")
                .HasExpression((context) => Expression.Call(
                    context.PropertyValue,
                    typeof(string).GetMethod(nameof(string.StartsWith), new Type[] { typeof(string) }),
                    context.FilterValue))
                .NegateExpression();


            new FilterOperatorBuilder(this, symbol: "==*")
                .HasName("equal to (case insensitive)")
                .HasExpression((context) => Expression.Equal(context.FilterValue, context.PropertyValue))
                .IsCaseInsensitive();


            new FilterOperatorBuilder(this, symbol: "@=*")
                .HasName("contains (case insensitive)")
                .HasExpression((context) => Expression.Call(
                    context.PropertyValue,
                    typeof(string).GetMethod(nameof(string.Contains), new Type[] { typeof(string) }),
                    context.FilterValue))
                .IsCaseInsensitive();
            new FilterOperatorBuilder(this, symbol: "_=*")
                .HasName("starts with (case insensitive)")
                .HasExpression((context) => Expression.Call(
                    context.PropertyValue,
                    typeof(string).GetMethod(nameof(string.StartsWith), new Type[] { typeof(string) }),
                    context.FilterValue))
                .IsCaseInsensitive();


            new FilterOperatorBuilder(this, symbol: "!@=*")
                .HasName("does not contain (case insensitive)")
                .HasExpression((context) => Expression.Call(
                    context.PropertyValue,
                    typeof(string).GetMethod(nameof(string.Contains), new Type[] { typeof(string) }),
                    context.FilterValue))
                .IsCaseInsensitive()
                .NegateExpression();
            new FilterOperatorBuilder(this, symbol: "!_=*")
                .HasName("does not start with (case insensitive)")
                .HasExpression((context) => Expression.Call(
                    context.PropertyValue,
                    typeof(string).GetMethod(nameof(string.StartsWith), new Type[] { typeof(string) }),
                    context.FilterValue))
                .IsCaseInsensitive()
                .NegateExpression();
        }
    }
}
