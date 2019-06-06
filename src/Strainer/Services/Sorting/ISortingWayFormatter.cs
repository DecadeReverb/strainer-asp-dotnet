namespace Fluorite.Strainer.Services.Sorting
{
    public interface ISortingWayFormatter
    {
        string Format(string input);
        bool IsDescending(string input);
        string Unformat(string input);
    }
}
