using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Sorting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Fluorite.Strainer.Services.Sorting
{
    public class CustomSortMethodMapper : ICustomSortMethodMapper
    {
        private readonly Dictionary<Type, IDictionary<string, ICustomSortMethod>> _methods;
        private readonly StrainerOptions _options;

        public CustomSortMethodMapper(IStrainerOptionsProvider optionsProvider)
        {
            _methods = new Dictionary<Type, IDictionary<string, ICustomSortMethod>>();
            _options = (optionsProvider ?? throw new ArgumentNullException(nameof(optionsProvider)))
                .GetStrainerOptions();
        }

        public IReadOnlyDictionary<Type, IReadOnlyDictionary<string, ICustomSortMethod>> Methods
        {
            get
            {
                var dictionary = _methods.ToDictionary(
                    k => k.Key,
                    v => new ReadOnlyDictionary<string, ICustomSortMethod>(v.Value) as IReadOnlyDictionary<string, ICustomSortMethod>);

                return new ReadOnlyDictionary<Type, IReadOnlyDictionary<string, ICustomSortMethod>>(dictionary);
            }
        }

        public void AddMap<TEntity>(ICustomSortMethod<TEntity> sortMethod)
        {
            if (sortMethod == null)
            {
                throw new ArgumentNullException(nameof(sortMethod));
            }

            if (!_methods.ContainsKey(typeof(TEntity)))
            {
                _methods[typeof(TEntity)] = new Dictionary<string, ICustomSortMethod>();
            }

            _methods[typeof(TEntity)][sortMethod.Name] = sortMethod;
        }

        public ICustomSortMethodBuilder<TEntity> CustomMethod<TEntity>(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(
                    $"{nameof(name)} cannot be null, empty " +
                    $"or contain only whitespace characaters.",
                    nameof(name));
            }

            if (!_methods.ContainsKey(typeof(TEntity)))
            {
                _methods[typeof(TEntity)] = new Dictionary<string, ICustomSortMethod>();
            }

            return new CustomSortMethodBuilder<TEntity>(_methods, name);
        }

        public ICustomSortMethod<TEntity> GetMethod<TEntity>(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (!_methods.TryGetValue(typeof(TEntity), out var customMethods))
            {
                return null;
            }

            var comparisonType = _options.IsCaseInsensitiveForNames
                ? StringComparison.OrdinalIgnoreCase
                : StringComparison.Ordinal;

            return customMethods
                .FirstOrDefault(pair => pair.Key.Equals(name, comparisonType))
                .Value as ICustomSortMethod<TEntity>;
        }
    }
}