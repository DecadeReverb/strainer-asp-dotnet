using System.Collections.Generic;

namespace Fluorite.Strainer.Services.Pipelines
{
    public class StrainerPipelineBuilder : IStrainerPipelineBuilder
    {
        private readonly List<IStrainerPipelineOperation> _operations;

        public StrainerPipelineBuilder()
        {
            _operations = new ();
        }

        public IStrainerPipeline Build()
        {
            return new StrainerPipeline(_operations);
        }

        public IStrainerPipelineBuilder Filter()
        {
            _operations.Add(new FilterPipelineOperation());

            return this;
        }

        public IStrainerPipelineBuilder Paginate()
        {
            _operations.Add(new PaginatePipelineOperation());

            return this;
        }

        public IStrainerPipelineBuilder Sort()
        {
            _operations.Add(new SortPipelineOperation());

            return this;
        }
    }
}
