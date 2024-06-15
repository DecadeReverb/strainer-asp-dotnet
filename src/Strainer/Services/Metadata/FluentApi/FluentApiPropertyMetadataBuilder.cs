using Fluorite.Strainer.Models.Metadata;
using System.Reflection;

namespace Fluorite.Strainer.Services.Metadata.FluentApi;

public class FluentApiPropertyMetadataBuilder : IFluentApiPropertyMetadataBuilder
{
    public IPropertyMetadata BuildPropertyMetadata(IObjectMetadata objectMetadata)
    {
        if (objectMetadata is null)
        {
            throw new ArgumentNullException(nameof(objectMetadata));
        }

        return new PropertyMetadata
        {
            IsDefaultSorting = true,
            IsDefaultSortingDescending = objectMetadata.IsDefaultSortingDescending,
            IsFilterable = objectMetadata.IsFilterable,
            IsSortable = objectMetadata.IsSortable,
            Name = objectMetadata.DefaultSortingPropertyName,
            PropertyInfo = objectMetadata.DefaultSortingPropertyInfo,
        };
    }

    public IPropertyMetadata BuildPropertyMetadataFromPropertyInfo(IObjectMetadata objectMetadata, PropertyInfo propertyInfo)
    {
        if (propertyInfo is null)
        {
            throw new ArgumentNullException(nameof(propertyInfo));
        }

        if (objectMetadata is null)
        {
            throw new ArgumentNullException(nameof(objectMetadata));
        }

        var isDefaultSorting = objectMetadata.DefaultSortingPropertyInfo == propertyInfo;
        var isDefaultSortingAscending = isDefaultSorting && objectMetadata.IsDefaultSortingDescending;

        return new PropertyMetadata
        {
            IsFilterable = objectMetadata.IsFilterable,
            IsSortable = objectMetadata.IsSortable,
            Name = propertyInfo.Name,
            IsDefaultSorting = isDefaultSorting,
            IsDefaultSortingDescending = isDefaultSortingAscending,
            PropertyInfo = propertyInfo,
        };
    }
}
