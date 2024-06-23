using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Filtering;

/// <summary>
/// Represents a workflow, that when provided with context produces a filtering expression.
/// </summary>
public class FilterExpressionWorkflow : IFilterExpressionWorkflow
{
    private readonly IReadOnlyCollection<IFilterExpressionWorkflowStep> _steps;

    /// <summary>
    /// Initializes a new instance of the <see cref="FilterExpressionWorkflow"/> class.
    /// </summary>
    /// <param name="steps">
    /// The workflow steps being its basis.
    /// </param>
    public FilterExpressionWorkflow(IReadOnlyCollection<IFilterExpressionWorkflowStep> steps)
    {
        _steps = Guard.Against.Null(steps);
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="workflowContext"/> is <see langword="null"/>.
    /// </exception>
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
