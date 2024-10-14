using Fluorite.Strainer.Services.Conversion;

namespace Fluorite.Strainer.Services.Filtering.Steps;

public class ConvertFilterValueToStringStep : IConvertFilterValueToStringStep
{
    private readonly IStringValueConverter _stringValueConverter;

    public ConvertFilterValueToStringStep(IStringValueConverter stringValueConverter)
    {
        _stringValueConverter = Guard.Against.Null(stringValueConverter);
    }

    public void Execute(FilterExpressionWorkflowContext context)
    {
        Guard.Against.Null(context);

        if (context.Term.Operator.IsStringBased)
        {
            return;
        }

        var canConvertFromString =
               context.PropertyMetadata.PropertyInfo.PropertyType != typeof(string)
            && context.TypeConverter.CanConvertFrom(typeof(string));

        if (canConvertFromString == true)
        {
            context.FilterTermConstant = _stringValueConverter.Convert(
                context.FilterTermValue,
                context.PropertyMetadata.PropertyInfo.PropertyType,
                context.TypeConverter);
        }
    }
}
