using Fluorite.Strainer.Exceptions;
using Fluorite.Strainer.Models.Filtering.Operators;

namespace Fluorite.Strainer.Services.Filtering.Steps;

public class ApplyFilterOperatorStep : IApplyFilterOperatorStep
{
    public void Execute(FilterExpressionWorkflowContext context)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var filterOperatorContext = new FilterExpressionContext(context.FinalExpression, context.PropertyValue);

        try
        {
            context.FinalExpression = context.Term.Operator.Expression(filterOperatorContext);
        }
        catch (Exception ex)
        {
            var metadata = context.PropertyMetadata;

            throw new StrainerUnsupportedOperatorException(
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
