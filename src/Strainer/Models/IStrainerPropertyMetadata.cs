using System;
using System.Reflection;

namespace Fluorite.Strainer.Models
{
    public interface IStrainerPropertyMetadata
    {
        string DisplayName { get; }
        bool IsFilterable { get; }
        bool IsSortable { get; }
        string Name { get; }
        PropertyInfo PropertyInfo { get; }
        Type Type { get; }
    }
}
