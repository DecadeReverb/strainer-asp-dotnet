using Fluorite.Strainer.Exceptions;
using Fluorite.Strainer.Models.Filtering.Operators;

namespace Fluorite.Strainer.Services.Filtering.Steps;

public class ApplyFilterOperatorStep : IApplyFilterOperatorStep
{
    public void Execute(FilterExpressionWorkflowContext context)
    {
        Guard.Against.Null(context);
        Guard.Against.Null(context.FinalExpression);
        Guard.Against.Null(context.PropertyValue);
        Guard.Against.Null(context.Term);
        Guard.Against.Null(context.Term.Operator);
        Guard.Against.Null(context.PropertyMetadata);
        Guard.Against.Null(context.PropertyMetadata.PropertyInfo);
        Guard.Against.Null(context.FilterTermValue);

        var filterOperatorContext = new FilterExpressionContext(context.FinalExpression, context.PropertyValue);

        try
        {
            context.FinalExpression = context.Term.Operator.Expression(filterOperatorContext);
        }
        catch (Exception ex)
        {
            var metadata = context.PropertyMetadata;

            throw new StrainerOperatorException(
                $"Failed to use operator '{context.Term.Operator}' " +
                $"for filter value '{context.FilterTermValue}' on property " +
                $"'{metadata.PropertyInfo.DeclaringType.FullName}.{metadata.PropertyInfo.Name}' " +
                $"of type '{metadata.PropertyInfo.PropertyType.FullName}'\n." +
                "Please ensure that this operator is supported by type " +
                $"'{metadata.PropertyInfo.PropertyType.FullName}'.",
                ex,
                context.Term.Operator,
                metadata.PropertyInfo,
                context.FilterTermValue);
        }
    }
}
