using Fluorite.Strainer.Models.Sorting.Terms;

namespace Fluorite.Strainer.Services.Sorting
{
    public interface ICustomSortingApplier
    {
        bool TryApplyCustomSorting<T>(
            ISortTerm sortTerm,
            bool isSubsequent,
            IQueryable<T> source,
            out IQueryable<T> sortedSource);
    }
}
