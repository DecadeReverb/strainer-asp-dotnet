namespace Fluorite.Strainer.Services.Sorting
{
    public interface ISortingContext
    {
        ISortExpressionProvider ExpressionProvider { get; }

        ISortExpressionValidator ExpressionValidator { get; }

        ISortingWayFormatter Formatter { get; }

        ISortTermParser TermParser { get; }
    }
}
