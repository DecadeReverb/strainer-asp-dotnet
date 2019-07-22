using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Fluorite.Strainer.Models
{
    /// <summary>
    /// Represents default property metadata model used by Strainer.
    /// </summary>
    [DebuggerDisplay("\\{" + nameof(Name) + " = " + "{" + nameof(Name) + "} \\}")]
    public class PropertyMetadata : IPropertyMetadata, IEquatable<PropertyMetadata>
    {
        /// <summary>
        /// Initializes new instance of <see cref="StrainerModel"/> class.
        /// </summary>
        public PropertyMetadata()
        {

        }

        /// <summary>
        /// Gets or sets the display name of the property.
        /// </summary>
        public string DisplayName { get; set; }

        public bool IsDefaultSortOrder { get; set; }

        public bool IsDefaultSortOrderAscending { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="bool"/> value indicating whether related
        /// property is marked as filterable for Strainer.
        /// </summary>
        public bool IsFilterable { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="bool"/> value indicating whether related
        /// property is marked as sortable for Strainer.
        /// </summary>
        public bool IsSortable { get; set; }

        /// <summary>
        /// Gets or sets the name under which property was marked.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="System.Reflection.PropertyInfo"/> for related
        /// property.
        /// </summary>
        public PropertyInfo PropertyInfo { get; set; }

        /// <summary>
        /// Checks if current instance of <see cref="PropertyMetadata"/>
        /// is equal to other <see cref="object"/> instance.
        /// </summary>
        /// <param name="obj">
        /// Other <see cref="object"/> instance.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if provided other <see cref="object"/>
        /// instance is equal to the current one; otherwise <see langword="false"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as PropertyMetadata);
        }

        /// <summary>
        /// Checks if current instance of <see cref="PropertyMetadata"/>
        /// is equal to other <see cref="PropertyMetadata"/> instance.
        /// </summary>
        /// <param name="other">
        /// Other <see cref="PropertyMetadata"/> instance.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if provided other <see cref="PropertyMetadata"/>
        /// instance is equal to the current one; otherwise <see langword="false"/>.
        /// </returns>
        public bool Equals(PropertyMetadata other)
        {
            return other != null &&
                   DisplayName == other.DisplayName &&
                   IsDefaultSortOrder == other.IsDefaultSortOrder &&
                   IsDefaultSortOrderAscending == other.IsDefaultSortOrderAscending &&
                   IsFilterable == other.IsFilterable &&
                   IsSortable == other.IsSortable &&
                   Name == other.Name &&
                   EqualityComparer<PropertyInfo>.Default.Equals(PropertyInfo, other.PropertyInfo);
        }

        /// <summary>
        /// Gets <see cref="int"/> hash code representation of current
        /// <see cref="PropertyMetadata"/>.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="PropertyMetadata"/>.
        /// </returns>
        public override int GetHashCode()
        {
            var hashCode = -1500598692;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DisplayName);
            hashCode = hashCode * -1521134295 + IsDefaultSortOrder.GetHashCode();
            hashCode = hashCode * -1521134295 + IsDefaultSortOrderAscending.GetHashCode();
            hashCode = hashCode * -1521134295 + IsFilterable.GetHashCode();
            hashCode = hashCode * -1521134295 + IsSortable.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<PropertyInfo>.Default.GetHashCode(PropertyInfo);
            return hashCode;
        }

        public static bool operator ==(PropertyMetadata metadata1, PropertyMetadata metadata2)
        {
            return EqualityComparer<PropertyMetadata>.Default.Equals(metadata1, metadata2);
        }

        public static bool operator !=(PropertyMetadata metadata1, PropertyMetadata metadata2)
        {
            return !(metadata1 == metadata2);
        }
    }
}
