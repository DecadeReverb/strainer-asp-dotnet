using Fluorite.Strainer.Models.Sorting;
using Fluorite.Strainer.Models.Sorting.Terms;

namespace Fluorite.Strainer.Services.Sorting
{
    public interface ICustomSortingExpressionProvider
    {
        bool TryGetCustomExpression<T>(
            ISortTerm sortTerm,
            bool isSubsequent,
            out ISortExpression<T> sortExpression);
    }
}
