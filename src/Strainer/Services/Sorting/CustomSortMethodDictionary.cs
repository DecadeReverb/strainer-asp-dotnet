using Fluorite.Strainer.Models.Sorting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Fluorite.Strainer.Services.Sorting
{
    public class CustomSortMethodDictionary :
      ICustomSortMethodDictionary,
      IReadOnlyDictionary<Type, IReadOnlyDictionary<string, ICustomSortMethod>>,
      IReadOnlyCollection<KeyValuePair<Type, IReadOnlyDictionary<string, ICustomSortMethod>>>,
      IEnumerable<KeyValuePair<Type, IReadOnlyDictionary<string, ICustomSortMethod>>>,
      IEnumerable
    {
        private readonly IReadOnlyDictionary<Type, IReadOnlyDictionary<string, ICustomSortMethod>> _customSortMethods;
        private readonly IStrainerOptionsProvider _optionsProvider;

        public CustomSortMethodDictionary(
            IDictionary<Type, IDictionary<string, ICustomSortMethod>> customSortMethods,
            IStrainerOptionsProvider strainerOptionsProvider)
        {
            if (customSortMethods is null)
            {
                throw new ArgumentNullException(nameof(customSortMethods));
            }

            _customSortMethods = new ReadOnlyDictionary<Type, IReadOnlyDictionary<string, ICustomSortMethod>>(
                customSortMethods.ToDictionary(
                    pair => pair.Key,
                    pair => (IReadOnlyDictionary<string, ICustomSortMethod>)
                        new ReadOnlyDictionary<string, ICustomSortMethod>(pair.Value)));
            _optionsProvider = strainerOptionsProvider;
        }

        public IReadOnlyDictionary<string, ICustomSortMethod> this[Type key]
        {
            get
            {
                return _customSortMethods[key];
            }
        }

        public int Count => _customSortMethods.Count;

        public IEnumerable<Type> Keys => _customSortMethods.Keys;

        public IEnumerable<IReadOnlyDictionary<string, ICustomSortMethod>> Values
        {
            get
            {
                return _customSortMethods.Values;
            }
        }

        public bool ContainsKey(Type key) => _customSortMethods.ContainsKey(key);

        public IEnumerator<KeyValuePair<Type, IReadOnlyDictionary<string, ICustomSortMethod>>> GetEnumerator()
        {
            return _customSortMethods.GetEnumerator();
        }

        public bool TryGetMethod<TEntity>(string name, out ICustomSortMethod<TEntity> customMethod)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            customMethod = null;

            if (!_customSortMethods.TryGetValue(typeof(TEntity), out var customMethods))
            {
                return false;
            }

            var comparisonType = _optionsProvider.GetStrainerOptions().IsCaseInsensitiveForNames
                ? StringComparison.OrdinalIgnoreCase
                : StringComparison.Ordinal;

            customMethod = customMethods
                .FirstOrDefault(pair => pair.Key.Equals(name, comparisonType))
                .Value as ICustomSortMethod<TEntity>;

            return customMethod != null;
        }

        public bool TryGetValue(Type key, out IReadOnlyDictionary<string, ICustomSortMethod> value)
        {
            return _customSortMethods.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _customSortMethods.GetEnumerator();
        }
    }
}
