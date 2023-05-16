using Fluorite.Strainer.Services.Conversion;

namespace Fluorite.Strainer.Services.Filtering.Steps
{
    public class ConvertFilterValueToStringStep : IConvertFilterValueToStringStep
    {
        private readonly IStringValueConverter _stringValueConverter;

        public ConvertFilterValueToStringStep(IStringValueConverter stringValueConverter)
        {
            _stringValueConverter = stringValueConverter ?? throw new ArgumentNullException(nameof(stringValueConverter));
        }

        public void Execute(FilterExpressionWorkflowContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Term.Operator.IsStringBased)
            {
                return;
            }

            var canConvertFromString =
                   context.PropertyMetadata.PropertyInfo.PropertyType != typeof(string)
                && context.TypeConverter.CanConvertFrom(typeof(string));

            if (canConvertFromString)
            {
                context.FilterTermConstant = _stringValueConverter.Convert(
                    context.FilterTermValue,
                    context.PropertyMetadata.PropertyInfo.PropertyType,
                    context.TypeConverter);
            }
        }
    }
}
