using Fluorite.Strainer.Collections;
using Fluorite.Strainer.Models.Filtering;
using Fluorite.Strainer.Models.Filtering.Operators;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Models.Sorting;

namespace Fluorite.Strainer.Models.Configuration;

/// <summary>
/// Provides readonly information about metadata, filter operators
/// and custom methods for Strainer.
/// </summary>
public class StrainerConfiguration : IStrainerConfiguration
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StrainerConfiguration"/> class.
    /// </summary>
    public StrainerConfiguration(
        IReadOnlyDictionary<Type, IReadOnlyDictionary<string, ICustomFilterMethod>> customFilterMethods,
        IReadOnlyDictionary<Type, IReadOnlyDictionary<string, ICustomSortMethod>> customSortMethods,
        IReadOnlyDictionary<Type, IPropertyMetadata> defaultMetadata,
        IReadOnlyDictionary<string, IFilterOperator> filterOperators,
        IReadOnlySet<string> excludedBuiltInFilterOperators,
        IReadOnlyDictionary<Type, IObjectMetadata> objectMetadata,
        IReadOnlyDictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>> propertyMetadata)
    {
        CustomFilterMethods = Guard.Against.Null(customFilterMethods);
        CustomSortMethods = Guard.Against.Null(customSortMethods);
        DefaultMetadata = Guard.Against.Null(defaultMetadata);
        FilterOperators = Guard.Against.Null(filterOperators);
        ExcludedBuiltInFilterOperators = Guard.Against.Null(excludedBuiltInFilterOperators);
        ObjectMetadata = Guard.Against.Null(objectMetadata);
        PropertyMetadata = Guard.Against.Null(propertyMetadata);
    }

    /// <inheritdoc/>
    public IReadOnlyDictionary<Type, IReadOnlyDictionary<string, ICustomFilterMethod>> CustomFilterMethods { get; }

    /// <inheritdoc/>
    public IReadOnlyDictionary<Type, IReadOnlyDictionary<string, ICustomSortMethod>> CustomSortMethods { get; }

    /// <inheritdoc/>
    public IReadOnlyDictionary<Type, IPropertyMetadata> DefaultMetadata { get; }

    /// <inheritdoc/>
    public IReadOnlyDictionary<string, IFilterOperator> FilterOperators { get; }

    /// <inheritdoc/>
    public IReadOnlySet<string> ExcludedBuiltInFilterOperators { get; }

    /// <inheritdoc/>
    public IReadOnlyDictionary<Type, IObjectMetadata> ObjectMetadata { get; }

    /// <inheritdoc/>
    public IReadOnlyDictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>> PropertyMetadata { get; }
}
