using Fluorite.Strainer.Attributes;
using Fluorite.Strainer.Models.Metadata;
using System.Reflection;

namespace Fluorite.Strainer.Services.Metadata.Attributes;

public class AttributePropertyMetadataBuilder : IAttributePropertyMetadataBuilder
{
    public IPropertyMetadata BuildDefaultPropertyMetadata(StrainerObjectAttribute attribute, PropertyInfo propertyInfo)
    {
        Guard.Against.Null(attribute);
        Guard.Against.Null(propertyInfo);

        return new PropertyMetadata(propertyInfo.Name, propertyInfo)
        {
            IsDefaultSorting = true,
            IsDefaultSortingDescending = attribute.IsDefaultSortingDescending,
            IsFilterable = attribute.IsFilterable,
            IsSortable = attribute.IsSortable,
        };
    }

    public IPropertyMetadata BuildPropertyMetadata(StrainerObjectAttribute attribute, PropertyInfo propertyInfo)
    {
        Guard.Against.Null(attribute);
        Guard.Against.Null(propertyInfo);

        var isDefaultSorting = attribute.DefaultSortingPropertyName == propertyInfo.Name;

        return new PropertyMetadata(propertyInfo.Name, propertyInfo)
        {
            IsDefaultSorting = isDefaultSorting,
            IsDefaultSortingDescending = isDefaultSorting && attribute.IsDefaultSortingDescending,
            IsFilterable = attribute.IsFilterable,
            IsSortable = attribute.IsSortable,
        };
    }
}
