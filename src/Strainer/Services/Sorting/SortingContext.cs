namespace Fluorite.Strainer.Services.Sorting
{
    public class SortingContext : ISortingContext
    {
        public SortingContext(ISortExpressionProvider expressionProvider, ISortingWayFormatter formatter, ISortTermParser parser)
        {
            ExpressionProvider = expressionProvider;
            Formatter = formatter;
            TermParser = parser;
        }

        public ISortExpressionProvider ExpressionProvider { get; set; }

        public ISortingWayFormatter Formatter { get; }

        public ISortTermParser TermParser { get; }
    }
}
