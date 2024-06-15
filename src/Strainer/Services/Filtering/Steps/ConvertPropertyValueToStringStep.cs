using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Filtering.Steps;

public class ConvertPropertyValueToStringStep : IConvertPropertyValueToStringStep
{
    public void Execute(FilterExpressionWorkflowContext context)
    {
        Guard.Against.Null(context);

        if (context.Term.Operator.IsStringBased)
        {
            if (context.PropertyMetadata.PropertyInfo.PropertyType != typeof(string))
            {
                context.PropertyValue = Expression.Call(
                    context.PropertyValue,
                    typeof(object).GetMethod(nameof(object.ToString)));
            }
        }
    }
}
