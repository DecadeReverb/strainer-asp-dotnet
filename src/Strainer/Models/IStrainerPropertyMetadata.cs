namespace Fluorite.Strainer.Models
{
	public interface IStrainerPropertyMetadata
    {
        bool CanFilter { get; }
        bool CanSort { get; }
        string FullName { get; }
        string Name { get; }
    }
}
