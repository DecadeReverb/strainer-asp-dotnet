using Fluorite.Strainer.Models;
using System.Linq;

namespace Fluorite.Strainer.Services.Pipelines
{
    public interface IStrainerPipeline
    {
        IQueryable<T> Run<T>(IStrainerModel model, IQueryable<T> source, IStrainerContext context);
    }
}