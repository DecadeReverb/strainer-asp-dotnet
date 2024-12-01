using System.Reflection;

namespace Fluorite.Strainer.Models.Metadata;

/// <summary>
/// Represents filtering and sorting metadata for a class or struct,
/// setting default values for all its properties.
/// </summary>
public interface IObjectMetadata
{
    /// <summary>
    /// Gets a property name being default sorting property for marked object.
    /// </summary>
    string DefaultSortingPropertyName { get; }

    /// <summary>
    /// Gets a value indicating whether default
    /// sorting way for marked object is descending.
    /// </summary>
    bool IsDefaultSortingDescending { get; }

    /// <summary>
    /// Gets a value indicating whether related
    /// object is marked as filterable.
    /// </summary>
    bool IsFilterable { get; }

    /// <summary>
    /// Gets a value indicating whether related
    /// object is marked as filterable.
    /// <para/>
    /// Defaults to <see langword="true"/>.
    /// </summary>
    bool IsSortable { get; }

    /// <summary>
    /// Gets the <see cref="PropertyInfo"/> for default sorting property.
    /// </summary>
    PropertyInfo? DefaultSortingPropertyInfo { get; }
}
