using Fluorite.Strainer.Models.Sorting.Terms;

namespace Fluorite.Strainer.Services.Sorting
{
    public interface ISortingApplier
    {
        bool TryApplySorting<T>(IList<ISortTerm> sortTerms, IQueryable<T> source, out IQueryable<T> sortedSource);
    }
}
