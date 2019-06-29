using Fluorite.Strainer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services
{
    public class StrainerPropertyMapper : IStrainerPropertyMapper
    {
        private readonly Dictionary<Type, ISet<IStrainerPropertyMetadata>> _map;

        public StrainerPropertyMapper()
        {
            _map = new Dictionary<Type, ISet<IStrainerPropertyMetadata>>();
        }

        public void AddMap<TEntity>(IStrainerPropertyMetadata metadata)
        {
            if (metadata == null)
            {
                throw new ArgumentNullException(nameof(metadata));
            }

            if (!_map.Keys.Contains(typeof(TEntity)))
            {
                _map[typeof(TEntity)] = new HashSet<IStrainerPropertyMetadata>();
            }

            _map[typeof(TEntity)].Add(metadata);
        }

        public IStrainerPropertyMetadata FindProperty<TEntity>(
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

                return _map[typeof(TEntity)]
                    .FirstOrDefault(metadata =>
                        metadata.DisplayName.Equals(name, comparisonMethod)
                        && (!canSortRequired || metadata.IsSortable)
                        && (!canFilterRequired || metadata.IsFilterable));
            }
            catch (Exception ex) when (ex is KeyNotFoundException || ex is ArgumentNullException)
            {
                return null;
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
                _map[typeof(TEntity)] = new HashSet<IStrainerPropertyMetadata>();
            }

            return new StrainerPropertyBuilder<TEntity>(this, expression);
        }
    }
}
