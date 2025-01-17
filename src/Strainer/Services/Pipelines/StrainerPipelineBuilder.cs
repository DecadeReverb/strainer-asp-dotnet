﻿namespace Fluorite.Strainer.Services.Pipelines;

public class StrainerPipelineBuilder : IStrainerPipelineBuilder
{
    private readonly List<IStrainerPipelineOperation> _operations;
    private readonly IFilterPipelineOperation _filterPipelineOperation;
    private readonly ISortPipelineOperation _sortPipelineOperation;
    private readonly IPaginatePipelineOperation _paginatePipelineOperation;
    private readonly IStrainerOptionsProvider _strainerOptionsProvider;

    public StrainerPipelineBuilder(
        IFilterPipelineOperation filterPipelineOperation,
        ISortPipelineOperation sortPipelineOperation,
        IPaginatePipelineOperation paginatePipelineOperation,
        IStrainerOptionsProvider strainerOptionsProvider)
    {
        _operations = new();
        _filterPipelineOperation = Guard.Against.Null(filterPipelineOperation);
        _sortPipelineOperation = Guard.Against.Null(sortPipelineOperation);
        _paginatePipelineOperation = Guard.Against.Null(paginatePipelineOperation);
        _strainerOptionsProvider = Guard.Against.Null(strainerOptionsProvider);
    }

    public IStrainerPipeline Build()
    {
        return new StrainerPipeline(_operations, _strainerOptionsProvider);
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
