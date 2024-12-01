using Fluorite.Strainer.Services.Conversion;

namespace Fluorite.Strainer.Services.Filtering.Steps;

public class ChangeTypeOfFilterValueStep : IChangeTypeOfFilterValueStep
{
    private readonly ITypeChanger _typeChanger;

    public ChangeTypeOfFilterValueStep(ITypeChanger typeChanger)
    {
        _typeChanger = Guard.Against.Null(typeChanger);
    }

    public void Execute(FilterExpressionWorkflowContext context)
    {
        Guard.Against.Null(context);
        Guard.Against.Null(context.Term);
        Guard.Against.Null(context.Term.Operator);
        Guard.Against.Null(context.PropertyMetadata);
        Guard.Against.Null(context.PropertyMetadata.PropertyInfo);
        Guard.Against.Null(context.TypeConverter);
        Guard.Against.Null(context.FilterTermValue);

        if (context.Term.Operator.IsStringBased)
        {
            return;
        }

        var canConvertFromString =
               context.PropertyMetadata.PropertyInfo.PropertyType != typeof(string)
            && context.TypeConverter.CanConvertFrom(typeof(string));

        if (canConvertFromString == false)
        {
            context.FilterTermConstant = _typeChanger.ChangeType(
                context.FilterTermValue,
                context.PropertyMetadata.PropertyInfo.PropertyType);
        }
    }
}
