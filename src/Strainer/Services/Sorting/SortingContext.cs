namespace Fluorite.Strainer.Services.Sorting
{
    public class SortingContext : ISortingContext
    {
        public SortingContext(
            ISortingExpressionProvider expressionProvider,
            ISortingExpressionValidator expressionValidator,
            ISortingWayFormatter formatter,
            ISortingTermParser parser)
        {
            ExpressionProvider = expressionProvider;
            ExpressionValidator = expressionValidator;
            Formatter = formatter;
            TermParser = parser;
        }

        public ISortingExpressionProvider ExpressionProvider { get; set; }

        public ISortingExpressionValidator ExpressionValidator { get; set; }

        public ISortingWayFormatter Formatter { get; }

        public ISortingTermParser TermParser { get; }
    }
}
