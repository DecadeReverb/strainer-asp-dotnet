using Fluorite.Strainer.Exceptions;
using Fluorite.Strainer.Models.Sorting;
using Fluorite.Strainer.Models.Sorting.Terms;
using Fluorite.Strainer.Services.Configuration;

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

            sortedSource = (customMethod as ICustomSortMethod<T>).Function(source, sortTerm.IsDescending, isSubsequent);

            return true;
        }

        private bool TryGetCustomSortingMethod<T>(ISortTerm sortTerm, out ICustomSortMethod customMethod)
        {
            customMethod = null;

            return _configurationCustomMethodsProvider.GetCustomSortMethods().TryGetValue(typeof(T), out var customSortMethods)
                && customSortMethods.TryGetValue(sortTerm.Name, out customMethod);
        }
    }
}
