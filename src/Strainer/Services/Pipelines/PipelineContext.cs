namespace Fluorite.Strainer.Services.Pipelines;

public class PipelineContext : IPipelineContext
{
    public PipelineContext(IStrainerPipelineBuilderFactory strainerPipelineBuilderFactory)
    {
        BuilderFactory = Guard.Against.Null(strainerPipelineBuilderFactory);
    }

    public IStrainerPipelineBuilderFactory BuilderFactory { get; }
}
