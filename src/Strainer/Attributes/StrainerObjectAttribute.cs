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
        /// <param name="defaultSortingPropertyName">
        /// Property name being default sorting property for marked object.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="defaultSortingPropertyName"/> is <see langword="null"/>,
        /// empty or contains only whitespace characters.
        /// </exception>
        public StrainerObjectAttribute(string defaultSortingPropertyName)
        {
            if (string.IsNullOrWhiteSpace(defaultSortingPropertyName))
            {
                throw new ArgumentException(
                    $"{nameof(defaultSortingPropertyName)} cannot be null, empty " +
                    $"or contain only whitespace characters.",
                    nameof(defaultSortingPropertyName));
            }

            DefaultSortingPropertyName = defaultSortingPropertyName;
        }

        /// <summary>
        /// Initializes new instance of <see cref="StrainerObjectAttribute"/>
        /// class.
        /// </summary>
        /// <param name="defaultSortingPropertyName">
        /// Property name being default sorting property for marked object.
        /// </param>
        /// <param name="isDefaultSortingDescending">
        /// A <see cref="bool"/> value indicating whether default sorting way
        /// for marked object is descending.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="defaultSortingPropertyName"/> is <see langword="null"/>,
        /// empty or contains only whitespace characters.
        /// </exception>
        public StrainerObjectAttribute(string defaultSortingPropertyName, bool isDefaultSortingDescending) : this(defaultSortingPropertyName)
        {
            IsDefaultSortingDescending = isDefaultSortingDescending;
        }

        /// <summary>
        /// Gets a property name being default sorting property for marked object.
        /// </summary>
        public string DefaultSortingPropertyName { get; }

        /// <summary>
        /// Gets a <see cref="bool"/> value indicating whether default
        /// sorting way for marked object is descending.
        /// <para/>
        /// Defaults to <see langword="false"/>.
        /// </summary>
        public bool IsDefaultSortingDescending { get; } = false;

        /// <summary>
        /// Gets or sets a <see cref="bool"/> value indicating whether marked
        /// object is marked as filterable.
        /// <para/>
        /// Defaults to <see langword="true"/>.
        /// </summary>
        public bool IsFilterable { get; set; } = true;

        /// <summary>
        /// Gets or sets a <see cref="bool"/> value indicating whether marked
        /// object is marked as filterable.
        /// <para/>
        /// Defaults to <see langword="true"/>.
        /// </summary>
        public bool IsSortable { get; set; } = true;
    }
}
