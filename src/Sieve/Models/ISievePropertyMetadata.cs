namespace Sieve.Models
{
	public interface ISievePropertyMetadata
    {
        string Name { get; }
        string FullName { get; }
        bool CanFilter { get; }
        bool CanSort { get; }
    }
}
