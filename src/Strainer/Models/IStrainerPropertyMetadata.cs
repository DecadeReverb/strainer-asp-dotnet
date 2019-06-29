using System.Reflection;

namespace Fluorite.Strainer.Models
{
    /// <summary>
    /// Provides metadata about properties used by Strainer.
    /// </summary>
    public interface IStrainerPropertyMetadata
    {
        /// <summary>
        /// Gets the display name of the property.
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// Gets a <see cref="bool"/> value indicating whether related
        /// property is marked as filterable for Strainer.
        /// </summary>
        bool IsFilterable { get; }

        /// <summary>
        /// Gets a <see cref="bool"/> value indicating whether related
        /// property is marked as sortable for Strainer.
        /// </summary>
        bool IsSortable { get; }

        /// <summary>
        /// Gets the name under which property was marked.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the <see cref="System.Reflection.PropertyInfo"/> for related
        /// property.
        /// </summary>
        PropertyInfo PropertyInfo { get; }
    }
}
