using Fluorite.Strainer.Attributes;
using Fluorite.Strainer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fluorite.Strainer.Services
{
    public class AttributePropertyMetadataProvider : IAttributePropertyMetadataProvider
    {
        private readonly IPropertyMapper _mapper;
        private readonly StrainerOptions _options;

        public AttributePropertyMetadataProvider(IPropertyMapper mapper, StrainerOptions options)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public IPropertyMetadata GetPropertyMetadata<TEntity>(
            bool isSortingRequired,
            bool isFilteringRequired,
            string name)
        {
            var modelType = typeof(TEntity);
            var propertyMetadata = GetPropertyMetadataFromPropertyAttribute(modelType, isSortingRequired, isFilteringRequired, name);
            if (propertyMetadata == null)
            {
                propertyMetadata = GetPropertyMetadataFromObjectAttribute(modelType, isSortingRequired, isFilteringRequired, name);
            }

            return propertyMetadata;
        }

        private IPropertyMetadata GetPropertyMetadataFromObjectAttribute(
            Type modelType,
            bool isSortingRequired,
            bool isFilteringRequired,
            string name)
        {
            var currentType = modelType;

            do
            {
                var propertyInfo = modelType
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .FirstOrDefault(p => p.Name == name);
                var attribute = currentType.GetCustomAttribute<StrainerObjectAttribute>(inherit: false);

                if (attribute != null
                    && propertyInfo != null
                    && (isSortingRequired ? attribute.IsSortable : true)
                    && (isFilteringRequired ? attribute.IsFilterable : true))
                {
                    return new PropertyMetadata
                    {
                        IsFilterable = attribute.IsFilterable,
                        IsSortable = attribute.IsSortable,
                        Name = propertyInfo.Name,
                        PropertyInfo = propertyInfo,
                    };
                }

                currentType = currentType.BaseType;

            } while (currentType != typeof(object) && currentType != typeof(ValueType));

            return null;
        }

        private IPropertyMetadata GetPropertyMetadataFromPropertyAttribute(
            Type modelType,
            bool isSortingRequired,
            bool isFilteringRequired,
            string name)
        {
            var stringComparisonMethod = _options.CaseSensitive
                ? StringComparison.Ordinal
                : StringComparison.OrdinalIgnoreCase;

            var keyValue = modelType
                .GetProperties()
                .Select(propertyInfo =>
                {
                    var attribute = propertyInfo.GetCustomAttribute<StrainerPropertyAttribute>(inherit: true);

                    return new KeyValuePair<PropertyInfo, StrainerPropertyAttribute>(propertyInfo, attribute);
                })
                .Where(pair => pair.Value != null)
                .FirstOrDefault(pair =>
                {
                    var propertyInfo = pair.Key;
                    var attribute = pair.Value;

                    return (isSortingRequired ? attribute.IsSortable : true)
                        && (isFilteringRequired ? attribute.IsFilterable : true)
                        && ((attribute.DisplayName ?? attribute.Name ?? propertyInfo.Name).Equals(name, stringComparisonMethod));
                });

            if (keyValue.Value != null)
            {
                if (keyValue.Value.PropertyInfo == null)
                {
                    keyValue.Value.PropertyInfo = keyValue.Key;
                }
            }

            return keyValue.Value;
        }
    }
}
