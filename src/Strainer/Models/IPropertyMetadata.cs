using System.Reflection;

namespace Fluorite.Strainer.Models
{
    /// <summary>
    /// Provides metadata about a property.
    /// </summary>
    public interface IPropertyMetadata
    {
        /// <summary>
        /// Gets the display name of the property.
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// Gets a <see cref="bool"/> value indicating whether related
        /// property should be used as a default (fallback) property when
        /// sorting objects having related property.
        /// </summary>
        bool IsDefaultSorting { get; }

        /// <summary>
        /// Gets a <see cref="bool"/> value indicating whether default
        /// sorting should be performed in a descending way.
        /// </summary>
        bool IsDefaultSortingDescending { get; }

        /// <summary>
        /// Gets a <see cref="bool"/> value indicating whether related
        /// property is marked as filterable.
        /// </summary>
        bool IsFilterable { get; }

        /// <summary>
        /// Gets a <see cref="bool"/> value indicating whether related
        /// property is marked as sortable.
        /// </summary>
        bool IsSortable { get; }

        /// <summary>
        /// Gets the name of related property.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the <see cref="System.Reflection.PropertyInfo"/> for related
        /// property.
        /// </summary>
        PropertyInfo PropertyInfo { get; }
    }
}
