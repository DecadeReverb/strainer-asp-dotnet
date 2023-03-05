using Fluorite.Extensions;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services.Sorting;

namespace Fluorite.Strainer.Services.Pipelines
{
    public class SortPipelineOperation : ISortPipelineOperation, IStrainerPipelineOperation
    {
        private readonly ISortingApplier _sortingApplier;

        public SortPipelineOperation(ISortingApplier sortingApplier)
        {
            _sortingApplier = sortingApplier;
        }

        public IQueryable<T> Execute<T>(IStrainerModel model, IQueryable<T> source, IStrainerContext context)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var parsedTerms = context.Sorting.TermParser.GetParsedTerms(model.Sorts);
            var isSortingApplied = _sortingApplier.TryApplySorting(context, parsedTerms, source, out var sortedSource);
            if (isSortingApplied)
            {
                return sortedSource;
            }

            var defaultSortExpression = context.Sorting.ExpressionProvider.GetDefaultExpression<T>();
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
}
