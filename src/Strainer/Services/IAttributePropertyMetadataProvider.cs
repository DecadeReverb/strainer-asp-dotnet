using Fluorite.Strainer.Models;

namespace Fluorite.Strainer.Services
{
    public interface IAttributePropertyMetadataProvider
    {
        IPropertyMetadata GetPropertyMetadata<TEntity>(
            bool isSortingRequired,
            bool isFilteringRequired,
            string name);
    }
}
