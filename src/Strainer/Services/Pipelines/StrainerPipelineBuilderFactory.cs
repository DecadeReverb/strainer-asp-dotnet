namespace Fluorite.Strainer.Services.Pipelines
{
    public class StrainerPipelineBuilderFactory : IStrainerPipelineBuilderFactory
    {
        private readonly IFilterPipelineOperation _filterPipelineOperation;
        private readonly ISortPipelineOperation _sortPipelineOperation;
        private readonly IPaginatePipelineOperation _paginatePipelineOperation;

        public StrainerPipelineBuilderFactory(
            IFilterPipelineOperation filterPipelineOperation,
            ISortPipelineOperation sortPipelineOperation,
            IPaginatePipelineOperation paginatePipelineOperation)
        {
            _filterPipelineOperation = filterPipelineOperation;
            _sortPipelineOperation = sortPipelineOperation;
            _paginatePipelineOperation = paginatePipelineOperation;
        }

        public IStrainerPipelineBuilder CreateBuilder()
        {
            return new StrainerPipelineBuilder(
                _filterPipelineOperation,
                _sortPipelineOperation,
                _paginatePipelineOperation);
        }
    }
}
