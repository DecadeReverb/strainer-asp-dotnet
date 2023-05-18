using Fluorite.Strainer.Models;

namespace Fluorite.Strainer.Services.Pipelines
{
    public interface IStrainerPipelineOperation
    {
        IQueryable<T> Execute<T>(IStrainerModel model, IQueryable<T> source);
    }
}
