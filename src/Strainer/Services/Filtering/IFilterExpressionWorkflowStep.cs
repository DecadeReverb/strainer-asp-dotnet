namespace Fluorite.Strainer.Services.Filtering
{
    public interface IFilterExpressionWorkflowStep
    {
        void Execute(FilterExpressionWorkflowContext context);
    }
}
