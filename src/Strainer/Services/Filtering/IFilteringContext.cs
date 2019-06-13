namespace Fluorite.Strainer.Services.Filtering
{
    public interface IFilteringContext
    {
        IFilterExpressionMapper ExpressionMapper { get; }
        IFilterOperatorParser OperatorParser { get; }
        IFilterOperatorProvider OperatorProvider { get; }
        IFilterOperatorValidator OperatorValidator { get; }
        IFilterTermParser TermParser { get; }
    }
}
