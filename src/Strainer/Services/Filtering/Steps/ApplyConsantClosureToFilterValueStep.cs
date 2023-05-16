using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Filtering.Steps
{
    public class ApplyConsantClosureToFilterValueStep : IApplyConsantClosureToFilterValueStep
    {
        public void Execute(FilterExpressionWorkflowContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var constantClosureType = context.Term.Operator.IsStringBased
                ? typeof(string)
                : context.PropertyMetadata.PropertyInfo.PropertyType;

            context.FinalExpression = GetClosureOverConstant(
                context.FilterTermConstant,
                constantClosureType);
        }

        // Workaround to ensure that the filter value gets passed as a parameter in generated SQL from EF Core
        // See https://github.com/aspnet/EntityFrameworkCore/issues/3361
        // Expression.Constant passed the target type to allow Nullable comparison
        // See http://bradwilson.typepad.com/blog/2008/07/creating-nullab.html
        private Expression GetClosureOverConstant<T>(T constant, Type targetType)
        {
            return Expression.Constant(constant, targetType);
        }
    }
}
