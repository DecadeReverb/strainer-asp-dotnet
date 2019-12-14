﻿using Fluorite.Strainer.Attributes;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Metadata;
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
            return GetDefaultMetadata(typeof(TEntity));
        }

        public IPropertyMetadata GetDefaultMetadata(Type modelType)
        {
            if (modelType is null)
            {
                throw new ArgumentNullException(nameof(modelType));
            }

            var propertyMetadata = GetDefaultMetadataFromPropertyAttribute(modelType);

            if (propertyMetadata == null)
            {
                propertyMetadata = GetDefaultMetadataFromObjectAttribute(modelType);
            }

            return propertyMetadata;
        }

        public IPropertyMetadata GetPropertyMetadata<TEntity>(
            bool isSortableRequired,
            bool isFilterableRequired,
            string name)
        {
            return GetPropertyMetadata(typeof(TEntity), isSortableRequired, isFilterableRequired, name);
        }

        public IPropertyMetadata GetPropertyMetadata(
            Type modelType,
            bool isSortableRequired,
            bool isFilterableRequired,
            string name)
        {
            if (modelType is null)
            {
                throw new ArgumentNullException(nameof(modelType));
            }

            var propertyMetadata = GetMetadataFromPropertyAttribute(modelType, isSortableRequired, isFilterableRequired, name);

            if (propertyMetadata == null)
            {
                propertyMetadata = GetMetadataFromObjectAttribute(modelType, isSortableRequired, isFilterableRequired, name);
            }

            return propertyMetadata;
        }

        public IEnumerable<IPropertyMetadata> GetPropertyMetadatas<TEntity>()
        {
            return GetPropertyMetadatas(typeof(TEntity));
        }

        public IEnumerable<IPropertyMetadata> GetPropertyMetadatas(Type modelType)
        {
            if (modelType is null)
            {
                throw new ArgumentNullException(nameof(modelType));
            }

            var propertyMetadatas = GetMetadatasFromPropertyAttribute(modelType);

            if (propertyMetadatas == null)
            {
                propertyMetadatas = GetMetadatasFromObjectAttribute(modelType);
            }

            return propertyMetadatas;
        }

        private IPropertyMetadata GetDefaultMetadataFromObjectAttribute(Type modelType)
        {
            if (!IsMetadataSourceEnabled(MetadataSourceType.ObjectAttributes))
            {
                return null;
            }

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

        private IPropertyMetadata GetDefaultMetadataFromPropertyAttribute(Type modelType)
        {
            if (!IsMetadataSourceEnabled(MetadataSourceType.PropertyAttributes))
            {
                return null;
            }

            var keyValue = modelType
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

        private IPropertyMetadata GetMetadataFromObjectAttribute(
            Type modelType,
            bool isSortableRequired,
            bool isFilterableRequired,
            string name)
        {
            if (!IsMetadataSourceEnabled(MetadataSourceType.ObjectAttributes))
            {
                return null;
            }

            var currentType = modelType;

            do
            {
                var propertyInfo = modelType
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .FirstOrDefault(p => p.Name == name);
                var attribute = currentType.GetCustomAttribute<StrainerObjectAttribute>(inherit: false);

                if (attribute != null
                    && propertyInfo != null
                    && (isSortableRequired ? attribute.IsSortable : true)
                    && (isFilterableRequired ? attribute.IsFilterable : true))
                {
                    var isDefaultSorting = attribute.DefaultSortingPropertyName == propertyInfo.Name;

                    return new PropertyMetadata
                    {
                        IsDefaultSorting = isDefaultSorting,
                        IsDefaultSortingDescending = isDefaultSorting ? attribute.IsDefaultSortingDescending : false,
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

        private IEnumerable<IPropertyMetadata> GetMetadatasFromObjectAttribute(Type modelType)
        {
            if (!IsMetadataSourceEnabled(MetadataSourceType.ObjectAttributes))
            {
                return null;
            }

            var currentType = modelType;

            do
            {
                var attribute = currentType.GetCustomAttribute<StrainerObjectAttribute>(inherit: false);

                if (attribute != null)
                {
                    return currentType
                        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Select(propertyInfo =>
                        {
                            var isDefaultSorting = attribute.DefaultSortingPropertyName == propertyInfo.Name;

                            return new PropertyMetadata
                            {
                                IsDefaultSorting = isDefaultSorting,
                                IsDefaultSortingDescending = isDefaultSorting ? attribute.IsDefaultSortingDescending : false,
                                IsFilterable = attribute.IsFilterable,
                                IsSortable = attribute.IsSortable,
                                Name = propertyInfo.Name,
                                PropertyInfo = propertyInfo,
                            };
                        });
                }

                currentType = currentType.BaseType;
            } while (currentType != typeof(object) && currentType != typeof(ValueType));

            return null;
        }

        private IPropertyMetadata GetMetadataFromPropertyAttribute(
            Type modelType,
            bool isSortableRequired,
            bool isFilterableRequired,
            string name)
        {
            if (!IsMetadataSourceEnabled(MetadataSourceType.PropertyAttributes))
            {
                return null;
            }

            var stringComparisonMethod = _options.IsCaseInsensitiveForNames
                ? StringComparison.OrdinalIgnoreCase
                : StringComparison.Ordinal;

            var keyValue = modelType
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

                    return (isSortableRequired ? attribute.IsSortable : true)
                        && (isFilterableRequired ? attribute.IsFilterable : true)
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

        private IEnumerable<IPropertyMetadata> GetMetadatasFromPropertyAttribute(Type modelType)
        {
            if (!IsMetadataSourceEnabled(MetadataSourceType.PropertyAttributes))
            {
                return null;
            }

            var metadataPairs = modelType
                .GetProperties()
                .Select(propertyInfo =>
                {
                    var attribute = propertyInfo.GetCustomAttribute<StrainerPropertyAttribute>(inherit: false);

                    return new KeyValuePair<PropertyInfo, StrainerPropertyAttribute>(propertyInfo, attribute);
                })
                .Where(pair => pair.Value != null);

            foreach (var metadataPair in metadataPairs)
            {
                if (metadataPair.Value.PropertyInfo == null)
                {
                    metadataPair.Value.PropertyInfo = metadataPair.Key;
                }
            }

            return metadataPairs.Select(metadataPair => metadataPair.Value);
        }

        private bool IsMetadataSourceEnabled(MetadataSourceType metadataSourceType)
            => _options.MetadataSourceType.HasFlag(metadataSourceType);
    }
}