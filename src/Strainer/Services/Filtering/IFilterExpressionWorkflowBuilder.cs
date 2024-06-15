namespace Fluorite.Strainer.Services.Filtering;

/// <summary>
/// Provides filter workflow building capabilites.
/// </summary>
public interface IFilterExpressionWorkflowBuilder
{
    /// <summary>
    /// Creates a default filtering expression workflow.
    /// </summary>
    /// <returns>An instance of <see cref="IFilterExpressionWorkflow"/>.</returns>
    IFilterExpressionWorkflow BuildDefaultWorkflow();
}
