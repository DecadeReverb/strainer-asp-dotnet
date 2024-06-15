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

        var options = _strainerOptionsProvider.GetStrainerOptions();

        if ((context.Term.Operator.IsCaseInsensitive
            || (!context.Term.Operator.IsCaseInsensitive && options.IsCaseInsensitiveForValues))
            && context.PropertyMetadata.PropertyInfo.PropertyType == typeof(string))
        {
            context.PropertyValue = Expression.Call(
                context.PropertyValue,
                typeof(string).GetMethods()
                    .First(m => m.Name == nameof(string.ToUpper) && m.GetParameters().Length == 0));

            context.FinalExpression = Expression.Call(
                context.FinalExpression,
                typeof(string).GetMethods()
                    .First(m => m.Name == nameof(string.ToUpper) && m.GetParameters().Length == 0));
        }
    }
}
