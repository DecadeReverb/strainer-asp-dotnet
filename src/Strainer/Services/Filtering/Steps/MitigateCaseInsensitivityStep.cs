using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Filtering.Steps;

public class MitigateCaseInsensitivityStep : IMitigateCaseInsensitivityStep
{
    private readonly IStrainerOptionsProvider _strainerOptionsProvider;

    public MitigateCaseInsensitivityStep(IStrainerOptionsProvider strainerOptionsProvider)
    {
        _strainerOptionsProvider = Guard.Against.Null(strainerOptionsProvider);
    }

    public void Execute(FilterExpressionWorkflowContext context)
    {
        Guard.Against.Null(context);
        Guard.Against.Null(context.Term);
        Guard.Against.Null(context.Term.Operator);
        Guard.Against.Null(context.PropertyMetadata);
        Guard.Against.Null(context.PropertyMetadata.PropertyInfo);

        var options = _strainerOptionsProvider.GetStrainerOptions();

        if ((context.Term.Operator.IsCaseInsensitive
            || (!context.Term.Operator.IsCaseInsensitive && options.IsCaseInsensitiveForValues))
            && context.PropertyMetadata.PropertyInfo.PropertyType == typeof(string))
        {
            context.PropertyValue = Expression.Call(
                context.PropertyValue,
                typeof(string).GetMethod(nameof(string.ToUpper), Array.Empty<Type>()));

            context.FinalExpression = Expression.Call(
                context.FinalExpression,
                typeof(string).GetMethod(nameof(string.ToUpper), Array.Empty<Type>()));
        }
    }
}
