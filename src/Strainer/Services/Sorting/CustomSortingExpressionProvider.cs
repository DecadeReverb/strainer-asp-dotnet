﻿using Fluorite.Strainer.Models.Sorting;
using Fluorite.Strainer.Models.Sorting.Terms;
using Fluorite.Strainer.Services.Configuration;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Sorting;

public class CustomSortingExpressionProvider : ICustomSortingExpressionProvider
{
    private readonly IConfigurationCustomMethodsProvider _configurationCustomMethodsProvider;

    public CustomSortingExpressionProvider(IConfigurationCustomMethodsProvider configurationCustomMethodsProvider)
    {
        _configurationCustomMethodsProvider = Guard.Against.Null(configurationCustomMethodsProvider);
    }

    public bool TryGetCustomExpression<T>(
        ISortTerm sortTerm,
        bool isSubsequent,
        out ISortExpression<T>? sortExpression)
    {
        Guard.Against.Null(sortTerm);

        if (!TryGetCustomSortingMethod<T>(sortTerm, out var customMethod))
        {
            sortExpression = null;

            return false;
        }

        var expression = GetExpression(sortTerm, (ICustomSortMethod<T>)customMethod!);
        sortExpression = new SortExpression<T>(expression)
        {
            IsDefault = false,
            IsDescending = sortTerm.IsDescending,
            IsSubsequent = isSubsequent,
        };

        return true;
    }

    private Expression<Func<T, object>> GetExpression<T>(ISortTerm sortTerm, ICustomSortMethod<T> customSortMethod)
    {
        return customSortMethod.ExpressionProvider != null
            ? customSortMethod.ExpressionProvider(sortTerm)
            : customSortMethod.Expression!;
    }

    private bool TryGetCustomSortingMethod<T>(ISortTerm sortTerm, out ICustomSortMethod? customMethod)
    {
        customMethod = null;

        return _configurationCustomMethodsProvider.GetCustomSortMethods().TryGetValue(typeof(T), out var customSortMethods)
            && customSortMethods.TryGetValue(sortTerm.Name, out customMethod);
    }
}
