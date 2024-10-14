using Fluorite.Strainer.Services.Validation;

namespace Fluorite.Strainer.Services.Filtering;

public class FilterContext : IFilterContext
{
    public FilterContext(
        IFilterExpressionProvider expressionProvider,
        IFilterOperatorParser operatorParser,
        IFilterOperatorValidator operatorValidator,
        IFilterTermParser termParser)
    {
        ExpressionProvider = Guard.Against.Null(expressionProvider);
        OperatorParser = Guard.Against.Null(operatorParser);
        OperatorValidator = Guard.Against.Null(operatorValidator);
        TermParser = Guard.Against.Null(termParser);
    }

    public IFilterExpressionProvider ExpressionProvider { get; }

    public IFilterOperatorParser OperatorParser { get; }

    public IFilterOperatorValidator OperatorValidator { get; }

    public IFilterTermParser TermParser { get; }
}
