using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Filtering;

/// <summary>
/// Represents a workflow, that when provided with context
/// with produces a filtering expression.
/// </summary>
public interface IFilterExpressionWorkflow
{
    /// <summary>
    /// Launches the workflow with context and produces a filtering expression.
    /// </summary>
    /// <param name="workflowContext">The workflow context holding core information.</param>
    /// <returns>A filtering expression.</returns>
    Expression Run(FilterExpressionWorkflowContext workflowContext);
}
