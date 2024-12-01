using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Filtering.Steps;

public class ApplyConsantClosureToFilterValueStep : IApplyConsantClosureToFilterValueStep
{
    public void Execute(FilterExpressionWorkflowContext context)
    {
        Guard.Against.Null(context);
        Guard.Against.Null(context.Term);
        Guard.Against.Null(context.Term.Operator);
        Guard.Against.Null(context.PropertyMetadata);
        Guard.Against.Null(context.PropertyMetadata.PropertyInfo);
        Guard.Against.Null(context.FilterTermConstant);

        var constantClosureType = context.Term.Operator.IsStringBased
            ? typeof(string)
            : context.PropertyMetadata.PropertyInfo.PropertyType;

        if (context.FilterTermConstant.GetType() != constantClosureType)
        {
            throw new InvalidOperationException(
                "Cannot get a closure over constant using wrong type. " +
                $"Give value is of {context.FilterTermConstant.GetType().Name} type while expected target type " +
                $"is of {constantClosureType.Name} type.");
        }

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
