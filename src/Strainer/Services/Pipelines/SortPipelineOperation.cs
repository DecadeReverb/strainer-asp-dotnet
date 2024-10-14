using Fluorite.Extensions;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services.Sorting;

namespace Fluorite.Strainer.Services.Pipelines;

public class SortPipelineOperation : ISortPipelineOperation, IStrainerPipelineOperation
{
    private readonly ISortingApplier _sortingApplier;
    private readonly ISortTermParser _sortTermParser;
    private readonly ISortExpressionProvider _sortExpressionProvider;

    public SortPipelineOperation(
        ISortingApplier sortingApplier,
        ISortTermParser sortTermParser,
        ISortExpressionProvider sortExpressionProvider)
    {
        _sortingApplier = Guard.Against.Null(sortingApplier);
        _sortTermParser = Guard.Against.Null(sortTermParser);
        _sortExpressionProvider = Guard.Against.Null(sortExpressionProvider);
    }

    public IQueryable<T> Execute<T>(IStrainerModel model, IQueryable<T> source)
    {
        Guard.Against.Null(model);
        Guard.Against.Null(source);

        var parsedTerms = _sortTermParser.GetParsedTerms(model.Sorts);
        var isSortingApplied = _sortingApplier.TryApplySorting(parsedTerms, source, out var sortedSource);
        if (isSortingApplied)
        {
            return sortedSource;
        }

        var defaultSortExpression = _sortExpressionProvider.GetDefaultExpression<T>();
        if (defaultSortExpression != null)
        {
            return source.OrderWithSortExpression(defaultSortExpression);
        }
        else
        {
            return source;
        }
    }
}
