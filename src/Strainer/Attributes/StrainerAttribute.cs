using Fluorite.Strainer.Models;
using System;

namespace Fluorite.Strainer.Attributes
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class StrainerAttribute : Attribute, IStrainerPropertyMetadata
    {
        public StrainerAttribute()
        {

        }

        public string DisplayName { get; set; }
        public bool IsSortable { get; set; }
        public bool IsFilterable { get; set; }
        public string Name { get; set; }
    }
}
