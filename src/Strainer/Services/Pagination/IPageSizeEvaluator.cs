using Fluorite.Strainer.Models;

namespace Fluorite.Strainer.Services.Pagination;

public interface IPageSizeEvaluator
{
    int Evaluate(IStrainerModel model);
}
