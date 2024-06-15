using Fluorite.Strainer.Models;

namespace Fluorite.Strainer.Services.Pagination;

public class PageNumberEvaluator : IPageNumberEvaluator
{
    private readonly IStrainerOptionsProvider _strainerOptionsProvider;

    public PageNumberEvaluator(IStrainerOptionsProvider strainerOptionsProvider)
    {
        _strainerOptionsProvider = Guard.Against.Null(strainerOptionsProvider);
    }

    public int Evaluate(IStrainerModel model)
    {
        Guard.Against.Null(model);

        var options = _strainerOptionsProvider.GetStrainerOptions();

        return model.Page ?? options.DefaultPageNumber;
    }
}
