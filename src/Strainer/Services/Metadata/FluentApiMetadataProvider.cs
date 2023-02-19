using Fluorite.Extensions;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Services.Configuration;
using System.Collections.ObjectModel;

namespace Fluorite.Strainer.Services.Metadata
{
    public class FluentApiMetadataProvider : IMetadataProvider
    {
        private readonly IStrainerOptionsProvider _strainerOptionsProvider;
        private readonly IConfigurationMetadataProvider _metadataProvider;

        public FluentApiMetadataProvider(
            IStrainerOptionsProvider strainerOptionsProvider,
            IConfigurationMetadataProvider metadataProvider)
        {
            _strainerOptionsProvider = strainerOptionsProvider ?? throw new ArgumentNullException(nameof(strainerOptionsProvider));
            _metadataProvider = metadataProvider ?? throw new ArgumentNullException(nameof(metadataProvider));
        }

        public IReadOnlyDictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>> GetAllPropertyMetadata()
        {
            if (!IsMetadataSourceEnabled(MetadataSourceType.FluentApi))
            {
                return null;
            }

            var joinedTypes = _metadataProvider
                .GetPropertyMetadata()
                .Keys
                .Union(_metadataProvider.GetObjectMetadata().Keys);

            // TODO:
            // Refactor this monster below:
            return new ReadOnlyDictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>>(
                joinedTypes.Select(type =>
                {
                    if (_metadataProvider.GetPropertyMetadata().TryGetValue(type, out var metadatas))
                    {
                        return new KeyValuePair<Type, IReadOnlyDictionary<string, IPropertyMetadata>>(
                            type,
                            metadatas.ToReadOnlyDictionary());
                    }

                    var objectMetadata = _metadataProvider.GetObjectMetadata()[type];

                    return new KeyValuePair<Type, IReadOnlyDictionary<string, IPropertyMetadata>>(
                        type,
                        type
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
                            .ToDictionary(
                                propertyMetadata => propertyMetadata.Name,
                                propertyMetadata => (IPropertyMetadata)propertyMetadata)
                            .ToReadOnlyDictionary());
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

            _metadataProvider.GetDefaultMetadata().TryGetValue(modelType, out var propertyMetadata);

            if (propertyMetadata == null)
            {
                if (_metadataProvider.GetObjectMetadata().TryGetValue(modelType, out var objectMetadata))
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

            if (_metadataProvider.GetPropertyMetadata().TryGetValue(modelType, out var propertyMetadatas))
            {
                var propertyMetadata = propertyMetadatas.FirstOrDefault(pair =>
                {
                    return pair.Key.Equals(name)
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

            if (_metadataProvider.GetPropertyMetadata().TryGetValue(modelType, out var metadatas))
            {
                return metadatas.Values;
            }

            return null;
        }

        private bool IsMetadataSourceEnabled(MetadataSourceType metadataSourceType)
        {
            return _strainerOptionsProvider
                .GetStrainerOptions()
                .MetadataSourceType
                .HasFlag(metadataSourceType);
        }
    }
}
