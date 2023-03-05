using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services.Pagination;

namespace Fluorite.Strainer.Services.Pipelines
{
    public class PaginatePipelineOperation : IPaginatePipelineOperation, IStrainerPipelineOperation
    {
        private readonly IPageNumberEvaluator _pageNumberEvaluator;

        public PaginatePipelineOperation(
            IPageNumberEvaluator pageNumberEvaluator)
        {
            _pageNumberEvaluator = pageNumberEvaluator;
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

            var page = _pageNumberEvaluator.Evaluate(model);
            var pageSize = model.PageSize ?? context.Options.DefaultPageSize;
            var maxPageSize = context.Options.MaxPageSize > 0
                ? context.Options.MaxPageSize
                : pageSize;
            var finalPageSize = Math.Min(pageSize, maxPageSize);

            if (page > 1)
            {
                source = source.Skip((page - 1) * pageSize);
            }

            if (pageSize > 0)
            {
                source = source.Take(finalPageSize);
            }

            return source;
        }
    }
}
