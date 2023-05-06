using Fluorite.Extensions;
using Fluorite.Strainer.Models.Metadata;

namespace Fluorite.Strainer.Services.Metadata.Attributes
{
    public class AttributeMetadataProvider : IMetadataProvider
    {
        private readonly IMetadataSourceTypeProvider _metadataSourceTypeProvider;
        private readonly IMetadataAssemblySourceProvider _metadataAssemblySourceProvider;
        private readonly IAttributeMetadataRetriever _attributeMetadataRetriever;
        private readonly IStrainerAttributeProvider _strainerObjectAttributeProvider;
        private readonly IPropertyMetadataDictionaryProvider _propertyMetadataDictionaryProvider;

        public AttributeMetadataProvider(
            IMetadataSourceTypeProvider metadataSourceTypeProvider,
            IMetadataAssemblySourceProvider metadataAssemblySourceProvider,
            IAttributeMetadataRetriever attributeMetadataRetriever,
            IStrainerAttributeProvider strainerObjectAttributeProvider,
            IPropertyMetadataDictionaryProvider propertyMetadataDictionaryProvider)
        {
            _metadataSourceTypeProvider = metadataSourceTypeProvider ?? throw new ArgumentNullException(nameof(metadataSourceTypeProvider));
            _metadataAssemblySourceProvider = metadataAssemblySourceProvider ?? throw new ArgumentNullException(nameof(metadataAssemblySourceProvider));
            _attributeMetadataRetriever = attributeMetadataRetriever ?? throw new ArgumentNullException(nameof(attributeMetadataRetriever));
            _strainerObjectAttributeProvider = strainerObjectAttributeProvider ?? throw new ArgumentNullException(nameof(strainerObjectAttributeProvider));
            _propertyMetadataDictionaryProvider = propertyMetadataDictionaryProvider ?? throw new ArgumentNullException(nameof(propertyMetadataDictionaryProvider));
        }

        public IReadOnlyDictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>> GetAllPropertyMetadata()
        {
            var assemblies = _metadataAssemblySourceProvider.GetAssemblies();
            var types = _metadataSourceTypeProvider.GetSourceTypes(assemblies);

            // TODO:
            // Move it someplace else? Some provider?
            var objectMetadatas = types
                .Select(type => new
                {
                    Type = type,
                    Attribute = _strainerObjectAttributeProvider.GetObjectAttribute(type),
                })
                .Where(x => x.Attribute != null)
                .Select(x => new
                {
                    x.Type,
                    Metadatas = _propertyMetadataDictionaryProvider.GetMetadata(x.Type, x.Attribute),
                })
                .ToDictionary(x => x.Type, x => x.Metadatas);

            // TODO:
            // Move it someplace else? Some provider?
            var propertyMetadatas = types
                .Select(type => new
                {
                    Type = type,
                    Attributes = _propertyMetadataDictionaryProvider.GetMetadata(type),
                })
                .Where(x => x.Attributes.Any())
                .ToDictionary(x => x.Type, x => x.Attributes);

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

            var propertyMetadata = _attributeMetadataRetriever.GetDefaultMetadataFromPropertyAttribute(modelType);
            propertyMetadata ??= _attributeMetadataRetriever.GetDefaultMetadataFromObjectAttribute(modelType);

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

            var propertyMetadata = _attributeMetadataRetriever.GetMetadataFromPropertyAttribute(modelType, isSortableRequired, isFilterableRequired, name);
            propertyMetadata ??= _attributeMetadataRetriever.GetMetadataFromObjectAttribute(modelType, isSortableRequired, isFilterableRequired, name);

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

            var propertyMetadatas = _attributeMetadataRetriever.GetMetadatasFromPropertyAttribute(modelType);
            propertyMetadatas ??= _attributeMetadataRetriever.GetMetadatasFromObjectAttribute(modelType);

            return propertyMetadatas;
        }
    }
}
