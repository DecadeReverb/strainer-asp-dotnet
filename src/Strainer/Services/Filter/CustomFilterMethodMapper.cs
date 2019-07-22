using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Filtering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fluorite.Strainer.Services.Filter
{
    public class CustomFilterMethodMapper : ICustomFilterMethodMapper
    {
        private readonly Dictionary<Type, Dictionary<string, object>> _methods;
        private readonly StrainerOptions _options;

        public CustomFilterMethodMapper(StrainerOptions options)
        {
            _methods = new Dictionary<Type, Dictionary<string, object>>();
            _options = options;
        }

        public void AddMap<TEntity>(ICustomFilterMethod<TEntity> sortMethod)
        {
            if (sortMethod == null)
            {
                throw new ArgumentNullException(nameof(sortMethod));
            }

            if (!_methods.ContainsKey(typeof(TEntity)))
            {
                _methods[typeof(TEntity)] = new Dictionary<string, object>();
            }

            _methods[typeof(TEntity)][sortMethod.Name] = sortMethod;
        }

        public ICustomFilterMethod<TEntity> GetMethod<TEntity>(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (!_methods.ContainsKey(typeof(TEntity)))
            {
                return null;
            }

            var comparisonType = _options.CaseSensitive
                ? StringComparison.Ordinal
                : StringComparison.OrdinalIgnoreCase;

            return _methods[typeof(TEntity)]
                .FirstOrDefault(pair => pair.Key.Equals(name, comparisonType))
                .Value as ICustomFilterMethod<TEntity>;
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
                _methods[typeof(TEntity)] = new Dictionary<string, object>();
            }

            return new CustomFilterMethodBuilder<TEntity>(this, name);
        }
    }
}