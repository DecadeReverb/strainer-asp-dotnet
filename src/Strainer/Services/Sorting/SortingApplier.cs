using Fluorite.Extensions;
using Fluorite.Strainer.Exceptions;
using Fluorite.Strainer.Models.Sorting.Terms;
using Fluorite.Strainer.Services.Metadata;

namespace Fluorite.Strainer.Services.Sorting;

public class SortingApplier : ISortingApplier
{
    private readonly ICustomSortingExpressionProvider _customSortingExpressionProvider;
    private readonly ISortExpressionProvider _sortExpressionProvider;
    private readonly IMetadataFacade _metadataFacade;
    private readonly IStrainerOptionsProvider _strainerOptionsProvider;

    public SortingApplier(
        ICustomSortingExpressionProvider customSortingExpressionProvider,
        ISortExpressionProvider sortExpressionProvider,
        IMetadataFacade metadataFacade,
        IStrainerOptionsProvider strainerOptionsProvider)
    {
        _customSortingExpressionProvider = Guard.Against.Null(customSortingExpressionProvider);
        _sortExpressionProvider = Guard.Against.Null(sortExpressionProvider);
        _metadataFacade = Guard.Against.Null(metadataFacade);
        _strainerOptionsProvider = Guard.Against.Null(strainerOptionsProvider);
    }

    public bool TryApplySorting<T>(IList<ISortTerm> sortTerms, IQueryable<T> source, out IQueryable<T> sortedSource)
    {
        Guard.Against.Null(sortTerms);
        Guard.Against.Null(source);

        var options = _strainerOptionsProvider.GetStrainerOptions();
        var isSubsequent = false;
        var isSortingApplied = false;
        sortedSource = source;

        foreach (var sortTerm in sortTerms)
        {
            var metadata = _metadataFacade.GetMetadata<T>(
                isSortableRequired: true,
                isFilterableRequired: false,
                name: sortTerm.Name);

            if (metadata != null)
            {
                var sortExpression = _sortExpressionProvider.GetExpression<T>(metadata.PropertyInfo, sortTerm, isSubsequent);
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
                    if (!_customSortingExpressionProvider.TryGetCustomExpression<T>(sortTerm, isSubsequent, out var sortExpression))
                    {
                        throw new StrainerMethodNotFoundException(
                            sortTerm.Name,
                            $"Property or custom sorting method '{sortTerm.Name}' was not found.");
                    }
                    else
                    {
                        sortedSource = sortedSource.OrderWithSortExpression(sortExpression);
                        isSortingApplied = true;
                    }
                }
                catch (StrainerException) when (!options.ThrowExceptions)
                {
                    return false;
                }
            }

            isSubsequent = true;
        }

        return isSortingApplied;
    }
}
