namespace Fluorite.Strainer.Services.Sorting
{
    public interface ISortingContext
    {
        ISortExpressionProvider ExpressionProvider { get; }
        ISortingWayFormatter Formatter { get; }
        ISortTermParser TermParser { get; }
    }
}
