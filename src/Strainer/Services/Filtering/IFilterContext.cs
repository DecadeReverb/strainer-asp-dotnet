namespace Fluorite.Strainer.Services.Filtering
{
    public interface IFilterContext
    {
        IFilterExpressionProvider ExpressionProvider { get; }

        IFilterOperatorDictionary OperatorDictionary { get; }

        IFilterOperatorParser OperatorParser { get; }

        IFilterOperatorValidator OperatorValidator { get; }

        IFilterTermParser TermParser { get; }
    }
}
