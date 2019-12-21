using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Filtering;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Fluorite.Strainer.Services.Filtering
{
    public class CustomFilterMethodMapper : ICustomFilterMethodMapper
    {
        private readonly Dictionary<Type, IDictionary<string, ICustomFilterMethod>> _methods;
        private readonly StrainerOptions _options;

        public CustomFilterMethodMapper(IStrainerOptionsProvider optionsProvider)
        {
            _methods = new Dictionary<Type, IDictionary<string, ICustomFilterMethod>>();
            _options = (optionsProvider ?? throw new ArgumentNullException(nameof(optionsProvider)))
                .GetStrainerOptions();
        }

        public IReadOnlyDictionary<Type, IReadOnlyDictionary<string, ICustomFilterMethod>> Methods
        {
            get
            {
                var dictionary = _methods.ToDictionary(
                    k => k.Key,
                    v => new ReadOnlyDictionary<string, ICustomFilterMethod>(v.Value) as IReadOnlyDictionary<string, ICustomFilterMethod>);

                return new ReadOnlyDictionary<Type, IReadOnlyDictionary<string, ICustomFilterMethod>>(dictionary);
            }
        }

        public void AddMap<TEntity>(ICustomFilterMethod<TEntity> customMethod)
        {
            if (customMethod == null)
            {
                throw new ArgumentNullException(nameof(customMethod));
            }

            if (!_methods.ContainsKey(typeof(TEntity)))
            {
                _methods[typeof(TEntity)] = new Dictionary<string, ICustomFilterMethod>();
            }

            _methods[typeof(TEntity)][customMethod.Name] = customMethod;
        }

        public ICustomFilterMethodBuilder<TEntity> CustomMethod<TEntity>(string name)
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
                _methods[typeof(TEntity)] = new Dictionary<string, ICustomFilterMethod>();
            }

            return new CustomFilterMethodBuilder<TEntity>(_methods, name);
        }

        public ICustomFilterMethod<TEntity> GetMethod<TEntity>(string name)
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
                .Value as ICustomFilterMethod<TEntity>;
        }
    }
}