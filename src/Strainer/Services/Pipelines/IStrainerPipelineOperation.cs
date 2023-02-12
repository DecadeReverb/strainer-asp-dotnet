using Fluorite.Strainer.Models;
using System.Linq;

namespace Fluorite.Strainer.Services.Pipelines
{
    public interface IStrainerPipelineOperation
    {
        IQueryable<T> Execute<T>(IStrainerModel model, IQueryable<T> source, IStrainerContext context);
    }
}
