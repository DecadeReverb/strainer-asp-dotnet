using Fluorite.Strainer.Models;

namespace Fluorite.Strainer.Services.Pipelines
{
    public class PaginatePipelineOperation : IStrainerPipelineOperation
    {

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

            var page = model.Page ?? context.Options.DefaultPageNumber;
            var pageSize = model.PageSize ?? context.Options.DefaultPageSize;
            var maxPageSize = context.Options.MaxPageSize > 0
                ? context.Options.MaxPageSize
                : pageSize;

            if (page > 1)
            {
                source = source.Skip((page - 1) * pageSize);
            }

            if (pageSize > 0)
            {
                source = source.Take(Math.Min(pageSize, maxPageSize));
            }

            return source;
        }
    }
}
