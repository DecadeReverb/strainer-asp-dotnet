using Fluorite.Strainer.Models.Filtering;
using Fluorite.Strainer.Models.Sorting;

namespace Fluorite.Strainer.Services.Configuration;

/// <summary>
/// Provides read-only configuration for custom methods.
/// </summary>
public interface IConfigurationCustomMethodsProvider
{
    /// <summary>
    /// Gets the object custom filter methods dictionary.
    /// </summary>
    IReadOnlyDictionary<Type, IReadOnlyDictionary<string, ICustomFilterMethod>> GetCustomFilterMethods();

    /// <summary>
    /// Gets the object custom sorting methods dictionary.
    /// </summary>
    IReadOnlyDictionary<Type, IReadOnlyDictionary<string, ICustomSortMethod>> GetCustomSortMethods();
}
