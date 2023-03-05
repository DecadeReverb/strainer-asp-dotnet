namespace Fluorite.Strainer.Services.Pipelines
{
    public class StrainerPipelineBuilder : IStrainerPipelineBuilder
    {
        private readonly List<IStrainerPipelineOperation> _operations;
        private readonly IFilterPipelineOperation _filterPipelineOperation;
        private readonly ISortPipelineOperation _sortPipelineOperation;
        private readonly IPaginatePipelineOperation _paginatePipelineOperation;

        public StrainerPipelineBuilder(
            IFilterPipelineOperation filterPipelineOperation,
            ISortPipelineOperation sortPipelineOperation,
            IPaginatePipelineOperation paginatePipelineOperation)
        {
            _operations = new ();
            _filterPipelineOperation = filterPipelineOperation;
            _sortPipelineOperation = sortPipelineOperation;
            _paginatePipelineOperation = paginatePipelineOperation;
        }

        public IStrainerPipeline Build()
        {
            return new StrainerPipeline(_operations);
        }

        public IStrainerPipelineBuilder Filter()
        {
            _operations.Add(_filterPipelineOperation);

            return this;
        }

        public IStrainerPipelineBuilder Paginate()
        {
            _operations.Add(_paginatePipelineOperation);

            return this;
        }

        public IStrainerPipelineBuilder Sort()
        {
            _operations.Add(_sortPipelineOperation);

            return this;
        }
    }
}
