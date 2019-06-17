using Fluorite.Strainer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Fluorite.Strainer.Services
{
	public class StrainerPropertyMapper : IStrainerPropertyMapper
    {
        private readonly Dictionary<Type, ISet<KeyValuePair<PropertyInfo, IStrainerPropertyMetadata>>> _map;

        public StrainerPropertyMapper()
        {
            _map = new Dictionary<Type, ISet<KeyValuePair<PropertyInfo, IStrainerPropertyMetadata>>>();
        }

        public void AddMap<TEntity>(PropertyInfo propertyInfo, IStrainerPropertyMetadata metadata)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }

            if (metadata == null)
            {
                throw new ArgumentNullException(nameof(metadata));
            }

            var pair = new KeyValuePair<PropertyInfo, IStrainerPropertyMetadata>(propertyInfo, metadata);
            if (!_map.Keys.Contains(typeof(TEntity)))
            {
                _map[typeof(TEntity)] = new HashSet<KeyValuePair<PropertyInfo, IStrainerPropertyMetadata>>();
            }

            _map[typeof(TEntity)].Add(pair);
        }

        public (string, PropertyInfo) FindProperty<TEntity>(
            bool canSortRequired,
            bool canFilterRequired,
            string name,
            bool isCaseSensitive)
        {
            try
            {
                var comparisonMethod = isCaseSensitive
                    ? StringComparison.Ordinal
                    : StringComparison.OrdinalIgnoreCase;
                var result = _map[typeof(TEntity)]
                    .FirstOrDefault(pair =>
                        pair.Value.DisplayName.Equals(name, comparisonMethod)
                        && (!canSortRequired || pair.Value.IsSortable)
                        && (!canFilterRequired || pair.Value.IsFilterable));

                return (result.Value?.Name, result.Key);
            }
            catch (Exception ex) when (ex is KeyNotFoundException || ex is ArgumentNullException)
            {
                return (null, null);
            }
        }

        public IStrainerPropertyBuilder<TEntity> Property<TEntity>(Expression<Func<TEntity, object>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            if (!_map.ContainsKey(typeof(TEntity)))
            {
                _map[typeof(TEntity)] = new HashSet<KeyValuePair<PropertyInfo, IStrainerPropertyMetadata>>();
            }

            return new StrainerPropertyBuilder<TEntity>(this, expression);
        }
    }
}
