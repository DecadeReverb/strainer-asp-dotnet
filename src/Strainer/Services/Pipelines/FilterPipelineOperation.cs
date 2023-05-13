using Fluorite.Strainer.Exceptions;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services.Filtering;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Pipelines
{
    public class FilterPipelineOperation : IFilterPipelineOperation, IStrainerPipelineOperation
    {
        private readonly ICustomFilteringApplier _customFilteringApplier;

        public FilterPipelineOperation(ICustomFilteringApplier customFilteringApplier)
        {
            _customFilteringApplier = customFilteringApplier;
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
                            var isCustomFilteringApplied = _customFilteringApplier.TryApplyCustomFiltering(source, filterTerm, filterTermName, out var filteredSource);
                            if (isCustomFilteringApplied)
                            {
                                source = filteredSource;
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
