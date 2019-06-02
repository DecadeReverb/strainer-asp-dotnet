namespace Fluorite.Strainer.Models
{
	public interface ISortTerm
    {
        string Input { get; }
        bool IsDescending { get; }
        string Name { get; }
    }
}
