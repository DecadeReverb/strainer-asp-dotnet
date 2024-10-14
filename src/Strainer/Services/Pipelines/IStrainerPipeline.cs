using Fluorite.Strainer.Models;

namespace Fluorite.Strainer.Services.Pipelines;

public interface IStrainerPipeline
{
    IQueryable<T> Run<T>(IStrainerModel model, IQueryable<T> source);
}