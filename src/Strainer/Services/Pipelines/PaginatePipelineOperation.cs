using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services.Pagination;

namespace Fluorite.Strainer.Services.Pipelines
{
    public class PaginatePipelineOperation : IPaginatePipelineOperation, IStrainerPipelineOperation
    {
        private readonly IPageNumberEvaluator _pageNumberEvaluator;
        private readonly IPageSizeEvaluator _pageSizeEvaluator;

        public PaginatePipelineOperation(
            IPageNumberEvaluator pageNumberEvaluator,
            IPageSizeEvaluator pageSizeEvaluator)
        {
            _pageNumberEvaluator = pageNumberEvaluator;
            _pageSizeEvaluator = pageSizeEvaluator;
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

            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var page = _pageNumberEvaluator.Evaluate(model);
            var pageSize = _pageSizeEvaluator.Evaluate(model);

            if (page > 1)
            {
                source = source.Skip((page - 1) * pageSize);
            }

            if (pageSize > 0)
            {
                source = source.Take(pageSize);
            }

            return source;
        }
    }
}
