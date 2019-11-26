using Fluorite.Strainer.Models.Metadata;
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
        /// Gets or sets <see cref="bool"/> value indicating whether related
        /// property should be used as a default (fallback) property when
        /// no sorting information was provided but sorting was still requested.
        /// <para/>
        /// Default sorting is not perfomed when sorting information was not
        /// properly recognized.
        /// <para/>
        /// Defaults to <see langword="false"/>.
        /// </summary>
        public bool IsDefaultSorting { get; set; } = false;

        /// <summary>
        /// Gets or sets a <see cref="bool"/> value indicating whether default
        /// sorting should be performed in a descending way.
        /// <para/>
        /// Defaults to <see langword="false"/>.
        /// </summary>
        public bool IsDefaultSortingDescending { get; set; } = false;

        /// <summary>
        /// Gets or sets <see cref="bool"/> value indicating whether related
        /// property is marked as filterable.
        /// <para/>
        /// Defaults to <see langword="false"/>.
        /// </summary>
        public bool IsFilterable { get; set; } = false;

        /// <summary>
        /// Gets or sets a <see cref="bool"/> value indicating whether related
        /// property is marked as sortable.
        /// <para/>
        /// Defaults to <see langword="false"/>.
        /// </summary>
        public bool IsSortable { get; set; } = false;

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
