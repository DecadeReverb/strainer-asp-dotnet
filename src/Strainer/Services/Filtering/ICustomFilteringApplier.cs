using Fluorite.Strainer.Models.Filtering.Terms;

namespace Fluorite.Strainer.Services.Filtering
{
    public interface ICustomFilteringApplier
    {
        bool TryApplyCustomFiltering<T>(
            IQueryable<T> source,
            IFilterTerm filterTerm,
            string filterTermName,
            out IQueryable<T> filteredSource);
    }
}
