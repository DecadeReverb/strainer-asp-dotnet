using Fluorite.Strainer.Models.Filtering;
using Fluorite.Strainer.Models.Filtering.Operators;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Models.Sorting;

namespace Fluorite.Strainer.Services.Modules;

/// <summary>
/// Stores Strainer configuration in an encapsulated form of a module.
/// </summary>
public interface IStrainerModule
{
    /// <summary>
    /// Gets the object custom filter methods dictionary.
    /// </summary>
    IDictionary<Type, IDictionary<string, ICustomFilterMethod>> CustomFilterMethods { get; }

    /// <summary>
    /// Gets the object custom sorting methods dictionary.
    /// </summary>
    IDictionary<Type, IDictionary<string, ICustomSortMethod>> CustomSortMethods { get; }

    /// <summary>
    /// Gets the object default dictionary.
    /// </summary>
    IDictionary<Type, IPropertyMetadata> DefaultMetadata { get; }

    /// <summary>
    /// Gets the object filter operator dictionary.
    /// </summary>
    IDictionary<string, IFilterOperator> FilterOperators { get; }

    /// <summary>
    /// Gets the object metadata dictionary.
    /// </summary>
    IDictionary<Type, IObjectMetadata> ObjectMetadata { get; }

    /// <summary>
    /// Gets the object property dictionary.
    /// </summary>
    IDictionary<Type, IDictionary<string, IPropertyMetadata>> PropertyMetadata { get; }

    /// <summary>
    /// Loads configuration in module.
    /// </summary>
    /// <param name="builder">
    /// The Strainer Module builder.
    /// </param>
    void Load(IStrainerModuleBuilder builder);
}
