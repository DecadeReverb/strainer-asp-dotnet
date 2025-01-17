﻿using Fluorite.Strainer.Exceptions;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Metadata;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Pipelines;

public class FilterPipelineOperation : IFilterPipelineOperation, IStrainerPipelineOperation
{
    private readonly ICustomFilteringExpressionProvider _customFilteringExpressionProvider;
    private readonly IFilterExpressionProvider _filterExpressionProvider;
    private readonly IFilterTermParser _filterTermParser;
    private readonly IMetadataFacade _metadataFacade;
    private readonly IStrainerOptionsProvider _strainerOptionsProvider;

    public FilterPipelineOperation(
        ICustomFilteringExpressionProvider customFilteringExpressionProvider,
        IFilterExpressionProvider filterExpressionProvider,
        IFilterTermParser filterTermParser,
        IMetadataFacade metadataFacade,
        IStrainerOptionsProvider strainerOptionsProvider)
    {
        _customFilteringExpressionProvider = Guard.Against.Null(customFilteringExpressionProvider);
        _filterExpressionProvider = Guard.Against.Null(filterExpressionProvider);
        _filterTermParser = Guard.Against.Null(filterTermParser);
        _metadataFacade = Guard.Against.Null(metadataFacade);
        _strainerOptionsProvider = Guard.Against.Null(strainerOptionsProvider);
    }

    public IQueryable<T> Execute<T>(IStrainerModel model, IQueryable<T> source)
    {
        Guard.Against.Null(model);
        Guard.Against.Null(source);

        var options = _strainerOptionsProvider.GetStrainerOptions();
        var parsedTerms = _filterTermParser.GetParsedTerms(model.Filters);
        if (parsedTerms.Count == 0)
        {
            return source;
        }

        Expression? outerExpression = null;
        var parameterExpression = Expression.Parameter(typeof(T), "e");
        foreach (var filterTerm in parsedTerms)
        {
            Expression? termExpression = null;
            foreach (var filterTermName in filterTerm.Names)
            {
                var metadata = _metadataFacade.GetMetadata<T>(
                    isSortableRequired: false,
                    isFilterableRequired: true,
                    name: filterTermName);

                try
                {
                    if (metadata is not null)
                    {
                        termExpression = _filterExpressionProvider.GetExpression(metadata, filterTerm, parameterExpression, termExpression);
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
                catch (StrainerException) when (!options.ThrowExceptions)
                {
                    return source;
                }
            }

            if (termExpression is not null)
            {
                if (outerExpression is null)
                {
                    outerExpression = termExpression;
                }
                else
                {
                    outerExpression = Expression.And(outerExpression, termExpression);
                }
            }
        }

        if (outerExpression is null)
        {
            return source;
        }

        var lambdaExpression = Expression.Lambda<Func<T, bool>>(outerExpression, parameterExpression);

        return source.Where(lambdaExpression);
    }
}
