using Fluorite.Strainer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services
{
    public class PropertyMapper : IPropertyMapper
    {
        private readonly Dictionary<Type, ISet<IPropertyMetadata>> _map;
        private readonly StrainerOptions _options;

        public PropertyMapper(StrainerOptions options)
        {
            _map = new Dictionary<Type, ISet<IPropertyMetadata>>();
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public IReadOnlyDictionary<Type, IEnumerable<IPropertyMetadata>> Properties
        {
            get
            {
                var newdict = _map.ToDictionary(k => k.Key, v => v.Value as IEnumerable<IPropertyMetadata>);

                return new ReadOnlyDictionary<Type, IEnumerable<IPropertyMetadata>>(newdict);
            }
        }

        public void AddMap<TEntity>(IPropertyMetadata metadata)
        {
            if (metadata == null)
            {
                throw new ArgumentNullException(nameof(metadata));
            }

            if (!_map.Keys.Contains(typeof(TEntity)))
            {
                _map[typeof(TEntity)] = new HashSet<IPropertyMetadata>();
            }

            _map[typeof(TEntity)].Add(metadata);
        }

        public IPropertyMetadata FindProperty<TEntity>(
            bool isSortableRequired,
            bool isFilterableRequired,
            string name)
        {
            try
            {
                var comparisonMethod = _options.CaseSensitive
                    ? StringComparison.Ordinal
                    : StringComparison.OrdinalIgnoreCase;

                return _map[typeof(TEntity)]
                    .FirstOrDefault(metadata =>
                        (metadata.DisplayName ?? metadata.Name).Equals(name, comparisonMethod)
                        && (!isSortableRequired || metadata.IsSortable)
                        && (!isFilterableRequired || metadata.IsFilterable));
            }
            catch (Exception ex) when (ex is KeyNotFoundException || ex is ArgumentNullException)
            {
                return null;
            }
        }

        public IPropertyBuilder<TEntity> Property<TEntity>(Expression<Func<TEntity, object>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            if (!_map.ContainsKey(typeof(TEntity)))
            {
                _map[typeof(TEntity)] = new HashSet<IPropertyMetadata>();
            }

            return new PropertyBuilder<TEntity>(this, expression);
        }
    }
}
