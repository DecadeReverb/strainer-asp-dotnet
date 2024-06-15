namespace Fluorite.Strainer.Services.Pipelines;

public interface IStrainerPipelineBuilder
{
    IStrainerPipeline Build();

    IStrainerPipelineBuilder Filter();

    IStrainerPipelineBuilder Paginate();

    IStrainerPipelineBuilder Sort();
}
