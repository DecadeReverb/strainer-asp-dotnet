using Fluorite.Strainer.Services.Validation;

namespace Fluorite.Strainer.Services.Sorting;

public class SortingContext : ISortingContext
{
    public SortingContext(
        ISortExpressionProvider expressionProvider,
        ISortExpressionValidator expressionValidator,
        ISortingWayFormatter formatter,
        ISortTermParser sortTermParser,
        ISortTermValueParser sortTermValueParser)
    {
        ExpressionProvider = Guard.Against.Null(expressionProvider);
        ExpressionValidator = Guard.Against.Null(expressionValidator);
        Formatter = Guard.Against.Null(formatter);
        TermParser = Guard.Against.Null(sortTermParser);
        TermValueParser = Guard.Against.Null(sortTermValueParser);
    }

    public ISortExpressionProvider ExpressionProvider { get; set; }

    public ISortExpressionValidator ExpressionValidator { get; set; }

    public ISortingWayFormatter Formatter { get; }

    public ISortTermParser TermParser { get; }

    public ISortTermValueParser TermValueParser { get; }
}
