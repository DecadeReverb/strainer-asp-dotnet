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
        _configurationCustomMethodsProvider = configurationCustomMethodsProvider;
    }

    public bool TryGetCustomExpression<T>(
        IFilterTerm filterTerm,
        string filterTermName,
        out Expression<Func<T, bool>> expression)
    {
        if (!TryGetCustomMethod<T>(filterTermName, out var customMethod))
        {
            expression = null;

            return false;
        }

        expression = GetExpression(filterTerm, customMethod as ICustomFilterMethod<T>);

        return true;
    }

    private bool TryGetCustomMethod<T>(string filterTermName, out ICustomFilterMethod customMethod)
    {
        customMethod = null;

        return _configurationCustomMethodsProvider.GetCustomFilterMethods().TryGetValue(typeof(T), out var typeCustomFilterMethods)
            && typeCustomFilterMethods.TryGetValue(filterTermName, out customMethod);
    }

    private Expression<Func<T, bool>> GetExpression<T>(IFilterTerm filterTerm, ICustomFilterMethod<T> customFilterMethod)
    {
        return customFilterMethod.FilterTermExpression is not null
            ? customFilterMethod.FilterTermExpression(filterTerm)
            : customFilterMethod.Expression;
    }
}
