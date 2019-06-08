namespace Fluorite.Strainer.Models
{
	public interface IStrainerPropertyMetadata
    {
        string DisplayName { get; }
        bool IsFilterable { get; }
        bool IsSortable { get; }
        string Name { get; }
    }
}
