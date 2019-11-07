﻿using Fluorite.Strainer.Models;
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

        public PropertyMapper(IStrainerOptionsProvider optionsProvider)
        {
            _map = new Dictionary<Type, ISet<IPropertyMetadata>>();
            _options = (optionsProvider ?? throw new ArgumentNullException(nameof(optionsProvider)))
                .GetStrainerOptions();
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
            var comparisonMethod = _options.IsCaseInsensitiveForNames
                ? StringComparison.OrdinalIgnoreCase
                : StringComparison.Ordinal;

            _map.TryGetValue(typeof(TEntity), out ISet<IPropertyMetadata> metadataSet);

            if (metadataSet == null)
            {
                return null;
            }

            return metadataSet.FirstOrDefault(metadata =>
            {
                return (metadata.DisplayName ?? metadata.Name).Equals(name, comparisonMethod)
                    && (!isSortableRequired || metadata.IsSortable)
                    && (!isFilterableRequired || metadata.IsFilterable);
            });
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