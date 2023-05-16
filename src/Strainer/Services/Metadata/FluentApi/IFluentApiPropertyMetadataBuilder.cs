using Fluorite.Strainer.Models.Metadata;
using System.Reflection;

namespace Fluorite.Strainer.Services.Metadata.FluentApi
{
    public interface IFluentApiPropertyMetadataBuilder
    {
        IPropertyMetadata BuildPropertyMetadata(IObjectMetadata objectMetadata);

        IPropertyMetadata BuildPropertyMetadataFromPropertyInfo(IObjectMetadata objectMetadata, PropertyInfo propertyInfo);
    }
}
