using Fluorite.Strainer.Models.Metadata;

namespace Fluorite.Strainer.Services.Metadata.Attributes
{
    public interface IObjectMetadataProvider
    {
        IPropertyMetadata GetDefaultMetadataFromObjectAttribute(Type modelType);
    }
}
