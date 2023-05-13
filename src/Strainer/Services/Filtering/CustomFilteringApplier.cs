using Fluorite.Strainer.Exceptions;
using Fluorite.Strainer.Models.Filtering;
using Fluorite.Strainer.Models.Filtering.Terms;
using Fluorite.Strainer.Services.Configuration;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Filtering
{
    public class CustomFilteringApplier : ICustomFilteringApplier
    {
        private readonly IConfigurationCustomMethodsProvider _configurationCustomMethodsProvider;

        public CustomFilteringApplier(IConfigurationCustomMethodsProvider configurationCustomMethodsProvider)
        {
            _configurationCustomMethodsProvider = configurationCustomMethodsProvider;
        }

        public bool TryApplyCustomFiltering<T>(
            IQueryable<T> source,
            IFilterTerm filterTerm,
            string filterTermName,
            out IQueryable<T> filteredSource)
        {
            var customFilterMethods = _configurationCustomMethodsProvider.GetCustomFilterMethods();

            if (customFilterMethods.TryGetValue(typeof(T), out var typeCustomFilterMethods)
                && typeCustomFilterMethods.TryGetValue(filterTermName, out var customMethod))
            {
                var customFilterExpression = GetExpression(filterTerm, customMethod as ICustomFilterMethod<T>);

                filteredSource = source.Where(customFilterExpression);

                return true;
            }

            throw new StrainerMethodNotFoundException(
                filterTermName,
                $"Property or custom filter method '{filterTermName}' was not found.");
        }

        private Expression<Func<T, bool>> GetExpression<T>(IFilterTerm filterTerm, ICustomFilterMethod<T> customFilterMethod)
        {
            return customFilterMethod.FilterTermExpression is not null
                ? customFilterMethod.FilterTermExpression(filterTerm)
                : customFilterMethod.Expression;
        }
    }
}
