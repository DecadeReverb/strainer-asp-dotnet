using System;

namespace Fluorite.Strainer.Models
{
    /// <summary>
    /// Defines source type for property metadata.
    /// </summary>
    [Flags]
    public enum MetadataSourceType
    {
        None = 1,

        PropertyAttributes = 2,

        ObjectAttributes = 4,

        Attributes = PropertyAttributes | ObjectAttributes,

        FluentApi = 8,

        All = PropertyAttributes | ObjectAttributes | FluentApi,
    }
}
