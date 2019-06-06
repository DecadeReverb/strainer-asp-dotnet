namespace Fluorite.Strainer.Services.Sorting
{
    public interface ISortingContext
    {
        ISortingWayFormatter Formatter { get; }
        ISortTermParser TermParser { get; }
    }
}
