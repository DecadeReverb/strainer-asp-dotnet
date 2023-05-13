using Fluorite.Extensions;
using Fluorite.Strainer.Exceptions;
using Fluorite.Strainer.Models.Sorting;
using Fluorite.Strainer.Models.Sorting.Terms;
using Fluorite.Strainer.Services.Configuration;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Sorting
{
    public class CustomSortingApplier : ICustomSortingApplier
    {
        private readonly IConfigurationCustomMethodsProvider _configurationCustomMethodsProvider;

        public CustomSortingApplier(IConfigurationCustomMethodsProvider configurationCustomMethodsProvider)
        {
            _configurationCustomMethodsProvider = configurationCustomMethodsProvider;
        }

        public bool TryApplyCustomSorting<T>(
            ISortTerm sortTerm,
            bool isSubsequent,
            IQueryable<T> source,
            out IQueryable<T> sortedSource)
        {
            if (!TryGetCustomSortingMethod<T>(sortTerm, out var customMethod))
            {
                throw new StrainerMethodNotFoundException(
                    sortTerm.Name,
                    $"Property or custom sorting method '{sortTerm.Name}' was not found.");
            }

            var expression = GetExpression(sortTerm, customMethod as ICustomSortMethod<T>);
            var sortExpression = new SortExpression<T>
            {
                Expression = expression,
                IsDescending = sortTerm.IsDescending,
                IsSubsequent = isSubsequent,
            };

            sortedSource = source.OrderWithSortExpression(sortExpression);

            return true;
        }

        private Expression<Func<T, object>> GetExpression<T>(ISortTerm sortTerm, ICustomSortMethod<T> customSortMethod)
        {
            return customSortMethod.SortTermExpression != null
                ? customSortMethod.SortTermExpression(sortTerm)
                : customSortMethod.Expression;
        }

        private bool TryGetCustomSortingMethod<T>(ISortTerm sortTerm, out ICustomSortMethod customMethod)
        {
            customMethod = null;

            return _configurationCustomMethodsProvider.GetCustomSortMethods().TryGetValue(typeof(T), out var customSortMethods)
                && customSortMethods.TryGetValue(sortTerm.Name, out customMethod);
        }
    }
}
