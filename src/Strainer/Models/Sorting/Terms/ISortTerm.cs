namespace Fluorite.Strainer.Models.Sorting.Terms
{
	public interface ISortTerm
    {
        string Input { get; }
        bool IsDescending { get; }
        string Name { get; }
    }
}
