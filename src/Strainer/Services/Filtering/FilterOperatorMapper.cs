using Fluorite.Strainer.Models.Filtering.Operators;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Filtering
{
    public class FilterOperatorMapper :
        IFilterOperatorMapper,
        IDictionary<string, IFilterOperator>,
        ICollection<KeyValuePair<string, IFilterOperator>>,
        IEnumerable<KeyValuePair<string, IFilterOperator>>,
        IEnumerable
    {
        private readonly IDictionary<string, IFilterOperator> _filterOperators;
        private readonly IFilterOperatorValidator _validator;

        public static IReadOnlyDictionary<string, IFilterOperator> DefaultOperators
        {
            get
            {
                return new ReadOnlyDictionary<string, IFilterOperator>(
                    GetDefaultFilterOperators()
                        .ToDictionary(
                            filterOperator => filterOperator.Symbol,
                            FilterOperator => FilterOperator));
            }
        }

        public FilterOperatorMapper(IFilterOperatorValidator validator)
        {
            _filterOperators = new Dictionary<string, IFilterOperator>();
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));

            GetDefaultFilterOperators()
                .ToList()
                .ForEach(filterOperator => _filterOperators[filterOperator.Symbol] = filterOperator);

            _validator.Validate(_filterOperators.Values);
        }

        public IFilterOperator this[string key]
        {
            get
            {
                return _filterOperators[key];
            }
            set
            {
                _filterOperators[key] = value;
            }

        }

        public int Count => _filterOperators.Count;

        public bool IsReadOnly => false;

        public ICollection<string> Keys => _filterOperators.Keys;

        public ICollection<IFilterOperator> Values => _filterOperators.Values;

        public void Add(string key, IFilterOperator value)
        {
            _filterOperators.Add(key, value);
        }

        public void Add(KeyValuePair<string, IFilterOperator> item)
        {
            _filterOperators.Add(item);
        }

        public void AddMap(string symbol, IFilterOperator filterOperator)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentException(
                    $"{nameof(symbol)} cannot be null, empty " +
                    $"or contain only whitespace characaters.",
                    nameof(symbol));
            }

            _filterOperators[symbol] = filterOperator ?? throw new ArgumentNullException(nameof(filterOperator));
        }

        public IFilterOperator Find(string symbol)
        {
            if (symbol == null)
            {
                throw new ArgumentNullException(nameof(symbol));
            }

            if (_filterOperators.TryGetValue(symbol, out var filterOperator))
            {
                return filterOperator;
            }

            return null;
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

            //if (Keys.Contains(symbol))
            //{
            //    throw new InvalidOperationException(
            //        $"There is already existing operator with a symbol {symbol}. " +
            //        $"Choose a different symbol.");
            //}

            return new FilterOperatorBuilder(_filterOperators, symbol);
        }

        public IEnumerator GetEnumerator() => _filterOperators.GetEnumerator();

        public void Clear()
        {
            _filterOperators.Clear();
        }

        public bool Contains(KeyValuePair<string, IFilterOperator> item)
        {
            return _filterOperators.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, IFilterOperator>[] array, int arrayIndex)
        {
            _filterOperators.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, IFilterOperator> item) => _filterOperators.Remove(item);

        public bool ContainsKey(string key) => _filterOperators.ContainsKey(key);

        public bool Remove(string key) => _filterOperators.Remove(key);

        public bool TryGetValue(string key, out IFilterOperator value)
        {
            return _filterOperators.TryGetValue(key, out value);
        }

        IEnumerator<KeyValuePair<string, IFilterOperator>> IEnumerable<KeyValuePair<string, IFilterOperator>>.GetEnumerator()
        {
            return _filterOperators.GetEnumerator();
        }

        private static IFilterOperator[] GetDefaultFilterOperators()
        {
            return new[]
            {
                new FilterOperatorBuilder(null, symbol: "==")
                    .HasName("equal")
                    .HasExpression((context) => Expression.Equal(context.FilterValue, context.PropertyValue))
                    .Build(),
                new FilterOperatorBuilder(null, symbol: "!=")
                    .HasName("does not equal")
                    .HasExpression((context) => Expression.NotEqual(context.FilterValue, context.PropertyValue))
                    .Build(),

                new FilterOperatorBuilder(null, symbol: "<")
                    .HasName("less than")
                    .HasExpression((context) => Expression.LessThan(context.PropertyValue, context.FilterValue))
                    .Build(),
                new FilterOperatorBuilder(null, symbol: "<=")
                    .HasName("less than or equal to")
                    .HasExpression((context) => Expression.LessThanOrEqual(context.PropertyValue, context.FilterValue))
                    .Build(),

                new FilterOperatorBuilder(null, symbol: ">")
                    .HasName("greater than")
                    .HasExpression((context) => Expression.GreaterThan(context.PropertyValue, context.FilterValue))
                    .Build(),
                new FilterOperatorBuilder(null, symbol: ">=")
                    .HasName("greater than or equal to")
                    .HasExpression((context) => Expression.GreaterThanOrEqual(context.PropertyValue, context.FilterValue))
                    .Build(),

                new FilterOperatorBuilder(null, symbol: "@=")
                    .HasName("contains")
                    .IsStringBased()
                    .HasExpression((context) => Expression.Call(
                        context.PropertyValue,
                        typeof(string).GetMethod(nameof(string.Contains), new Type[] { typeof(string) }),
                        context.FilterValue))
                    .Build(),
                new FilterOperatorBuilder(null, symbol: "_=")
                    .HasName("starts with")
                    .IsStringBased()
                    .HasExpression((context) => Expression.Call(
                        context.PropertyValue,
                        typeof(string).GetMethod(nameof(string.StartsWith), new Type[] { typeof(string) }),
                        context.FilterValue))
                    .Build(),
                new FilterOperatorBuilder(null, symbol: "=_")
                    .HasName("ends with")
                    .IsStringBased()
                    .HasExpression((context) => Expression.Call(
                        context.PropertyValue,
                        typeof(string).GetMethod(nameof(string.EndsWith), new Type[] { typeof(string) }),
                        context.FilterValue))
                    .Build(),

                new FilterOperatorBuilder(null, symbol: "!@=")
                    .HasName("does not contain")
                    .IsStringBased()
                    .HasExpression((context) => Expression.Not(Expression.Call(
                        context.PropertyValue,
                        typeof(string).GetMethod(nameof(string.Contains), new Type[] { typeof(string) }),
                        context.FilterValue)))
                    .Build(),
                new FilterOperatorBuilder(null, symbol: "!_=")
                    .HasName("does not start with")
                    .IsStringBased()
                    .HasExpression((context) => Expression.Not(Expression.Call(
                        context.PropertyValue,
                        typeof(string).GetMethod(nameof(string.StartsWith), new Type[] { typeof(string) }),
                        context.FilterValue)))
                    .Build(),
                new FilterOperatorBuilder(null, symbol: "!=_")
                    .HasName("does not end with")
                    .IsStringBased()
                    .HasExpression((context) => Expression.Not(Expression.Call(
                        context.PropertyValue,
                        typeof(string).GetMethod(nameof(string.EndsWith), new Type[] { typeof(string) }),
                        context.FilterValue)))
                    .Build(),

                new FilterOperatorBuilder(null, symbol: "==*")
                    .HasName("equal (case insensitive)")
                    .IsStringBased()
                    .HasExpression((context) => Expression.Equal(context.FilterValue, context.PropertyValue))
                    .IsCaseInsensitive()
                    .Build(),
                new FilterOperatorBuilder(null, symbol: "!=*")
                    .HasName("does not equal (case insensitive)")
                    .IsStringBased()
                    .HasExpression((context) => Expression.NotEqual(context.FilterValue, context.PropertyValue))
                    .IsCaseInsensitive()
                    .Build(),

                new FilterOperatorBuilder(null, symbol: "@=*")
                    .HasName("contains (case insensitive)")
                    .IsStringBased()
                    .HasExpression((context) => Expression.Call(
                        context.PropertyValue,
                        typeof(string).GetMethod(nameof(string.Contains), new Type[] { typeof(string) }),
                        context.FilterValue))
                    .IsCaseInsensitive()
                    .Build(),
                new FilterOperatorBuilder(null, symbol: "_=*")
                    .HasName("starts with (case insensitive)")
                    .IsStringBased()
                    .HasExpression((context) => Expression.Call(
                        context.PropertyValue,
                        typeof(string).GetMethod(nameof(string.StartsWith), new Type[] { typeof(string) }),
                        context.FilterValue))
                    .IsCaseInsensitive()
                    .Build(),
                new FilterOperatorBuilder(null, symbol: "=_*")
                    .HasName("ends with (case insensitive)")
                    .IsStringBased()
                    .HasExpression((context) => Expression.Call(
                        context.PropertyValue,
                        typeof(string).GetMethod(nameof(string.EndsWith), new Type[] { typeof(string) }),
                        context.FilterValue))
                    .IsCaseInsensitive()
                    .Build(),

                new FilterOperatorBuilder(null, symbol: "!@=*")
                    .HasName("does not contain (case insensitive)")
                    .IsStringBased()
                    .HasExpression((context) => Expression.Not(Expression.Call(
                        context.PropertyValue,
                        typeof(string).GetMethod(nameof(string.Contains), new Type[] { typeof(string) }),
                        context.FilterValue)))
                    .IsCaseInsensitive()
                    .Build(),
                new FilterOperatorBuilder(null, symbol: "!_=*")
                    .HasName("does not start with (case insensitive)")
                    .IsStringBased()
                    .HasExpression((context) => Expression.Not(Expression.Call(
                        context.PropertyValue,
                        typeof(string).GetMethod(nameof(string.StartsWith), new Type[] { typeof(string) }),
                        context.FilterValue)))
                    .IsCaseInsensitive()
                    .Build(),
                new FilterOperatorBuilder(null, symbol: "!=_*")
                    .HasName("does not end with (case insensitive)")
                    .IsStringBased()
                    .HasExpression((context) => Expression.Not(Expression.Call(
                        context.PropertyValue,
                        typeof(string).GetMethod(nameof(string.EndsWith), new Type[] { typeof(string) }),
                        context.FilterValue)))
                    .IsCaseInsensitive()
                    .Build(),
            };
        }
    }
}
