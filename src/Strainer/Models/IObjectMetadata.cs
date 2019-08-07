namespace Fluorite.Strainer.Models
{
    public interface IObjectMetadata
    {
        bool DefaultSortingPropertyName { get; set; }
        bool IsDefaultSortingDescending { get; set; }
        bool IsFilterable { get; set; }
        bool IsSortable { get; set; }
    }
}
