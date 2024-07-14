using Fluorite.Strainer.Models.Metadata;

namespace Fluorite.Strainer.Services.Configuration;

public class ConfigurationMetadataProvider : IConfigurationMetadataProvider
{
    private readonly IStrainerConfigurationProvider _strainerConfigurationProvider;

    public ConfigurationMetadataProvider(IStrainerConfigurationProvider strainerConfigurationProvider)
    {
        _strainerConfigurationProvider = Guard.Against.Null(strainerConfigurationProvider);
    }

    /// <summary>
    /// Gets the object default dictionary.
    /// </summary>
    public IReadOnlyDictionary<Type, IPropertyMetadata> GetDefaultMetadata()
    {
        return _strainerConfigurationProvider
            .GetStrainerConfiguration()
            .DefaultMetadata;
    }

    /// <summary>
    /// Gets the object metadata dictionary.
    /// </summary>
    public IReadOnlyDictionary<Type, IObjectMetadata> GetObjectMetadata()
    {
        return _strainerConfigurationProvider
            .GetStrainerConfiguration()
            .ObjectMetadata;
    }

    /// <summary>
    /// Gets the object property dictionary.
    /// </summary>
    public IReadOnlyDictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>> GetPropertyMetadata()
    {
        return _strainerConfigurationProvider
            .GetStrainerConfiguration()
            .PropertyMetadata;
    }
}
