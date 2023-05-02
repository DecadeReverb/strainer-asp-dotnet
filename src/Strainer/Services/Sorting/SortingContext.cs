using Fluorite.Strainer.Services.Validation;

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
            ExpressionProvider = expressionProvider ?? throw new ArgumentNullException(nameof(expressionProvider));
            ExpressionValidator = expressionValidator ?? throw new ArgumentNullException(nameof(expressionValidator));
            Formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
            TermParser = parser ?? throw new ArgumentNullException(nameof(parser));
        }

        public ISortExpressionProvider ExpressionProvider { get; set; }

        public ISortExpressionValidator ExpressionValidator { get; set; }

        public ISortingWayFormatter Formatter { get; }

        public ISortTermParser TermParser { get; }
    }
}
