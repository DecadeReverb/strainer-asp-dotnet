using Fluorite.Strainer.Models;

namespace Fluorite.Strainer.Services.Pipelines
{
    public interface IPageSizeEvaluator
    {
        int Evaluate(IStrainerModel model);
    }
}
