﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Fluorite.Strainer.Models;

namespace Fluorite.Strainer.Services
{
    public class StrainerPropertyBuilder<TEntity> : IStrainerPropertyBuilder<TEntity>
    {
        public StrainerPropertyBuilder(IStrainerPropertyMapper strainerPropertyMapper, Expression<Func<TEntity, object>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            Mapper = strainerPropertyMapper ?? throw new ArgumentNullException(nameof(strainerPropertyMapper));
            (FullName, PropertyInfo) = GetPropertyInfo(expression);
            Name = FullName;
            IsFilterable = false;
            IsSortable = false;
        }

        public string FullName { get; protected set; }
        public bool IsFilterable { get; protected set; }
        public bool IsSortable { get; protected set; }
        public string Name { get; protected set; }

        protected PropertyInfo PropertyInfo { get; }
        protected IStrainerPropertyMapper Mapper { get; }

        public IStrainerPropertyBuilder<TEntity> CanFilter()
        {
            IsFilterable = true;
            UpdateMap();

            return this;
        }

        public IStrainerPropertyBuilder<TEntity> CanSort()
        {
            IsSortable = true;
            UpdateMap();

            return this;
        }

        public IStrainerPropertyBuilder<TEntity> HasName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(
                    $"{nameof(name)} cannot be null, empty " +
                    $"or contain only whitespace characaters.",
                    nameof(name));
            }

            Name = name;
            UpdateMap();

            return this;
        }

        private void UpdateMap()
        {
            var metadata = new StrainerPropertyMetadata()
            {
                Name = Name,
                FullName = FullName,
                CanFilter = IsFilterable,
                CanSort = IsSortable
            };

            Mapper.AddMap<TEntity>(PropertyInfo, metadata);
        }

        private static (string, PropertyInfo) GetPropertyInfo(Expression<Func<TEntity, object>> expression)
        {
            if (!(expression.Body is MemberExpression body))
            {
                var ubody = expression.Body as UnaryExpression;
                body = ubody.Operand as MemberExpression;
            }

            var propertyInfo = body?.Member as PropertyInfo;
            var stack = new Stack<string>();
            while (body != null)
            {
                stack.Push(body.Member.Name);
                body = body.Expression as MemberExpression;
            }

            return (string.Join(".", stack.ToArray()), propertyInfo);
        }
    }
}