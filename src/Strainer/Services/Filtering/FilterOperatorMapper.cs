using Fluorite.Extensions;
using Fluorite.Strainer.Models.Filtering.Operators;
using Fluorite.Strainer.Services.Validation;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Filtering;

public class FilterOperatorMapper : IFilterOperatorMapper
{
    private readonly IDictionary<string, IFilterOperator> _filterOperators;
    private readonly IFilterOperatorValidator _validator;

    static FilterOperatorMapper()
    {
        DefaultOperators = new ReadOnlyDictionary<string, IFilterOperator>(
            GetDefaultFilterOperators()
                .ToDictionary(filterOperator => filterOperator.Symbol, filterOperator => filterOperator));
    }

    public FilterOperatorMapper(IFilterOperatorValidator validator)
    {
        _filterOperators = DefaultOperators.ToDictionary();
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));

        _validator.Validate(_filterOperators.Values);
    }

    public static IReadOnlyDictionary<string, IFilterOperator> DefaultOperators { get; }

    public int Count => _filterOperators.Count;

    public IFilterOperator Find(string symbol)
    {
        if (symbol == null)
        {
            throw new ArgumentNullException(nameof(symbol));
        }

        _filterOperators.TryGetValue(symbol, out var filterOperator);

        return filterOperator;
    }

    public IFilterOperatorBuilder Operator(string symbol)
    {
        if (string.IsNullOrWhiteSpace(symbol))
        {
            throw new ArgumentException(
                $"{nameof(symbol)} cannot be null, empty " +
                $"or contain only whitespace characters.",
                nameof(symbol));
        }

        return new FilterOperatorBuilder(_filterOperators, symbol);
    }

    private static IFilterOperator[] GetDefaultFilterOperators()
    {
        return GetEqualFilterOperators()
            .Concat(
                GetLessThanFilterOperators(),
                GetGreaterThanFilterOperators(),
                GetStringFilterOperators(),
                GetStringNegatedFilterOperators(),
                GetEqualCaseInsensitiveFilterOperators(),
                GetStringCaseInsensitiveFilterOperators(),
                GetStringNegatedCaseInsensitiveFilterOperators())
            .ToArray();
    }

    private static List<IFilterOperator> GetEqualFilterOperators()
    {
        return new List<IFilterOperator>
        {
            new FilterOperatorBuilder(null, symbol: "==")
                .HasName("equal")
                .HasExpression((context) => Expression.Equal(context.FilterValue, context.PropertyValue))
                .Build(),
            new FilterOperatorBuilder(null, symbol: "!=")
                .HasName("does not equal")
                .HasExpression((context) => Expression.NotEqual(context.FilterValue, context.PropertyValue))
                .Build(),
        };
    }

    private static IEnumerable<IFilterOperator> GetLessThanFilterOperators()
    {
        return new List<IFilterOperator>
        {
            new FilterOperatorBuilder(null, symbol: "<")
                .HasName("less than")
                .HasExpression((context) => Expression.LessThan(context.PropertyValue, context.FilterValue))
                .Build(),
            new FilterOperatorBuilder(null, symbol: "<=")
                .HasName("less than or equal to")
                .HasExpression((context) => Expression.LessThanOrEqual(context.PropertyValue, context.FilterValue))
                .Build(),
        };
    }

    private static IEnumerable<IFilterOperator> GetGreaterThanFilterOperators()
    {
        return new List<IFilterOperator>
        {
            new FilterOperatorBuilder(null, symbol: ">")
                .HasName("greater than")
                .HasExpression((context) => Expression.GreaterThan(context.PropertyValue, context.FilterValue))
                .Build(),
            new FilterOperatorBuilder(null, symbol: ">=")
                .HasName("greater than or equal to")
                .HasExpression((context) => Expression.GreaterThanOrEqual(context.PropertyValue, context.FilterValue))
                .Build(),
        };
    }

    private static IEnumerable<IFilterOperator> GetStringFilterOperators()
    {
        return new List<IFilterOperator>
        {
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
        };
    }

    private static IEnumerable<IFilterOperator> GetStringNegatedFilterOperators()
    {
        return new List<IFilterOperator>
        {
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
        };
    }

    private static IEnumerable<IFilterOperator> GetEqualCaseInsensitiveFilterOperators()
    {
        return new List<IFilterOperator>
        {
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
        };
    }

    private static IEnumerable<IFilterOperator> GetStringCaseInsensitiveFilterOperators()
    {
        return new List<IFilterOperator>
        {
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
        };
    }

    private static IEnumerable<IFilterOperator> GetStringNegatedCaseInsensitiveFilterOperators()
    {
        return new List<IFilterOperator>
        {
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
