using Fluorite.Strainer.Models.Filtering.Terms;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Services.Conversion;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Filtering;

public class FilterExpressionProvider : IFilterExpressionProvider
{
    private readonly ITypeConverterProvider _typeConverterProvider;
    private readonly IFilterExpressionWorkflowBuilder _filterExpressionWorkflowBuilder;

    public FilterExpressionProvider(
        ITypeConverterProvider typeConverterProvider,
        IFilterExpressionWorkflowBuilder filterExpressionWorkflowBuilder)
    {
        _typeConverterProvider = Guard.Against.Null(typeConverterProvider);
        _filterExpressionWorkflowBuilder = Guard.Against.Null(filterExpressionWorkflowBuilder);
    }

    public Expression GetExpression(
        IPropertyMetadata metadata,
        IFilterTerm filterTerm,
        ParameterExpression parameterExpression,
        Expression innerExpression)
    {
        Guard.Against.Null(metadata);
        Guard.Against.Null(filterTerm);
        Guard.Against.Null(parameterExpression);

        if (filterTerm.Values == null || !filterTerm.Values.Any())
        {
            return null;
        }

        var nameParts = metadata.Name.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
        if (!nameParts.Any())
        {
            throw new ArgumentException("Metadata name must not be empty.", nameof(metadata));
        }

        MemberExpression propertyExpresssion = null;

        foreach (var part in nameParts)
        {
            propertyExpresssion = Expression.PropertyOrField((Expression)propertyExpresssion ?? parameterExpression, part);
        }

        return CreateInnerExpression(
            metadata,
            filterTerm,
            innerExpression,
            propertyExpresssion);
    }

    private Expression CreateInnerExpression(
        IPropertyMetadata metadata,
        IFilterTerm filterTerm,
        Expression innerExpression,
        MemberExpression propertyExpression)
    {
        var typeConverter = _typeConverterProvider.GetTypeConverter(metadata.PropertyInfo.PropertyType);
        var workflow = _filterExpressionWorkflowBuilder.BuildDefaultWorkflow();

        foreach (var filterTermValue in filterTerm.Values)
        {
            var context = new FilterExpressionWorkflowContext
            {
                FilterTermConstant = filterTermValue,
                FilterTermValue = filterTermValue,
                FinalExpression = null,
                PropertyMetadata = metadata,
                PropertyValue = propertyExpression,
                Term = filterTerm,
                TypeConverter = typeConverter,
            };

            var expression = workflow.Run(context);

            if (innerExpression == null)
            {
                innerExpression = expression;
            }
            else
            {
                innerExpression = Expression.Or(innerExpression, expression);
            }
        }

        return innerExpression;
    }
}
