using Fluorite.Strainer.Services.Filtering.Steps;

namespace Fluorite.Strainer.Services.Filtering;

public class FilterExpressionWorkflowBuilder : IFilterExpressionWorkflowBuilder
{
    private readonly IConvertPropertyValueToStringStep _convertPropertyValueToStringStep;
    private readonly IConvertFilterValueToStringStep _convertFilterValueToStringStep;
    private readonly IChangeTypeOfFilterValueStep _changeTypeOfFilterValueStep;
    private readonly IApplyConsantClosureToFilterValueStep _applyConsantClosureToFilterValueStep;
    private readonly IMitigateCaseInsensitivityStep _mitigateCaseInsensitivityStep;
    private readonly IApplyFilterOperatorStep _applyFilterOperatorStep;

    public FilterExpressionWorkflowBuilder(
        IConvertPropertyValueToStringStep convertPropertyValueToStringStep,
        IConvertFilterValueToStringStep convertFilterValueToStringStep,
        IChangeTypeOfFilterValueStep changeTypeOfFilterValueStep,
        IApplyConsantClosureToFilterValueStep applyConsantClosureToFilterValueStep,
        IMitigateCaseInsensitivityStep mitigateCaseInsensitivityStep,
        IApplyFilterOperatorStep applyFilterOperatorStep)
    {
        _convertPropertyValueToStringStep = Guard.Against.Null(convertPropertyValueToStringStep);
        _convertFilterValueToStringStep = Guard.Against.Null(convertFilterValueToStringStep);
        _changeTypeOfFilterValueStep = Guard.Against.Null(changeTypeOfFilterValueStep);
        _applyConsantClosureToFilterValueStep = Guard.Against.Null(applyConsantClosureToFilterValueStep);
        _mitigateCaseInsensitivityStep = Guard.Against.Null(mitigateCaseInsensitivityStep);
        _applyFilterOperatorStep = Guard.Against.Null(applyFilterOperatorStep);
    }

    public IFilterExpressionWorkflow BuildDefaultWorkflow()
    {
        var steps = new List<IFilterExpressionWorkflowStep>
        {
            _convertPropertyValueToStringStep,
            _convertFilterValueToStringStep,
            _changeTypeOfFilterValueStep,
            _applyConsantClosureToFilterValueStep,
            _mitigateCaseInsensitivityStep,
            _applyFilterOperatorStep,
        };

        return new FilterExpressionWorkflow(steps);
    }
}
