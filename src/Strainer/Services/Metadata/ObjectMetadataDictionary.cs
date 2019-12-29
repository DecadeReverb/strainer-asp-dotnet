using Fluorite.Strainer.Models.Metadata;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Fluorite.Strainer.Services.Metadata
{
    public class ObjectMetadataDictionary :
        IObjectMetadataDictionary,
        IReadOnlyDictionary<Type, IObjectMetadata>,
        IReadOnlyCollection<KeyValuePair<Type, IObjectMetadata>>,
        IEnumerable<KeyValuePair<Type, IObjectMetadata>>,
        IEnumerable
    {
        private readonly IDictionary<Type, IObjectMetadata> _objectMetadata;

        public ObjectMetadataDictionary(IDictionary<Type, IObjectMetadata> objectMetadata)
        {
            if (objectMetadata is null)
            {
                throw new ArgumentNullException(nameof(objectMetadata));
            }

            _objectMetadata = objectMetadata;
        }

        public IObjectMetadata this[Type key] => _objectMetadata[key];

        public int Count => _objectMetadata.Count;

        public IEnumerable<Type> Keys => _objectMetadata.Keys;

        public IEnumerable<IObjectMetadata> Values => _objectMetadata.Values;

        public bool ContainsKey(Type key) => _objectMetadata.ContainsKey(key);

        public IEnumerator<KeyValuePair<Type, IObjectMetadata>> GetEnumerator()
        {
            return _objectMetadata.GetEnumerator();
        }

        public bool TryGetValue(Type key, out IObjectMetadata value)
        {
            return _objectMetadata.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _objectMetadata.GetEnumerator();
        }
    }
}
