using Fluorite.Strainer.Models.Metadata;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Fluorite.Strainer.Services.Metadata
{
    public class DefaultMetadataDictionary :
        IDefaultMetadataDictionary,
        IReadOnlyDictionary<Type, IPropertyMetadata>,
        IReadOnlyCollection<KeyValuePair<Type, IPropertyMetadata>>,
        IEnumerable<KeyValuePair<Type, IPropertyMetadata>>,
        IEnumerable
    {
        private readonly IDictionary<Type, IPropertyMetadata> _defaultMetadata;

        public DefaultMetadataDictionary(IDictionary<Type, IPropertyMetadata> defaultMetadata)
        {
            if (defaultMetadata is null)
            {
                throw new ArgumentNullException(nameof(defaultMetadata));
            }

            _defaultMetadata = defaultMetadata;
        }

        public IPropertyMetadata this[Type key] => _defaultMetadata[key];

        public int Count => _defaultMetadata.Count;

        public IEnumerable<Type> Keys => _defaultMetadata.Keys;

        public IEnumerable<IPropertyMetadata> Values => _defaultMetadata.Values;

        public bool ContainsKey(Type key) => _defaultMetadata.ContainsKey(key);

        public IEnumerator<KeyValuePair<Type, IPropertyMetadata>> GetEnumerator()
        {
            return _defaultMetadata.GetEnumerator();
        }

        public bool TryGetValue(Type key, out IPropertyMetadata value)
        {
            return _defaultMetadata.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _defaultMetadata.GetEnumerator();
        }
    }
}
