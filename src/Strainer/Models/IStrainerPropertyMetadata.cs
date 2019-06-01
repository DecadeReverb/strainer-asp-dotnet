namespace Strainer.Models
{
	public interface IStrainerPropertyMetadata
    {
        string Name { get; }
        string FullName { get; }
        bool CanFilter { get; }
        bool CanSort { get; }
    }
}
