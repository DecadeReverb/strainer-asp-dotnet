using Fluorite.Strainer.Attributes;
using Fluorite.Strainer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fluorite.Strainer.Services
{
    public class PropertyMetadataProvider : IPropertyMetadataProvider
    {
        private readonly IPropertyMapper _mapper;
        private readonly StrainerOptions _options;

        public PropertyMetadataProvider(IPropertyMapper mapper, StrainerOptions options)
        {
            _mapper = mapper;
            _options = options;
        }

        public IPropertyMetadata GetMetadataFromAttribute<TEntity>(
            bool isSortingRequired,
            bool isFilteringRequired,
            string name)
        {
            var stringComparisonMethod = _options.CaseSensitive
                ? StringComparison.Ordinal
                : StringComparison.OrdinalIgnoreCase;

            var modelType = typeof(TEntity);
            var keyValue = modelType
                .GetProperties()
                .Select(propertyInfo =>
                {
                    var attribute = propertyInfo.GetCustomAttribute<StrainerAttribute>(inherit: true);

                    return new KeyValuePair<PropertyInfo, StrainerAttribute>(propertyInfo, attribute);
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
