namespace Fluorite.Strainer.Services.Pipelines
{
    public interface IPipelineContext
    {
        IStrainerPipelineBuilderFactory BuilderFactory { get; }
    }
}
