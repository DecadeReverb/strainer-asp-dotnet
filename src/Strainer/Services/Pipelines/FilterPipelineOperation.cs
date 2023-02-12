using Fluorite.Strainer.Exceptions;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Filtering;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Pipelines
{
    public class FilterPipelineOperation : IStrainerPipelineOperation
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

            var parsedTerms = context.Filter.TermParser.GetParsedTerms(model.Filters);
            if (parsedTerms.Count == 0)
            {
                return source;
            }

            Expression outerExpression = null;
            var parameterExpression = Expression.Parameter(typeof(T), "e");
            foreach (var filterTerm in parsedTerms)
            {
                Expression innerExpression = null;
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
                            innerExpression = context.Filter.ExpressionProvider.GetExpression(metadata, filterTerm, parameterExpression, innerExpression);
                        }
                        else
                        {
                            if (context.CustomMethods.GetCustomFilterMethods().TryGetValue(typeof(T), out var customFilterMethods)
                                && customFilterMethods.TryGetValue(filterTermName, out var customMethod))
                            {
                                source = (customMethod as ICustomFilterMethod<T>).Function(source, filterTerm.Operator?.Symbol);
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

                if (outerExpression == null)
                {
                    outerExpression = innerExpression;
                    continue;
                }

                if (innerExpression == null)
                {
                    continue;
                }

                outerExpression = Expression.And(outerExpression, innerExpression);
            }

            if (outerExpression == null)
            {
                return source;
            }

            return source.Where(Expression.Lambda<Func<T, bool>>(outerExpression, parameterExpression));
        }
    }
}
