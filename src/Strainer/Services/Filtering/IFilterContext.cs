namespace Fluorite.Strainer.Services.Filtering
{
    public interface IFilterContext
    {
        IFilterExpressionProvider ExpressionProvider { get; }
        IFilterOperatorMapper OperatorMapper { get; }
        IFilterOperatorParser OperatorParser { get; }
        IFilterOperatorValidator OperatorValidator { get; }
        IFilterTermParser TermParser { get; }
    }
}
