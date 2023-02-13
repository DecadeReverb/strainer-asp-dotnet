using Fluorite.Strainer.Exceptions;
using Fluorite.Strainer.Models;

namespace Fluorite.Strainer.Services.Pipelines
{
    public class StrainerPipeline : IStrainerPipeline
    {
        private readonly IEnumerable<IStrainerPipelineOperation> _operations;

        public StrainerPipeline(
            IEnumerable<IStrainerPipelineOperation> operations)
        {
            _operations = operations;
        }

        public IQueryable<T> Run<T>(IStrainerModel model, IQueryable<T> source, IStrainerContext context)
        {
            var result = source;

            try
            {
                foreach (var operation in _operations)
                {
                    result = operation.Execute(model, result, context);
                }
            }
            catch (StrainerException) when (!context.Options.ThrowExceptions)
            {
                return source;
            }

            return result;
        }
    }
}
