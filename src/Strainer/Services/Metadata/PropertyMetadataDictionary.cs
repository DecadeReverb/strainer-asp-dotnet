using Fluorite.Strainer.Models.Metadata;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Fluorite.Strainer.Services.Metadata
{
    public class PropertyMetadataDictionary :
        IPropertyMetadataDictionary,
        IReadOnlyDictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>>,
        IReadOnlyCollection<KeyValuePair<Type, IReadOnlyDictionary<string, IPropertyMetadata>>>,
        IEnumerable<KeyValuePair<Type, IReadOnlyDictionary<string, IPropertyMetadata>>>,
        IEnumerable
    {
        private readonly IReadOnlyDictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>> _propertyMetadata;

        public PropertyMetadataDictionary(
            IDictionary<Type, IDictionary<string, IPropertyMetadata>> propertyMetadata)
        {
            if (propertyMetadata is null)
            {
                throw new ArgumentNullException(nameof(propertyMetadata));
            }

            _propertyMetadata = new ReadOnlyDictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>>(
                propertyMetadata.ToDictionary(
                    pair => pair.Key,
                    pair => (IReadOnlyDictionary<string, IPropertyMetadata>)
                        new ReadOnlyDictionary<string, IPropertyMetadata>(pair.Value)));
        }

        public IReadOnlyDictionary<string, IPropertyMetadata> this[Type key]
        {
            get
            {
                return _propertyMetadata[key];
            }
        }

        public int Count => _propertyMetadata.Count;

        public IEnumerable<Type> Keys => _propertyMetadata.Keys;

        public IEnumerable<IReadOnlyDictionary<string, IPropertyMetadata>> Values
        {
            get
            {
                return _propertyMetadata.Values;
            }
        }

        public bool ContainsKey(Type key) => _propertyMetadata.ContainsKey(key);

        public IEnumerator<KeyValuePair<Type, IReadOnlyDictionary<string, IPropertyMetadata>>> GetEnumerator()
        {
            return _propertyMetadata.GetEnumerator();
        }

        public bool TryGetValue(Type key, out IReadOnlyDictionary<string, IPropertyMetadata> value)
        {
            return _propertyMetadata.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _propertyMetadata.GetEnumerator();
        }
    }
}
