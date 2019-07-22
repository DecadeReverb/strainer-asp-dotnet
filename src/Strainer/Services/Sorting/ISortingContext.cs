namespace Fluorite.Strainer.Services.Sorting
{
    public interface ISortingContext
    {
        ISortingExpressionProvider ExpressionProvider { get; }
        ISortingExpressionValidator ExpressionValidator { get; }
        ISortingWayFormatter Formatter { get; }
        ISortingTermParser TermParser { get; }
    }
}
