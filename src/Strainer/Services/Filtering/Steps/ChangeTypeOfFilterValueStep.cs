using Fluorite.Strainer.Services.Conversion;

namespace Fluorite.Strainer.Services.Filtering.Steps
{
    public class ChangeTypeOfFilterValueStep : IChangeTypeOfFilterValueStep
    {
        private readonly ITypeChanger _typeChanger;

        public ChangeTypeOfFilterValueStep(ITypeChanger typeChanger)
        {
            _typeChanger = typeChanger ?? throw new ArgumentNullException(nameof(typeChanger));
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

            if (!canConvertFromString)
            {
                context.FilterTermConstant = _typeChanger.ChangeType(
                    context.FilterTermValue,
                    context.PropertyMetadata.PropertyInfo.PropertyType);
            }
        }
    }
}
