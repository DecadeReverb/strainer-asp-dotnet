using Fluorite.Strainer.Models.Filtering;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Fluorite.Strainer.Services.Filtering
{
    public class CustomFilterMethodDictionary :
        ICustomFilterMethodDictionary,
        IReadOnlyDictionary<Type, IReadOnlyDictionary<string, ICustomFilterMethod>>,
        IReadOnlyCollection<KeyValuePair<Type, IReadOnlyDictionary<string, ICustomFilterMethod>>>,
        IEnumerable<KeyValuePair<Type, IReadOnlyDictionary<string, ICustomFilterMethod>>>,
        IEnumerable
    {
        private readonly IReadOnlyDictionary<Type, IReadOnlyDictionary<string, ICustomFilterMethod>> _customFilterMethods;
        private readonly IStrainerOptionsProvider _optionsProvider;

        public CustomFilterMethodDictionary(
            IDictionary<Type, IDictionary<string, ICustomFilterMethod>> customFilterMethods,
            IStrainerOptionsProvider strainerOptionsProvider)
        {
            if (customFilterMethods is null)
            {
                throw new ArgumentNullException(nameof(customFilterMethods));
            }

            _customFilterMethods = new ReadOnlyDictionary<Type, IReadOnlyDictionary<string, ICustomFilterMethod>>(
                customFilterMethods.ToDictionary(
                    pair => pair.Key,
                    pair => (IReadOnlyDictionary<string, ICustomFilterMethod>)
                        new ReadOnlyDictionary<string, ICustomFilterMethod>(pair.Value)));
            _optionsProvider = strainerOptionsProvider;
        }

        public IReadOnlyDictionary<string, ICustomFilterMethod> this[Type key]
        {
            get
            {
                return _customFilterMethods[key];
            }
        }

        public int Count => _customFilterMethods.Count;

        public IEnumerable<Type> Keys => _customFilterMethods.Keys;

        public IEnumerable<IReadOnlyDictionary<string, ICustomFilterMethod>> Values
        {
            get
            {
                return _customFilterMethods.Values;
            }
        }

        public bool ContainsKey(Type key) => _customFilterMethods.ContainsKey(key);

        public IEnumerator<KeyValuePair<Type, IReadOnlyDictionary<string, ICustomFilterMethod>>> GetEnumerator()
        {
            return _customFilterMethods.GetEnumerator();
        }

        public bool TryGetMethod<TEntity>(string name, out ICustomFilterMethod<TEntity> customMethod)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            customMethod = null;

            if (!_customFilterMethods.TryGetValue(typeof(TEntity), out var customMethods))
            {
                return false;
            }

            var comparisonType = _optionsProvider.GetStrainerOptions().IsCaseInsensitiveForNames
                ? StringComparison.OrdinalIgnoreCase
                : StringComparison.Ordinal;

            customMethod = customMethods
                .FirstOrDefault(pair => pair.Key.Equals(name, comparisonType))
                .Value as ICustomFilterMethod<TEntity>;

            return customMethod != null;
        }

        public bool TryGetValue(Type key, out IReadOnlyDictionary<string, ICustomFilterMethod> value)
        {
            return _customFilterMethods.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _customFilterMethods.GetEnumerator();
        }
    }
}
