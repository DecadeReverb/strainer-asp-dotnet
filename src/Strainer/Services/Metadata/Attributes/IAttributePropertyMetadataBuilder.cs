using Fluorite.Strainer.Attributes;
using Fluorite.Strainer.Models.Metadata;
using System.Reflection;

namespace Fluorite.Strainer.Services.Metadata.Attributes;

public interface IAttributePropertyMetadataBuilder
{
    IPropertyMetadata BuildDefaultPropertyMetadata(StrainerObjectAttribute attribute, PropertyInfo propertyInfo);

    IPropertyMetadata BuildPropertyMetadata(StrainerObjectAttribute attribute, PropertyInfo propertyInfo);
}