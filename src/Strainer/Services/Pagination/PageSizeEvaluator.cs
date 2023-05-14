using Fluorite.Strainer.Models;

namespace Fluorite.Strainer.Services.Pagination
{
    public class PageSizeEvaluator : IPageSizeEvaluator
    {
        private readonly IStrainerOptionsProvider _strainerOptionsProvider;

        public PageSizeEvaluator(IStrainerOptionsProvider strainerOptionsProvider)
        {
            _strainerOptionsProvider = strainerOptionsProvider;
        }

        public int Evaluate(IStrainerModel model)
        {
            var options = _strainerOptionsProvider.GetStrainerOptions();
            var pageSize = model.PageSize ?? options.DefaultPageSize;
            var maxPageSize = options.MaxPageSize > 0
                ? options.MaxPageSize
                : pageSize;

            return Math.Min(pageSize, maxPageSize);
        }
    }
}
