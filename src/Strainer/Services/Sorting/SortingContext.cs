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
        ExpressionProvider = expressionProvider ?? throw new ArgumentNullException(nameof(expressionProvider));
        ExpressionValidator = expressionValidator ?? throw new ArgumentNullException(nameof(expressionValidator));
        Formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
        TermParser = sortTermParser ?? throw new ArgumentNullException(nameof(sortTermParser));
        TermValueParser = sortTermValueParser ?? throw new ArgumentNullException(nameof(sortTermValueParser));
    }

    public ISortExpressionProvider ExpressionProvider { get; set; }

    public ISortExpressionValidator ExpressionValidator { get; set; }

    public ISortingWayFormatter Formatter { get; }

    public ISortTermParser TermParser { get; }

    public ISortTermValueParser TermValueParser { get; }
}
