using Fluorite.Strainer.Attributes;
using Fluorite.Strainer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fluorite.Strainer.Services.Metadata
{
    public class AttributeMetadataProvider : IPropertyMetadataProvider
    {
        private readonly StrainerOptions _options;

        public AttributeMetadataProvider(IStrainerOptionsProvider optionsProvider)
        {
            _options = (optionsProvider ?? throw new ArgumentNullException(nameof(optionsProvider)))
                .GetStrainerOptions();
        }

        public IPropertyMetadata GetDefaultMetadata<TEntity>()
        {
            var propertyMetadata = GetDefaultMetadataFromPropertyAttribute<TEntity>();

            if (propertyMetadata == null)
            {
                propertyMetadata = GetDefaultMetadataFromObjectAttribute<TEntity>();
            }

            return propertyMetadata;
        }

        public IPropertyMetadata GetMetadata<TEntity>(
            bool isSortingRequired,
            bool isFilteringRequired,
            string name)
        {
            var propertyMetadata = GetMetadataFromPropertyAttribute<TEntity>(isSortingRequired, isFilteringRequired, name);

            if (propertyMetadata == null)
            {
                propertyMetadata = GetMetadataFromObjectAttribute<TEntity>(isSortingRequired, isFilteringRequired, name);
            }

            return propertyMetadata;
        }

        private IPropertyMetadata GetDefaultMetadataFromObjectAttribute<TEntity>()
        {
            if (!IsMetadataSourceEnabled(MetadataSourceType.ObjectAttributes))
            {
                return null;
            }

            var modelType = typeof(TEntity);
            var currentType = modelType;

            do
            {
                var attribute = currentType.GetCustomAttribute<StrainerObjectAttribute>(inherit: false);

                if (attribute != null && attribute.DefaultSortingPropertyName != null)
                {
                    var propertyInfo = modelType.GetProperty(
                        attribute.DefaultSortingPropertyName,
                        BindingFlags.Public | BindingFlags.Instance);

                    if (propertyInfo == null)
                    {
                        throw new InvalidOperationException(
                            $"Could not find property {attribute.DefaultSortingPropertyName} " +
                            $"in type {modelType.FullName} marked as its default " +
                            $"sorting property. Ensure that such property exists in " +
                            $"{modelType.Name} and it's accessible.");
                    }

                    return new PropertyMetadata
                    {
                        IsFilterable = attribute.IsFilterable,
                        IsSortable = attribute.IsSortable,
                        Name = propertyInfo.Name,
                        PropertyInfo = propertyInfo,
                        IsDefaultSorting = true,
                        IsDefaultSortingDescending = attribute.IsDefaultSortingDescending,
                    };
                }

                currentType = currentType.BaseType;

            } while (currentType != typeof(object) && currentType != typeof(ValueType));

            return null;
        }

        private IPropertyMetadata GetDefaultMetadataFromPropertyAttribute<TEntity>()
        {
            if (!IsMetadataSourceEnabled(MetadataSourceType.PropertyAttributes))
            {
                return null;
            }

            var keyValue = typeof(TEntity)
                .GetProperties()
                .Select(propertyInfo =>
                {
                    var attribute = propertyInfo.GetCustomAttribute<StrainerPropertyAttribute>(inherit: false);

                    return new KeyValuePair<PropertyInfo, StrainerPropertyAttribute>(propertyInfo, attribute);
                })
                .Where(pair => pair.Value != null)
                .FirstOrDefault(pair => pair.Value.IsDefaultSorting);

            if (keyValue.Value != null)
            {
                if (keyValue.Value.PropertyInfo == null)
                {
                    keyValue.Value.PropertyInfo = keyValue.Key;
                }

                if (!keyValue.Value.IsSortable)
                {
                    throw new InvalidOperationException(
                        $"Property {keyValue.Key.Name} on {keyValue.Key.DeclaringType.FullName} " +
                        $"is declared as {nameof(IPropertyMetadata.IsDefaultSorting)} " +
                        $"but not as {nameof(IPropertyMetadata.IsSortable)}. " +
                        $"Set the {nameof(IPropertyMetadata.IsSortable)} to true " +
                        $"in order to use the property as a default sortable property.");
                }
            }

            return keyValue.Value;
        }

        private IPropertyMetadata GetMetadataFromObjectAttribute<TEntity>(
            bool isSortingRequired,
            bool isFilteringRequired,
            string name)
        {
            if (!IsMetadataSourceEnabled(MetadataSourceType.ObjectAttributes))
            {
                return null;
            }

            var modelType = typeof(TEntity);
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

        private IPropertyMetadata GetMetadataFromPropertyAttribute<TEntity>(
            bool isSortingRequired,
            bool isFilteringRequired,
            string name)
        {
            if (!IsMetadataSourceEnabled(MetadataSourceType.PropertyAttributes))
            {
                return null;
            }

            var stringComparisonMethod = _options.IsCaseInsensitiveForNames
                ? StringComparison.OrdinalIgnoreCase
                : StringComparison.Ordinal;

            var keyValue = typeof(TEntity)
                .GetProperties()
                .Select(propertyInfo =>
                {
                    var attribute = propertyInfo.GetCustomAttribute<StrainerPropertyAttribute>(inherit: false);

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

        private bool IsMetadataSourceEnabled(MetadataSourceType metadataSourceType)
            => _options.MetadataSourceType.HasFlag(metadataSourceType);
    }
}
