using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Fluorite.Strainer.Models;

namespace Fluorite.Strainer.Services
{
    public class StrainerPropertyBuilder<TEntity> : IStrainerPropertyBuilder<TEntity>
    {
        private readonly StrainerPropertyMetadata _propertyMetadata;

        public StrainerPropertyBuilder(IStrainerPropertyMapper strainerPropertyMapper, Expression<Func<TEntity, object>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            Mapper = strainerPropertyMapper ?? throw new ArgumentNullException(nameof(strainerPropertyMapper));
            var (name, propertyInfo) = GetPropertyInfo(expression);
            PropertyInfo = propertyInfo;
            _propertyMetadata = new StrainerPropertyMetadata
            {
                Name = name,
                PropertyInfo = PropertyInfo,
            };
        }

        protected PropertyInfo PropertyInfo { get; }
        protected IStrainerPropertyMapper Mapper { get; }

        public IStrainerPropertyMetadata Build() => _propertyMetadata;

        public IStrainerPropertyBuilder<TEntity> CanFilter()
        {
            _propertyMetadata.IsFilterable = true;
            UpdateMap();

            return this;
        }

        public IStrainerPropertyBuilder<TEntity> CanSort()
        {
            _propertyMetadata.IsSortable = true;
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

            _propertyMetadata.DisplayName = displayName;
            UpdateMap();

            return this;
        }

        private void UpdateMap()
        {
            Mapper.AddMap<TEntity>(_propertyMetadata);
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
