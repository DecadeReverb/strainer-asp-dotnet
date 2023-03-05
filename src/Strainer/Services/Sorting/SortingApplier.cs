using Fluorite.Extensions;
using Fluorite.Strainer.Exceptions;
using Fluorite.Strainer.Models.Sorting.Terms;

namespace Fluorite.Strainer.Services.Sorting
{
    public class SortingApplier : ISortingApplier
    {
        private readonly ICustomSortingApplier _customSortingApplier;

        public SortingApplier(ICustomSortingApplier customSortingApplier)
        {
            _customSortingApplier = customSortingApplier;
        }

        public bool TryApplySorting<T>(IStrainerContext context, IList<ISortTerm> sortTerms, IQueryable<T> source, out IQueryable<T> sortedSource)
        {
            var isSubsequent = false;
            var isSortingApplied = false;
            sortedSource = source;

            foreach (var sortTerm in sortTerms)
            {
                var metadata = context.Metadata.GetMetadata<T>(
                    isSortableRequired: true,
                    isFilterableRequired: false,
                    name: sortTerm.Name);

                if (metadata != null)
                {
                    var sortExpression = context.Sorting.ExpressionProvider.GetExpression<T>(metadata.PropertyInfo, sortTerm, isSubsequent);
                    if (sortExpression != null)
                    {
                        sortedSource = sortedSource.OrderWithSortExpression(sortExpression);
                        isSortingApplied = true;
                    }
                }
                else
                {
                    try
                    {
                        isSortingApplied = _customSortingApplier.TryApplyCustomSorting(sortTerm, isSubsequent, sortedSource, out sortedSource);
                    }
                    catch (StrainerException) when (!context.Options.ThrowExceptions)
                    {
                        return false;
                    }
                }

                isSubsequent = true;
            }

            return isSortingApplied;
        }
    }
}
