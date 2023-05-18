namespace Fluorite.Strainer.Services.Pipelines
{
    public class PipelineContext : IPipelineContext
    {
        public PipelineContext(IStrainerPipelineBuilderFactory strainerPipelineBuilderFactory)
        {
            if (strainerPipelineBuilderFactory == null)
            {
                throw new ArgumentException(nameof(strainerPipelineBuilderFactory));
            }

            BuilderFactory = strainerPipelineBuilderFactory;
        }

        public IStrainerPipelineBuilderFactory BuilderFactory { get; }
    }
}
