using Fluorite.Strainer.Models;
using System;
using System.Reflection;

namespace Fluorite.Strainer.Attributes
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class StrainerPropertyAttribute : Attribute, IPropertyMetadata
    {
        public StrainerPropertyAttribute()
        {

        }

        public string DisplayName { get; set; }
        public bool IsDefaultSorting { get; set; }
        public bool IsDefaultSortingAscending { get; set; }
        public bool IsFilterable { get; set; }
        public bool IsSortable { get; set; }
        public string Name => PropertyInfo?.Name;
        public PropertyInfo PropertyInfo { get; set; }
    }
}
