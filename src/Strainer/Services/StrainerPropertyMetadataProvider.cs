using Fluorite.Strainer.Attributes;
using Fluorite.Strainer.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fluorite.Strainer.Services
{
    public class StrainerPropertyMetadataProvider : IStrainerPropertyMetadataProvider
    {
        private readonly IStrainerPropertyMapper _mapper;
        private readonly StrainerOptions _options;

        public StrainerPropertyMetadataProvider(IStrainerPropertyMapper mapper, IOptions<StrainerOptions> options)
        {
            _mapper = mapper;
            _options = options.Value;
        }

        public IStrainerPropertyMetadata GetMetadata<TEntity>(
            bool isSortingRequired,
            bool ifFileringRequired,
            string name,
            bool includeAttributes = true)
        {
            var metadata = _mapper.FindProperty<TEntity>(
                isSortingRequired,
                ifFileringRequired,
                name,
                _options.CaseSensitive);

            if (metadata == null && includeAttributes)
            {
                return GetMetadataFromAttribute<TEntity>(isSortingRequired, ifFileringRequired, name);
            }

            return metadata;
        }

        public IStrainerPropertyMetadata GetMetadataFromAttribute<TEntity>(
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
                if (string.IsNullOrEmpty(keyValue.Value.Name))
                {
                    keyValue.Value.Name = keyValue.Key.Name;
                }

                keyValue.Value.PropertyInfo = keyValue.Key;
                keyValue.Value.Type = modelType;
            }

            return keyValue.Value;
        }
    }
}
