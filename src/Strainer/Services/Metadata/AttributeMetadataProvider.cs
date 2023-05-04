using Fluorite.Extensions;
using Fluorite.Strainer.Attributes;
using Fluorite.Strainer.Models.Metadata;
using System.Reflection;

namespace Fluorite.Strainer.Services.Metadata
{
    public class AttributeMetadataProvider : IMetadataProvider
    {
        private readonly IStrainerOptionsProvider _strainerOptionsProvider;
        private readonly IMetadataSourceTypeProvider _metadataSourceTypeProvider;
        private readonly IMetadataAssemblySourceProvider _metadataAssemblySourceProvider;
        private readonly IObjectMetadataProvider _objectMetadataProvider;

        public AttributeMetadataProvider(
            IStrainerOptionsProvider strainerOptionsProvider,
            IMetadataSourceTypeProvider metadataSourceTypeProvider,
            IMetadataAssemblySourceProvider metadataAssemblySourceProvider,
            IObjectMetadataProvider objectMetadataProvider)
        {
            _strainerOptionsProvider = strainerOptionsProvider ?? throw new ArgumentNullException(nameof(strainerOptionsProvider));
            _metadataSourceTypeProvider = metadataSourceTypeProvider ?? throw new ArgumentNullException(nameof(metadataSourceTypeProvider));
            _metadataAssemblySourceProvider = metadataAssemblySourceProvider ?? throw new ArgumentNullException(nameof(metadataAssemblySourceProvider));
            _objectMetadataProvider = objectMetadataProvider ?? throw new ArgumentNullException(nameof(objectMetadataProvider));
        }

        public IReadOnlyDictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>> GetAllPropertyMetadata()
        {
            var assemblies = _metadataAssemblySourceProvider.GetAssemblies();
            var types = _metadataSourceTypeProvider.GetSourceTypes(assemblies);

            // TODO:
            // Move it someplace else? Some provider?
            var objectMetadatas = types
                .Select(type => new { Type = type, Attribute = type.GetCustomAttribute<StrainerObjectAttribute>(inherit: false) })
                .Where(pair => pair.Attribute != null)
                .Select(pair =>
                {
                    return new
                    {
                        pair.Type,
                        Metadatas = BuildMetadata(pair.Type, pair.Attribute),
                    };
                })
                .ToDictionary(pair => pair.Type, pair => pair.Metadatas);

            // TODO:
            // Move it someplace else? Some provider?
            var propertyMetadatas = types
                .Select(type => new
                {
                    Type = type,
                    Attributes = BuildMetadata(type),
                })
                .Where(pair => pair.Attributes.Any())
                .ToDictionary(pair => pair.Type, pair => pair.Attributes);

            return objectMetadatas
                .MergeLeft(propertyMetadatas)
                .ToReadOnly();
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
            propertyMetadata ??= _objectMetadataProvider.GetDefaultMetadataFromObjectAttribute(modelType);

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
            propertyMetadata ??= GetMetadataFromObjectAttribute(modelType, isSortableRequired, isFilterableRequired, name);

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
            propertyMetadatas ??= GetMetadatasFromObjectAttribute(modelType);

            return propertyMetadatas;
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
                    && (!isSortableRequired || attribute.IsSortable)
                    && (!isFilterableRequired || attribute.IsFilterable))
                {
                    var isDefaultSorting = attribute.DefaultSortingPropertyName == propertyInfo.Name;

                    return new PropertyMetadata
                    {
                        IsDefaultSorting = isDefaultSorting,
                        IsDefaultSortingDescending = isDefaultSorting && attribute.IsDefaultSortingDescending,
                        IsFilterable = attribute.IsFilterable,
                        IsSortable = attribute.IsSortable,
                        Name = propertyInfo.Name,
                        PropertyInfo = propertyInfo,
                    };
                }

                currentType = currentType.BaseType;

            }
            while (currentType != typeof(object) && currentType != typeof(ValueType));

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
                                IsDefaultSortingDescending = isDefaultSorting && attribute.IsDefaultSortingDescending,
                                IsFilterable = attribute.IsFilterable,
                                IsSortable = attribute.IsSortable,
                                Name = propertyInfo.Name,
                                PropertyInfo = propertyInfo,
                            };
                        });
                }

                currentType = currentType.BaseType;
            }
            while (currentType != typeof(object) && currentType != typeof(ValueType));

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

                    return (!isSortableRequired || attribute.IsSortable)
                        && (!isFilterableRequired || attribute.IsFilterable)
                        && (attribute.DisplayName ?? attribute.Name ?? propertyInfo.Name).Equals(name);
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

            if (!metadataPairs.Any())
            {
                return null;
            }

            return metadataPairs.Select(metadataPair =>
            {
                if (metadataPair.Value.PropertyInfo == null)
                {
                    metadataPair.Value.PropertyInfo = metadataPair.Key;
                }

                return metadataPair.Value;
            });
        }

        private IReadOnlyDictionary<string, IPropertyMetadata> BuildMetadata(Type type)
        {
            return type
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(propertyInfo => GetAttributeWithPropertyInfo(propertyInfo))
                .Where(attribute => attribute != null)
                .ToDictionary(attribute => attribute.Name, attribute => (IPropertyMetadata)attribute)
                .ToReadOnly();
        }

        private IReadOnlyDictionary<string, IPropertyMetadata> BuildMetadata(Type type, StrainerObjectAttribute strainerObjectAttribute)
        {
            return type
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(propertyInfo => BuildPropertyMetadata(propertyInfo, strainerObjectAttribute))
                .ToDictionary(metadata => metadata.Name, metadata => (IPropertyMetadata)metadata)
                .ToReadOnly();
        }

        private PropertyMetadata BuildPropertyMetadata(PropertyInfo propertyInfo, StrainerObjectAttribute attribute)
        {
            var isDefaultSorting = attribute.DefaultSortingPropertyName == propertyInfo.Name;

            return new PropertyMetadata
            {
                IsDefaultSorting = isDefaultSorting,
                IsDefaultSortingDescending = isDefaultSorting && attribute.IsDefaultSortingDescending,
                IsFilterable = attribute.IsFilterable,
                IsSortable = attribute.IsSortable,
                Name = propertyInfo.Name,
                PropertyInfo = propertyInfo,
            };
        }

        private StrainerPropertyAttribute GetAttributeWithPropertyInfo(PropertyInfo propertyInfo)
        {
            var attribute = propertyInfo.GetCustomAttribute<StrainerPropertyAttribute>(inherit: false);

            if (attribute != null)
            {
                attribute.PropertyInfo = propertyInfo;
            }

            return attribute;
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
