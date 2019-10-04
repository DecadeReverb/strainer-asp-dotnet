using Fluorite.Strainer.Models;
using System;
using System.Reflection;

namespace Fluorite.Strainer.Attributes
{
    /// <summary>
    /// Marks a property as filterable and/or sortable.
    /// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class StrainerPropertyAttribute : Attribute, IPropertyMetadata
    {
        /// <summary>
        /// Initializes new instance of <see cref="StrainerPropertyAttribute"/>
        /// class.
        /// </summary>
        public StrainerPropertyAttribute()
        {

        }

        /// <summary>
        /// Gets or sets the display name for related property.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="bool"/> value indicating whether related
        /// property should be used as a default (fallback) property when
        /// sorting objects having related property.
        /// </summary>
        public bool IsDefaultSorting { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="bool"/> value indicating whether default
        /// sorting should be performed in a descending way.
        /// </summary>
        public bool IsDefaultSortingDescending { get; set; }

        /// <summary>
        /// Gets a <see cref="bool"/> value indicating whether related
        /// property is marked as filterable.
        /// </summary>
        public bool IsFilterable { get; set; }

        /// <summary>
        /// Gets a <see cref="bool"/> value indicating whether related
        /// property is marked as sortable.
        /// </summary>
        public bool IsSortable { get; set; }

        /// <summary>
        /// Gets the real name of related property.
        /// </summary>
        public string Name => PropertyInfo?.Name;

        /// <summary>
        /// Gets or sets the <see cref="System.Reflection.PropertyInfo"/> for
        /// related property.
        /// </summary>
        public PropertyInfo PropertyInfo { get; set; }
    }
}
