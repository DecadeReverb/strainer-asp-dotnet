using Fluorite.Strainer.Attributes;
using Fluorite.Strainer.Models.Metadata;
using System.Reflection;

namespace Fluorite.Strainer.Services.Metadata
{
    public class AttributePropertyMetadataBuilder : IAttributePropertyMetadataBuilder
    {
        public IPropertyMetadata BuildDefaultMetadata(StrainerObjectAttribute attribute, PropertyInfo propertyInfo)
        {
            if (attribute is null)
            {
                throw new ArgumentNullException(nameof(attribute));
            }

            if (propertyInfo is null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }

            return new PropertyMetadata
            {
                IsDefaultSorting = true,
                IsDefaultSortingDescending = attribute.IsDefaultSortingDescending,
                IsFilterable = attribute.IsFilterable,
                IsSortable = attribute.IsSortable,
                Name = propertyInfo.Name,
                PropertyInfo = propertyInfo,
            };
        }
    }
}
