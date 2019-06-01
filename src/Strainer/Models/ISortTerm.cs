namespace Fluorite.Strainer.Models
{
	public interface ISortTerm
    {
        string Input { set; }
        bool IsDescending { get; }
        string Name { get; }
    }
}
