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
            if (operations is null)
            {
                throw new ArgumentNullException(nameof(operations));
            }

            _operations = operations;
        }

        public IQueryable<T> Run<T>(IStrainerModel model, IQueryable<T> source, IStrainerContext context)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

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
