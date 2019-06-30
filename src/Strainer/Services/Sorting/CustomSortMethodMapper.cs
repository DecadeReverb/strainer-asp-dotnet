﻿using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Sorting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fluorite.Strainer.Services.Sorting
{
    public class CustomSortMethodMapper : ICustomSortMethodMapper
    {
        private readonly Dictionary<Type, Dictionary<string, object>> _methods;
        private readonly StrainerOptions _options;

        public CustomSortMethodMapper(IOptions<StrainerOptions> options)
        {
            _methods = new Dictionary<Type, Dictionary<string, object>>();
            _options = options.Value;
        }

        public void AddMethod<TEntity>(ICustomSortMethod<TEntity> sortMethod)
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

        public ICustomSortMethod<TEntity> GetMethod<TEntity>(string name)
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
                .Value as ICustomSortMethod<TEntity>;
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
                _methods[typeof(TEntity)] = new Dictionary<string, object>();
            }

            return new CustomSortMethodBuilder<TEntity>(this, name);
        }
    }
}