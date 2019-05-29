using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Sieve.Services
{
	public class SievePropertyMapper : ISievePropertyMapper
    {
        private readonly Dictionary<Type, ICollection<KeyValuePair<PropertyInfo, ISievePropertyMetadata>>> _map;

        public SievePropertyMapper()
        {
            _map = new Dictionary<Type, ICollection<KeyValuePair<PropertyInfo, ISievePropertyMetadata>>>();
        }

        public void AddMap<TEntity>(PropertyInfo propertyInfo, ISievePropertyMetadata metadata)
        {
            if (propertyInfo == null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }

            if (metadata == null)
            {
                throw new ArgumentNullException(nameof(metadata));
            }

            var pair = new KeyValuePair<PropertyInfo, ISievePropertyMetadata>(propertyInfo, metadata);
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
                var result = _map[typeof(TEntity)]
                    .FirstOrDefault(kv =>
                    kv.Value.Name.Equals(name, isCaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase)
                    && (!canSortRequired || kv.Value.CanSort)
                    && (!canFilterRequired || kv.Value.CanFilter));

                return (result.Value?.FullName, result.Key);
            }
            catch (Exception ex) when (ex is KeyNotFoundException || ex is ArgumentNullException)
            {
                return (null, null);
            }
        }

        public ISievePropertyBuilder<TEntity> Property<TEntity>(Expression<Func<TEntity, object>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            if (!_map.ContainsKey(typeof(TEntity)))
            {
                _map.Add(typeof(TEntity), new List<KeyValuePair<PropertyInfo, ISievePropertyMetadata>>());
            }

            return new SievePropertyBuilder<TEntity>(this, expression);
        }
    }
}
