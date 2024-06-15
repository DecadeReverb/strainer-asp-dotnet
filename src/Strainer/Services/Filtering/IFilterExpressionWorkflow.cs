using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Filtering;

public interface IFilterExpressionWorkflow
{
    Expression Run(FilterExpressionWorkflowContext workflowContext);
}
