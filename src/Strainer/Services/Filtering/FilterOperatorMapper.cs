using Fluorite.Strainer.Models.Filter.Operators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Filtering
{
    public class FilterOperatorMapper : IFilterOperatorMapper
    {
        private readonly Dictionary<string, IFilterOperator> _map;
        private readonly IFilterOperatorValidator _validator;

        public FilterOperatorMapper(IFilterOperatorValidator validator)
        {
            _map = new Dictionary<string, IFilterOperator>();
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));

            AddInitialFilterOperators();

            _validator.Validate(_map.Values);
        }

        public IReadOnlyCollection<IFilterOperator> Operators => _map.Values;

        public IReadOnlyCollection<string> Symbols => _map.Keys;

        public void AddMap(string symbol, IFilterOperator filterOperator)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentException(
                    $"{nameof(symbol)} cannot be null, empty " +
                    $"or contain only whitespace characaters.",
                    nameof(symbol));
            }

            _map[symbol] = filterOperator ?? throw new ArgumentNullException(nameof(filterOperator));
        }

        public IFilterOperatorBuilder Operator(string symbol)
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
                    $"There is already existing operator with a symbol {symbol}. " +
                    $"Choose a different symbol.");
            }

            return new FilterOperatorBuilder(this, symbol);
        }

        public IFilterOperator Find(string symbol)
        {
            if (symbol == null)
            {
                throw new ArgumentNullException(nameof(symbol));
            }

            return _map.Values.FirstOrDefault(f => f.Symbol.Equals(symbol, StringComparison.OrdinalIgnoreCase));
        }

        public IFilterOperator GetDefault()
        {
            return _map.Values.FirstOrDefault(f => f.IsDefault);
        }

        private void AddInitialFilterOperators()
        {
            new FilterOperatorBuilder(this, symbol: "==")
                .HasName("equal")
                .HasExpression((context) => Expression.Equal(context.FilterValue, context.PropertyValue))
                .IsDefault();
            new FilterOperatorBuilder(this, symbol: "!=")
                .HasName("does not equal")
                .HasExpression((context) => Expression.NotEqual(context.FilterValue, context.PropertyValue));

            new FilterOperatorBuilder(this, symbol: "<")
                .HasName("less than")
                .HasExpression((context) => Expression.LessThan(context.PropertyValue, context.FilterValue));
            new FilterOperatorBuilder(this, symbol: "<=")
                .HasName("less than or equal to")
                .HasExpression((context) => Expression.LessThanOrEqual(context.PropertyValue, context.FilterValue));

            new FilterOperatorBuilder(this, symbol: ">")
                .HasName("greater than")
                .HasExpression((context) => Expression.GreaterThan(context.PropertyValue, context.FilterValue));
            new FilterOperatorBuilder(this, symbol: ">=")
                .HasName("greater than or equal to")
                .HasExpression((context) => Expression.GreaterThanOrEqual(context.PropertyValue, context.FilterValue));

            new FilterOperatorBuilder(this, symbol: "@=")
                .HasName("contains")
                .IsStringBased()
                .HasExpression((context) => Expression.Call(
                    context.PropertyValue,
                    typeof(string).GetMethod(nameof(string.Contains), new Type[] { typeof(string) }),
                    context.FilterValue));
            new FilterOperatorBuilder(this, symbol: "_=")
                .HasName("starts with")
                .IsStringBased()
                .HasExpression((context) => Expression.Call(
                    context.PropertyValue,
                    typeof(string).GetMethod(nameof(string.StartsWith), new Type[] { typeof(string) }),
                    context.FilterValue));
            new FilterOperatorBuilder(this, symbol: "=_")
                .HasName("ends with")
                .IsStringBased()
                .HasExpression((context) => Expression.Call(
                    context.PropertyValue,
                    typeof(string).GetMethod(nameof(string.EndsWith), new Type[] { typeof(string) }),
                    context.FilterValue));

            new FilterOperatorBuilder(this, symbol: "!@=")
                .HasName("does not contain")
                .IsStringBased()
                .HasExpression((context) => Expression.Not(Expression.Call(
                    context.PropertyValue,
                    typeof(string).GetMethod(nameof(string.Contains), new Type[] { typeof(string) }),
                    context.FilterValue)));
            new FilterOperatorBuilder(this, symbol: "!_=")
                .HasName("does not start with")
                .IsStringBased()
                .HasExpression((context) => Expression.Not(Expression.Call(
                    context.PropertyValue,
                    typeof(string).GetMethod(nameof(string.StartsWith), new Type[] { typeof(string) }),
                    context.FilterValue)));
            new FilterOperatorBuilder(this, symbol: "!=_")
                .HasName("does not end with")
                .IsStringBased()
                .HasExpression((context) => Expression.Not(Expression.Call(
                    context.PropertyValue,
                    typeof(string).GetMethod(nameof(string.EndsWith), new Type[] { typeof(string) }),
                    context.FilterValue)));

            new FilterOperatorBuilder(this, symbol: "==*")
                .HasName("equal (case insensitive)")
                .IsStringBased()
                .HasExpression((context) => Expression.Equal(context.FilterValue, context.PropertyValue))
                .IsCaseInsensitive();
            new FilterOperatorBuilder(this, symbol: "!=*")
                .HasName("does not equal (case insensitive)")
                .IsStringBased()
                .HasExpression((context) => Expression.NotEqual(context.FilterValue, context.PropertyValue))
                .IsCaseInsensitive();

            new FilterOperatorBuilder(this, symbol: "@=*")
                .HasName("contains (case insensitive)")
                .IsStringBased()
                .HasExpression((context) => Expression.Call(
                    context.PropertyValue,
                    typeof(string).GetMethod(nameof(string.Contains), new Type[] { typeof(string) }),
                    context.FilterValue))
                .IsCaseInsensitive();
            new FilterOperatorBuilder(this, symbol: "_=*")
                .HasName("starts with (case insensitive)")
                .IsStringBased()
                .HasExpression((context) => Expression.Call(
                    context.PropertyValue,
                    typeof(string).GetMethod(nameof(string.StartsWith), new Type[] { typeof(string) }),
                    context.FilterValue))
                .IsCaseInsensitive();
            new FilterOperatorBuilder(this, symbol: "=_*")
                .HasName("ends with (case insensitive)")
                .IsStringBased()
                .HasExpression((context) => Expression.Call(
                    context.PropertyValue,
                    typeof(string).GetMethod(nameof(string.EndsWith), new Type[] { typeof(string) }),
                    context.FilterValue))
                .IsCaseInsensitive();

            new FilterOperatorBuilder(this, symbol: "!@=*")
                .HasName("does not contain (case insensitive)")
                .IsStringBased()
                .HasExpression((context) => Expression.Not(Expression.Call(
                    context.PropertyValue,
                    typeof(string).GetMethod(nameof(string.Contains), new Type[] { typeof(string) }),
                    context.FilterValue)))
                .IsCaseInsensitive();
            new FilterOperatorBuilder(this, symbol: "!_=*")
                .HasName("does not start with (case insensitive)")
                .IsStringBased()
                .HasExpression((context) => Expression.Not(Expression.Call(
                    context.PropertyValue,
                    typeof(string).GetMethod(nameof(string.StartsWith), new Type[] { typeof(string) }),
                    context.FilterValue)))
                .IsCaseInsensitive();
            new FilterOperatorBuilder(this, symbol: "!=_*")
                .HasName("does not end with (case insensitive)")
                .IsStringBased()
                .HasExpression((context) => Expression.Not(Expression.Call(
                    context.PropertyValue,
                    typeof(string).GetMethod(nameof(string.EndsWith), new Type[] { typeof(string) }),
                    context.FilterValue)))
                .IsCaseInsensitive();
        }
    }
}
