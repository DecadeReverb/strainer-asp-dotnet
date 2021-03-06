using System.Reflection;

namespace Fluorite.Strainer.Models.Metadata
{
    /// <summary>
    /// Represents filtering and sorting metadata for a class or struct,
    /// setting default values for all its properties.
    /// </summary>
    public class ObjectMetadata : IObjectMetadata
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectMetadata"/> class.
        /// </summary>
        public ObjectMetadata()
        {

        }

        /// <summary>
        /// Gets or sets a property name being default sorting property
        /// for marked object.
        /// </summary>
        public string DefaultSortingPropertyName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether default
        /// sorting way for marked object is descending.
        /// </summary>
        public bool IsDefaultSortingDescending { get; set; }

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
        /// Gets or sets the <see cref="PropertyInfo"/> for default sorting property.
        /// </summary>
        public PropertyInfo DefaultSortingPropertyInfo { get; set;  }
    }
}
