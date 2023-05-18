using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Filtering
{
    public class FilterExpressionWorkflow : IFilterExpressionWorkflow
    {
        private readonly IReadOnlyCollection<IFilterExpressionWorkflowStep> _steps;

        public FilterExpressionWorkflow(IReadOnlyCollection<IFilterExpressionWorkflowStep> steps)
        {
            _steps = steps ?? throw new ArgumentNullException(nameof(steps));
        }

        public Expression Run(FilterExpressionWorkflowContext workflowContext)
        {
            if (workflowContext is null)
            {
                throw new ArgumentNullException(nameof(workflowContext));
            }

            foreach (var step in _steps)
            {
                step.Execute(workflowContext);
            }

            return workflowContext.FinalExpression;
        }
    }
}
