using Fluorite.Strainer.Exceptions;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services.Filtering;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Pipelines
{
    public class FilterPipelineOperation : IFilterPipelineOperation, IStrainerPipelineOperation
    {
        private readonly ICustomFilteringExpressionProvider _customFilteringExpressionProvider;

        public FilterPipelineOperation(ICustomFilteringExpressionProvider customFilteringExpressionProvider)
        {
            _customFilteringExpressionProvider = customFilteringExpressionProvider;
        }

        public IQueryable<T> Execute<T>(IStrainerModel model, IQueryable<T> source, IStrainerContext context)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var parsedTerms = context.Filter.TermParser.GetParsedTerms(model.Filters);
            if (parsedTerms.Count == 0)
            {
                return source;
            }

            Expression outerExpression = null;
            var parameterExpression = Expression.Parameter(typeof(T), "e");
            foreach (var filterTerm in parsedTerms)
            {
                Expression termExpression = null;
                foreach (var filterTermName in filterTerm.Names)
                {
                    var metadata = context.Metadata.GetMetadata<T>(
                        isSortableRequired: false,
                        isFilterableRequired: true,
                        name: filterTermName);

                    try
                    {
                        if (metadata != null)
                        {
                            termExpression = context.Filter.ExpressionProvider.GetExpression(metadata, filterTerm, parameterExpression, termExpression);
                        }
                        else
                        {
                            if (_customFilteringExpressionProvider.TryGetCustomExpression<T>(filterTerm, filterTermName, out var customExpression))
                            {
                                source = source.Where(customExpression);
                            }
                            else
                            {
                                throw new StrainerMethodNotFoundException(
                                    filterTermName,
                                    $"Property or custom filter method '{filterTermName}' was not found.");
                            }
                        }
                    }
                    catch (StrainerException) when (!context.Options.ThrowExceptions)
                    {
                        return source;
                    }
                }

                if (termExpression == null)
                {
                    continue;
                }

                if (outerExpression == null)
                {
                    outerExpression = termExpression;
                    continue;
                }

                outerExpression = Expression.And(outerExpression, termExpression);
            }

            if (outerExpression == null)
            {
                return source;
            }

            var lambdaExpression = Expression.Lambda<Func<T, bool>>(outerExpression, parameterExpression);

            return source.Where(lambdaExpression);
        }
    }
}
