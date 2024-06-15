using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Filtering;

public class FilterExpressionWorkflow : IFilterExpressionWorkflow
{
    private readonly IReadOnlyCollection<IFilterExpressionWorkflowStep> _steps;

    public FilterExpressionWorkflow(IReadOnlyCollection<IFilterExpressionWorkflowStep> steps)
    {
        _steps = Guard.Against.Null(steps);
    }

    public Expression Run(FilterExpressionWorkflowContext workflowContext)
    {
        Guard.Against.Null(workflowContext);

        foreach (var step in _steps)
        {
            step.Execute(workflowContext);
        }

        return workflowContext.FinalExpression;
    }
}
