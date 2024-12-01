using Fluorite.Strainer.Models.Filtering;
using Fluorite.Strainer.Models.Filtering.Terms;
using Fluorite.Strainer.Services.Configuration;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Filtering;

public class CustomFilteringExpressionProvider : ICustomFilteringExpressionProvider
{
    private readonly IConfigurationCustomMethodsProvider _configurationCustomMethodsProvider;

    public CustomFilteringExpressionProvider(IConfigurationCustomMethodsProvider configurationCustomMethodsProvider)
    {
        _configurationCustomMethodsProvider = Guard.Against.Null(configurationCustomMethodsProvider);
    }

    public bool TryGetCustomExpression<T>(
        IFilterTerm filterTerm,
        string filterTermName,
        out Expression<Func<T, bool>>? expression)
    {
        Guard.Against.Null(filterTerm);
        Guard.Against.Null(filterTermName);

        var customFilterMethods = _configurationCustomMethodsProvider.GetCustomFilterMethods();
        if (customFilterMethods.TryGetValue(typeof(T), out var typeCustomFilterMethods)
            && typeCustomFilterMethods.TryGetValue(filterTermName, out var customMethod))
        {
            var customFilterMethod = (ICustomFilterMethod<T>)customMethod;

            expression = customFilterMethod.FilterTermExpression is not null
                ? customFilterMethod.FilterTermExpression(filterTerm)
                : customFilterMethod.Expression!;

            return true;
        }

        expression = null;

        return false;
    }
}
