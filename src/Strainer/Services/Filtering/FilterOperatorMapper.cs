using Fluorite.Extensions;
using Fluorite.Strainer.Models.Filtering.Operators;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Filtering;

public static class FilterOperatorMapper
{
    static FilterOperatorMapper()
    {
        DefaultOperators = new ReadOnlyDictionary<string, IFilterOperator>(
            GetDefaultFilterOperators()
                .ToDictionary(filterOperator => filterOperator.Symbol, filterOperator => filterOperator));
    }

    public static IReadOnlyDictionary<string, IFilterOperator> DefaultOperators { get; }

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
            new FilterOperatorBuilder(symbol: FilterOperatorSymbols.EqualsSymbol)
                .HasName("equal")
                .HasExpression((context) => Expression.Equal(context.FilterValue, context.PropertyValue))
                .Build(),
            new FilterOperatorBuilder(symbol: FilterOperatorSymbols.DoesNotEqual)
                .HasName("does not equal")
                .HasExpression((context) => Expression.NotEqual(context.FilterValue, context.PropertyValue))
                .Build(),
        };
    }

    private static IEnumerable<IFilterOperator> GetLessThanFilterOperators()
    {
        return new List<IFilterOperator>
        {
            new FilterOperatorBuilder(symbol: FilterOperatorSymbols.LessThan)
                .HasName("less than")
                .HasExpression((context) => Expression.LessThan(context.PropertyValue, context.FilterValue))
                .Build(),
            new FilterOperatorBuilder(symbol: FilterOperatorSymbols.LessThanOrEqualTo)
                .HasName("less than or equal to")
                .HasExpression((context) => Expression.LessThanOrEqual(context.PropertyValue, context.FilterValue))
                .Build(),
        };
    }

    private static IEnumerable<IFilterOperator> GetGreaterThanFilterOperators()
    {
        return new List<IFilterOperator>
        {
            new FilterOperatorBuilder(symbol: FilterOperatorSymbols.GreaterThan)
                .HasName("greater than")
                .HasExpression((context) => Expression.GreaterThan(context.PropertyValue, context.FilterValue))
                .Build(),
            new FilterOperatorBuilder(symbol: FilterOperatorSymbols.GreaterThanOrEqualTo)
                .HasName("greater than or equal to")
                .HasExpression((context) => Expression.GreaterThanOrEqual(context.PropertyValue, context.FilterValue))
                .Build(),
        };
    }

    private static IEnumerable<IFilterOperator> GetStringFilterOperators()
    {
        return new List<IFilterOperator>
        {
            new FilterOperatorBuilder(symbol: FilterOperatorSymbols.Contains)
                .HasName("contains")
                .IsStringBased()
                .HasExpression((context) => Expression.Call(
                    context.PropertyValue,
                    typeof(string).GetMethod(nameof(string.Contains), new Type[] { typeof(string) }),
                    context.FilterValue))
                .Build(),
            new FilterOperatorBuilder(symbol: FilterOperatorSymbols.StartsWith)
                .HasName("starts with")
                .IsStringBased()
                .HasExpression((context) => Expression.Call(
                    context.PropertyValue,
                    typeof(string).GetMethod(nameof(string.StartsWith), new Type[] { typeof(string) }),
                    context.FilterValue))
                .Build(),
            new FilterOperatorBuilder(symbol: FilterOperatorSymbols.EndsWith)
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
            new FilterOperatorBuilder(symbol: FilterOperatorSymbols.DoesNotContain)
                .HasName("does not contain")
                .IsStringBased()
                .HasExpression((context) => Expression.Not(Expression.Call(
                    context.PropertyValue,
                    typeof(string).GetMethod(nameof(string.Contains), new Type[] { typeof(string) }),
                    context.FilterValue)))
                .Build(),
            new FilterOperatorBuilder(symbol: FilterOperatorSymbols.DoesNotStartWith)
                .HasName("does not start with")
                .IsStringBased()
                .HasExpression((context) => Expression.Not(Expression.Call(
                    context.PropertyValue,
                    typeof(string).GetMethod(nameof(string.StartsWith), new Type[] { typeof(string) }),
                    context.FilterValue)))
                .Build(),
            new FilterOperatorBuilder(symbol: FilterOperatorSymbols.DoesNotEndWith)
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
            new FilterOperatorBuilder(symbol: FilterOperatorSymbols.EqualsCaseInsensitive)
                .HasName("equal (case insensitive)")
                .IsStringBased()
                .HasExpression((context) => Expression.Equal(context.FilterValue, context.PropertyValue))
                .IsCaseInsensitive()
                .Build(),
            new FilterOperatorBuilder(symbol: FilterOperatorSymbols.DoesNotEqualCaseInsensitive)
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
            new FilterOperatorBuilder(symbol: FilterOperatorSymbols.ContainsCaseInsensitive)
                .HasName("contains (case insensitive)")
                .IsStringBased()
                .HasExpression((context) => Expression.Call(
                    context.PropertyValue,
                    typeof(string).GetMethod(nameof(string.Contains), new Type[] { typeof(string) }),
                    context.FilterValue))
                .IsCaseInsensitive()
                .Build(),
            new FilterOperatorBuilder(symbol: FilterOperatorSymbols.StartsWithCaseInsensitive)
                .HasName("starts with (case insensitive)")
                .IsStringBased()
                .HasExpression((context) => Expression.Call(
                    context.PropertyValue,
                    typeof(string).GetMethod(nameof(string.StartsWith), new Type[] { typeof(string) }),
                    context.FilterValue))
                .IsCaseInsensitive()
                .Build(),
            new FilterOperatorBuilder(symbol: FilterOperatorSymbols.EndsWithCaseInsensitive)
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
            new FilterOperatorBuilder(symbol: FilterOperatorSymbols.DoesNotContainCaseInsensitive)
                .HasName("does not contain (case insensitive)")
                .IsStringBased()
                .HasExpression((context) => Expression.Not(Expression.Call(
                    context.PropertyValue,
                    typeof(string).GetMethod(nameof(string.Contains), new Type[] { typeof(string) }),
                    context.FilterValue)))
                .IsCaseInsensitive()
                .Build(),
            new FilterOperatorBuilder(symbol: FilterOperatorSymbols.DoesNotStartWithCaseInsensitive)
                .HasName("does not start with (case insensitive)")
                .IsStringBased()
                .HasExpression((context) => Expression.Not(Expression.Call(
                    context.PropertyValue,
                    typeof(string).GetMethod(nameof(string.StartsWith), new Type[] { typeof(string) }),
                    context.FilterValue)))
                .IsCaseInsensitive()
                .Build(),
            new FilterOperatorBuilder(symbol: FilterOperatorSymbols.DoesNotEndWithCaseInsensitive)
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
