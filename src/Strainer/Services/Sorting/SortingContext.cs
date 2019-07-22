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
            ExpressionProvider = expressionProvider;
            ExpressionValidator = expressionValidator;
            Formatter = formatter;
            TermParser = parser;
        }

        public ISortExpressionProvider ExpressionProvider { get; set; }

        public ISortExpressionValidator ExpressionValidator { get; set; }

        public ISortingWayFormatter Formatter { get; }

        public ISortTermParser TermParser { get; }
    }
}
