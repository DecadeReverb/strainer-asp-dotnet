using System.Reflection;

namespace Fluorite.Strainer.Models.Metadata;

/// <summary>
/// Represents filtering and sorting metadata for a class or struct,
/// setting default values for all its properties.
/// </summary>
public class ObjectMetadata : IObjectMetadata
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ObjectMetadata"/> class.
    /// </summary>
    public ObjectMetadata(
        string defaultSortingPropertyName,
        bool isDefaultSortingDescending,
        PropertyInfo defaultSortingPropertyInfo)
    {
        DefaultSortingPropertyName = Guard.Against.NullOrWhiteSpace(defaultSortingPropertyName);
        IsDefaultSortingDescending = isDefaultSortingDescending;
        DefaultSortingPropertyInfo = Guard.Against.Null(defaultSortingPropertyInfo);
    }

    /// <summary>
    /// Gets a property name being default sorting property
    /// for marked object.
    /// </summary>
    public string DefaultSortingPropertyName { get; }

    /// <summary>
    /// Gets a value indicating whether default
    /// sorting way for marked object is descending.
    /// </summary>
    public bool IsDefaultSortingDescending { get; }

    /// <summary>
    /// Gets or sets a value indicating whether related
    /// object is marked as filterable.
    /// </summary>
    public bool IsFilterable { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether related
    /// object is marked as filterable.
    /// </summary>
    public bool IsSortable { get; set; }

    /// <summary>
    /// Gets the <see cref="PropertyInfo"/> for default sorting property.
    /// </summary>
    public PropertyInfo DefaultSortingPropertyInfo { get; }
}
