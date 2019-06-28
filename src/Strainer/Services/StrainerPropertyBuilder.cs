using System;
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
            (Name, PropertyInfo) = GetPropertyInfo(expression);
            DisplayName = Name;
            IsFilterable = false;
            IsSortable = false;
        }

        public string DisplayName { get; protected set; }
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

        public IStrainerPropertyBuilder<TEntity> HasDisplayName(string displayName)
        {
            if (string.IsNullOrWhiteSpace(displayName))
            {
                throw new ArgumentException(
                    $"{nameof(displayName)} cannot be null, empty " +
                    $"or contain only whitespace characaters.",
                    nameof(displayName));
            }

            DisplayName = displayName;
            UpdateMap();

            return this;
        }

        private void UpdateMap()
        {
            var metadata = new StrainerPropertyMetadata()
            {
                DisplayName = DisplayName,
                IsFilterable = IsFilterable,
                IsSortable = IsSortable,
                Name = Name,
                PropertyInfo = PropertyInfo,
            };

            Mapper.AddMap<TEntity>(metadata);
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
