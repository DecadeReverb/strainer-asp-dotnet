using Fluorite.Strainer.Models;

namespace Fluorite.Strainer.Services.Pagination;

public class PageSizeEvaluator : IPageSizeEvaluator
{
    private readonly IStrainerOptionsProvider _strainerOptionsProvider;

    public PageSizeEvaluator(IStrainerOptionsProvider strainerOptionsProvider)
    {
        _strainerOptionsProvider = strainerOptionsProvider ?? throw new ArgumentNullException(nameof(strainerOptionsProvider));
    }

    public int Evaluate(IStrainerModel model)
    {
        if (model is null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        var options = _strainerOptionsProvider.GetStrainerOptions();
        var pageSize = model.PageSize ?? options.DefaultPageSize;

        return options.MaxPageSize > 0
            ? Math.Min(pageSize, options.MaxPageSize)
            : pageSize;
    }
}
