using Fluorite.Strainer.Models.Metadata;

namespace Fluorite.Strainer.Services.Configuration;

public interface IConfigurationMetadataProvider
{
    /// <summary>
    /// Gets the object default dictionary.
    /// </summary>
    IReadOnlyDictionary<Type, IPropertyMetadata> GetDefaultMetadata();

    /// <summary>
    /// Gets the object property dictionary.
    /// </summary>
    IReadOnlyDictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>> GetPropertyMetadata();

    /// <summary>
    /// Gets the object metadata dictionary.
    /// </summary>
    IReadOnlyDictionary<Type, IObjectMetadata> GetObjectMetadata();
}
