using Fluorite.Extensions;
using Fluorite.Strainer.Exceptions;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Sorting;

namespace Fluorite.Strainer.Services.Pipelines
{
    public class SortPipelineOperation : IStrainerPipelineOperation
    {
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

            var parsedTerms = context.Sorting.TermParser.GetParsedTerms(model.Sorts);
            var isSubsequent = false;
            var sortingPerformed = false;

            foreach (var sortTerm in parsedTerms)
            {
                var metadata = context.Metadata.GetMetadata<T>(
                    isSortableRequired: true,
                    isFilterableRequired: false,
                    name: sortTerm.Name);

                if (metadata != null)
                {
                    var sortExpression = context.Sorting.ExpressionProvider.GetExpression<T>(metadata.PropertyInfo, sortTerm, isSubsequent);
                    if (sortExpression != null)
                    {
                        source = source.OrderWithSortExpression(sortExpression);
                        sortingPerformed = true;
                    }
                }
                else
                {
                    try
                    {
                        if (context.CustomMethods.GetCustomSortMethods().TryGetValue(typeof(T), out var customSortMethods)
                            && customSortMethods.TryGetValue(sortTerm.Name, out var customMethod))
                        {
                            source = (customMethod as ICustomSortMethod<T>).Function(source, sortTerm.IsDescending, isSubsequent);
                            sortingPerformed = true;
                        }
                        else
                        {
                            throw new StrainerMethodNotFoundException(
                                sortTerm.Name,
                                $"Property or custom sorting method '{sortTerm.Name}' was not found.");
                        }
                    }
                    catch (StrainerException) when (!context.Options.ThrowExceptions)
                    {
                        return source;
                    }
                }

                isSubsequent = true;
            }

            if (!sortingPerformed)
            {
                var defaultSortExpression = context.Sorting.ExpressionProvider.GetDefaultExpression<T>();
                if (defaultSortExpression != null)
                {
                    source = source.OrderWithSortExpression(defaultSortExpression);
                }
            }

            return source;
        }
    }
}
