using Fluorite.Strainer.Models.Metadata;

namespace Fluorite.Strainer.Services.Metadata
{
    public interface IObjectMetadataProvider
    {
        IPropertyMetadata GetDefaultMetadataFromObjectAttribute(Type modelType);
    }
}
