using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Metadata;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Fluorite.Strainer.Services.Metadata
{
    public class FluentApiMetadataProvider : IMetadataProvider
    {
        private readonly StrainerOptions _options;
        private readonly IDefaultMetadataDictionary _defaultMetadata;
        private readonly IObjectMetadataDictionary _objectMetadata;
        private readonly IPropertyMetadataDictionary _propertyMetadata;

        public FluentApiMetadataProvider(
            IStrainerOptionsProvider strainerOptionsProvider,
            IDefaultMetadataDictionary defaultMetadataDictionary,
            IObjectMetadataDictionary objectMetadataDictionary,
            IPropertyMetadataDictionary propertyMetadataDictionary)
        {
            _options = (strainerOptionsProvider?.GetStrainerOptions()
                ?? throw new ArgumentNullException(nameof(strainerOptionsProvider)));
            _defaultMetadata = defaultMetadataDictionary
                ?? throw new ArgumentNullException(nameof(defaultMetadataDictionary));
            _objectMetadata = objectMetadataDictionary
                ?? throw new ArgumentNullException(nameof(objectMetadataDictionary));
            _propertyMetadata = propertyMetadataDictionary
                ?? throw new ArgumentNullException(nameof(propertyMetadataDictionary));
        }

        public IReadOnlyDictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>> GetAllPropertyMetadata()
        {
            if (!IsMetadataSourceEnabled(MetadataSourceType.FluentApi))
            {
                return null;
            }

            var joinedTypes = _propertyMetadata.Keys.Union(_objectMetadata.Keys);

            return new ReadOnlyDictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>>(
                joinedTypes.Select(type =>
                {
                    if (_propertyMetadata.TryGetValue(type, out var metadatas))
                    {
                        return new KeyValuePair<Type, IReadOnlyDictionary<string, IPropertyMetadata>>(
                            type,
                            new ReadOnlyDictionary<string, IPropertyMetadata>(
                                metadatas.ToDictionary(pair => pair.Key, pair => pair.Value)));
                    }

                    var objectMetadata = _objectMetadata[type];

                    return new KeyValuePair<Type, IReadOnlyDictionary<string, IPropertyMetadata>>(
                        type,
                        new ReadOnlyDictionary<string, IPropertyMetadata>(type
                            .GetProperties()
                            .Select(propertyInfo =>
                            {
                                var isDefaultSorting = objectMetadata.DefaultSortingPropertyInfo == propertyInfo;

                                return new PropertyMetadata
                                {
                                    IsFilterable = objectMetadata.IsFilterable,
                                    IsSortable = objectMetadata.IsSortable,
                                    Name = propertyInfo.Name,
                                    IsDefaultSorting = isDefaultSorting,
                                    IsDefaultSortingDescending = isDefaultSorting
                                        ? objectMetadata.IsDefaultSortingDescending
                                        : false,
                                    PropertyInfo = propertyInfo,
                                };
                            })
                            .ToDictionary(propertyMetadata =>
                                propertyMetadata.Name, propertyMetadata => (IPropertyMetadata)propertyMetadata)));
                }).ToDictionary(pair => pair.Key, pair => pair.Value));
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

            if (!IsMetadataSourceEnabled(MetadataSourceType.FluentApi))
            {
                return null;
            }

            _defaultMetadata.TryGetValue(modelType, out var propertyMetadata);

            if (propertyMetadata == null)
            {
                if (_objectMetadata.TryGetValue(modelType, out var objectMetadata))
                {
                    propertyMetadata = new PropertyMetadata
                    {
                        IsDefaultSorting = true,
                        IsDefaultSortingDescending = objectMetadata.IsDefaultSortingDescending,
                        IsFilterable = objectMetadata.IsFilterable,
                        IsSortable = objectMetadata.IsSortable,
                        Name = objectMetadata.DefaultSortingPropertyName,
                        PropertyInfo = objectMetadata.DefaultSortingPropertyInfo,
                    };
                }
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

            if (!IsMetadataSourceEnabled(MetadataSourceType.FluentApi))
            {
                return null;
            }

            if (_propertyMetadata.TryGetValue(modelType, out var propertyMetadatas))
            {
                var comparisonMethod = _options.IsCaseInsensitiveForNames
                    ? StringComparison.OrdinalIgnoreCase
                    : StringComparison.Ordinal;

                var propertyMetadata = propertyMetadatas.FirstOrDefault(pair =>
                {
                    return pair.Key.Equals(name, comparisonMethod)
                        && (!isSortableRequired || pair.Value.IsSortable)
                        && (!isFilterableRequired || pair.Value.IsFilterable);
                }).Value;

                return propertyMetadata;
            }

            return null;
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

            if (_propertyMetadata.TryGetValue(modelType, out var metadatas))
            {
                return metadatas.Values;
            }

            return null;
        }

        private bool IsMetadataSourceEnabled(MetadataSourceType metadataSourceType)
            => _options.MetadataSourceType.HasFlag(metadataSourceType);
    }
}
