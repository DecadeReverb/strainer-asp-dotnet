﻿using Fluorite.Strainer.Models.Filtering;
using Fluorite.Strainer.Models.Sorting;

namespace Fluorite.Strainer.Services.Configuration;

/// <summary>
/// Provides read-only configuration for custom methods.
/// </summary>
public class ConfigurationCustomMethodsProvider : IConfigurationCustomMethodsProvider
{
    private readonly IStrainerConfigurationProvider _strainerConfigurationProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationCustomMethodsProvider"/>
    /// class.
    /// </summary>
    /// <param name="strainerConfigurationProvider">
    /// The main Stariner configuration provider to use.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="strainerConfigurationProvider"/> is <see langword="null"/>.
    /// </exception>
    public ConfigurationCustomMethodsProvider(IStrainerConfigurationProvider strainerConfigurationProvider)
    {
        _strainerConfigurationProvider = Guard.Against.Null(strainerConfigurationProvider);
    }

    /// <inheritdoc/>
    public IReadOnlyDictionary<Type, IReadOnlyDictionary<string, ICustomFilterMethod>> GetCustomFilterMethods()
    {
        return _strainerConfigurationProvider
            .GetStrainerConfiguration()
            .CustomFilterMethods;
    }

    /// <inheritdoc/>
    public IReadOnlyDictionary<Type, IReadOnlyDictionary<string, ICustomSortMethod>> GetCustomSortMethods()
    {
        return _strainerConfigurationProvider
            .GetStrainerConfiguration()
            .CustomSortMethods;
    }
}
