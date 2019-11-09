namespace Fluorite.Strainer.Models
{
    /// <summary>
    /// Represents filtering and sorting metadata for a class or struct,
    /// setting default values for all its properties.
    /// </summary>
    public interface IObjectMetadata
    {
        string DefaultSortingPropertyName { get; }

        bool IsDefaultSortingDescending { get; }

        /// <summary>
        /// Gets a <see cref="bool"/> value indicating whether related
        /// object is marked as filterable.
        /// </summary>
        bool IsFilterable { get; }

        /// <summary>
        /// Gets a <see cref="bool"/> value indicating whether related
        /// object is marked as filterable.
        /// <para/>
        /// Defaults to <see langword="true"/>.
        /// </summary>
        bool IsSortable { get; }
    }
}
