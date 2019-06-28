using System;
using System.Collections.Generic;
using System.Reflection;

namespace Fluorite.Strainer.Models
{
	public class StrainerPropertyMetadata : IStrainerPropertyMetadata, IEquatable<StrainerPropertyMetadata>
    {
        public StrainerPropertyMetadata()
        {

        }

        public string DisplayName { get; set; }
        public bool IsFilterable { get; set; }
        public bool IsSortable { get; set; }
        public string Name { get; set; }
        public PropertyInfo PropertyInfo { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as StrainerPropertyMetadata);
        }

        public bool Equals(StrainerPropertyMetadata other)
        {
            return other != null &&
                   DisplayName == other.DisplayName &&
                   IsFilterable == other.IsFilterable &&
                   IsSortable == other.IsSortable &&
                   Name == other.Name &&
                   EqualityComparer<PropertyInfo>.Default.Equals(PropertyInfo, other.PropertyInfo);
        }

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
