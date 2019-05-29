namespace Sieve.Models
{
	public interface ISortTerm
    {
        bool IsDescending { get; }
        string Name { get; }
        string Sort { set; }
    }
}
