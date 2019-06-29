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
    public class StrainerPropertyMetadata : IStrainerPropertyMetadata, IEquatable<StrainerPropertyMetadata>
    {
        /// <summary>
        /// Initializes new instance of <see cref="StrainerModel"/> class.
        /// </summary>
        public StrainerPropertyMetadata()
        {

        }

        /// <summary>
        /// Gets or sets the display name of the property.
        /// </summary>
        public string DisplayName { get; set; }

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
        /// Checks if current instance of <see cref="StrainerPropertyMetadata"/>
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
            return Equals(obj as StrainerPropertyMetadata);
        }

        /// <summary>
        /// Checks if current instance of <see cref="StrainerPropertyMetadata"/>
        /// is equal to other <see cref="StrainerPropertyMetadata"/> instance.
        /// </summary>
        /// <param name="other">
        /// Other <see cref="StrainerPropertyMetadata"/> instance.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if provided other <see cref="StrainerPropertyMetadata"/>
        /// instance is equal to the current one; otherwise <see langword="false"/>.
        /// </returns>
        public bool Equals(StrainerPropertyMetadata other)
        {
            return other != null &&
                   DisplayName == other.DisplayName &&
                   IsFilterable == other.IsFilterable &&
                   IsSortable == other.IsSortable &&
                   Name == other.Name &&
                   EqualityComparer<PropertyInfo>.Default.Equals(PropertyInfo, other.PropertyInfo);
        }

        /// <summary>
        /// Gets <see cref="int"/> hash code representation of current
        /// <see cref="StrainerPropertyMetadata"/>.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="StrainerPropertyMetadata"/>.
        /// </returns>
        public override int GetHashCode()
        {
            var hashCode = -1500598692;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(DisplayName);
            hashCode = hashCode * -1521134295 + IsFilterable.GetHashCode();
            hashCode = hashCode * -1521134295 + IsSortable.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<PropertyInfo>.Default.GetHashCode(PropertyInfo);
            return hashCode;
        }

        public static bool operator ==(StrainerPropertyMetadata metadata1, StrainerPropertyMetadata metadata2)
        {
            return EqualityComparer<StrainerPropertyMetadata>.Default.Equals(metadata1, metadata2);
        }

        public static bool operator !=(StrainerPropertyMetadata metadata1, StrainerPropertyMetadata metadata2)
        {
            return !(metadata1 == metadata2);
        }
    }
}
