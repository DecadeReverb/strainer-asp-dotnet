using Fluorite.Strainer.Models;
using System;

namespace Fluorite.Strainer.Attributes
{
    /// <summary>
    /// Marks a class or struct as filterable and/or sortable, setting default
    /// values for all its properties.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
    public class StrainerObjectAttribute : Attribute, IObjectMetadata
    {
        /// <summary>
        /// Initializes new instance of <see cref="StrainerObjectAttribute"/>
        /// class.
        /// </summary>
        public StrainerObjectAttribute()
        {

        }

        public string DefaultSortingPropertyName { get; set; }

        public bool IsDefaultSortingDescending { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="bool"/> value indicating whether related
        /// object is marked as filterable.
        /// <para/>
        /// Defaults to <see langword="true"/>.
        /// </summary>
        public bool IsFilterable { get; set; } = true;

        /// <summary>
        /// Gets or sets a <see cref="bool"/> value indicating whether related
        /// object is marked as filterable.
        /// <para/>
        /// Defaults to <see langword="true"/>.
        /// </summary>
        public bool IsSortable { get; set; } = true;
    }
}
