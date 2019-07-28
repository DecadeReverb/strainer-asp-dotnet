using Fluorite.Strainer.Models;
using System;

namespace Fluorite.Strainer.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
    public class StrainerObjectAttribute : Attribute, IObjectMetadata
    {
        public StrainerObjectAttribute()
        {

        }

        public bool DefaultSortingPropertyName { get; set; }
        public bool IsDefaultSortingDescending { get; set; }
        public bool IsFilterable { get; set; }
        public bool IsSortable { get; set; }
    }
}
