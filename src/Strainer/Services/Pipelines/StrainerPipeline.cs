using Fluorite.Strainer.Exceptions;
using Fluorite.Strainer.Models;

namespace Fluorite.Strainer.Services.Pipelines;

public class StrainerPipeline : IStrainerPipeline
{
    private readonly IEnumerable<IStrainerPipelineOperation> _operations;
    private readonly IStrainerOptionsProvider _strainerOptionsProvider;

    public StrainerPipeline(
        IEnumerable<IStrainerPipelineOperation> operations,
        IStrainerOptionsProvider strainerOptionsProvider)
    {
        Guard.Against.Null(operations);
        Guard.Against.Null(strainerOptionsProvider);

        _operations = operations;
        _strainerOptionsProvider = strainerOptionsProvider;
    }

    public IQueryable<T> Run<T>(IStrainerModel model, IQueryable<T> source)
    {
        if (model is null)
        {
            throw new ArgumentNullException(nameof(model));
        }

        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        var options = _strainerOptionsProvider.GetStrainerOptions();
        var result = source;

        try
        {
            foreach (var operation in _operations)
            {
                result = operation.Execute(model, result);
            }
        }
        catch (StrainerException) when (!options.ThrowExceptions)
        {
            return source;
        }

        return result;
    }
}
