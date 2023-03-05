using Fluorite.Strainer.Models;

namespace Fluorite.Strainer.Services.Pagination
{
    public interface IPageNumberEvaluator
    {
        int Evaluate(IStrainerModel model);
    }
}
