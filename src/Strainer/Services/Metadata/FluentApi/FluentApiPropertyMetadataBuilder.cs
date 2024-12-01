using Fluorite.Strainer.Models.Metadata;
using System.Reflection;

namespace Fluorite.Strainer.Services.Metadata.FluentApi;

public class FluentApiPropertyMetadataBuilder : IFluentApiPropertyMetadataBuilder
{
    public IPropertyMetadata BuildPropertyMetadata(IObjectMetadata objectMetadata)
    {
        Guard.Against.Null(objectMetadata);

        if (objectMetadata.DefaultSortingPropertyInfo is null)
        {
            throw new ArgumentException("Missing PropertyInfo in passed object metadata.", nameof(objectMetadata));
        }

        return new PropertyMetadata(objectMetadata.DefaultSortingPropertyName, objectMetadata.DefaultSortingPropertyInfo)
        {
            IsDefaultSorting = true,
            IsDefaultSortingDescending = objectMetadata.IsDefaultSortingDescending,
            IsFilterable = objectMetadata.IsFilterable,
            IsSortable = objectMetadata.IsSortable,
        };
    }

    public IPropertyMetadata BuildPropertyMetadataFromPropertyInfo(IObjectMetadata objectMetadata, PropertyInfo propertyInfo)
    {
        Guard.Against.Null(objectMetadata);
        Guard.Against.Null(propertyInfo);

        var isDefaultSorting = objectMetadata.DefaultSortingPropertyInfo == propertyInfo;
        var isDefaultSortingAscending = isDefaultSorting && objectMetadata.IsDefaultSortingDescending;

        return new PropertyMetadata(propertyInfo.Name, propertyInfo)
        {
            IsFilterable = objectMetadata.IsFilterable,
            IsSortable = objectMetadata.IsSortable,
            IsDefaultSorting = isDefaultSorting,
            IsDefaultSortingDescending = isDefaultSortingAscending,
        };
    }
}
