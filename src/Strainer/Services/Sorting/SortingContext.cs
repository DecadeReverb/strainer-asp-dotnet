namespace Fluorite.Strainer.Services.Sorting
{
    public class SortingContext : ISortingContext
    {
        public SortingContext(
            ISortExpressionProvider expressionProvider,
            ISortExpressionValidator expressionValidator,
            ISortingWayFormatter formatter,
            ISortTermParser parser)
        {
            ExpressionProvider = expressionProvider ?? throw new System.ArgumentNullException(nameof(expressionProvider));
            ExpressionValidator = expressionValidator ?? throw new System.ArgumentNullException(nameof(expressionValidator));
            Formatter = formatter ?? throw new System.ArgumentNullException(nameof(formatter));
            TermParser = parser ?? throw new System.ArgumentNullException(nameof(parser));
        }

        public ISortExpressionProvider ExpressionProvider { get; set; }

        public ISortExpressionValidator ExpressionValidator { get; set; }

        public ISortingWayFormatter Formatter { get; }

        public ISortTermParser TermParser { get; }
    }
}
