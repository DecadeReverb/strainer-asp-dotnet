using Fluorite.Strainer.Models.Metadata;

namespace Fluorite.Strainer.Services.Configuration;

public class ConfigurationMetadataProvider : IConfigurationMetadataProvider
{
    private readonly IStrainerConfigurationProvider _strainerConfigurationProvider;

    public ConfigurationMetadataProvider(IStrainerConfigurationProvider strainerConfigurationProvider)
    {
        _strainerConfigurationProvider = Guard.Against.Null(strainerConfigurationProvider);
    }

    /// <inheritdoc/>
    public IReadOnlyDictionary<Type, IPropertyMetadata> GetDefaultMetadata()
    {
        return _strainerConfigurationProvider
            .GetStrainerConfiguration()
            .DefaultMetadata;
    }

    /// <inheritdoc/>
    public IReadOnlyDictionary<Type, IObjectMetadata> GetObjectMetadata()
    {
        return _strainerConfigurationProvider
            .GetStrainerConfiguration()
            .ObjectMetadata;
    }

    /// <inheritdoc/>
    public IReadOnlyDictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>> GetPropertyMetadata()
    {
        return _strainerConfigurationProvider
            .GetStrainerConfiguration()
            .PropertyMetadata;
    }
}
