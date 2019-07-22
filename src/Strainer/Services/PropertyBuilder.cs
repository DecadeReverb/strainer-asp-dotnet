using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services.Sorting;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Fluorite.Strainer.Services
{
    public class PropertyBuilder<TEntity> : IPropertyBuilder<TEntity>
    {
        private readonly Expression<Func<TEntity, object>> _expression;
        private readonly IPropertyMapper _mapper;
        private readonly PropertyMetadata _propertyMetadata;

        public PropertyBuilder(IPropertyMapper strainerPropertyMapper, Expression<Func<TEntity, object>> expression)
        {
            _expression = expression ?? throw new ArgumentNullException(nameof(expression));
            _mapper = strainerPropertyMapper ?? throw new ArgumentNullException(nameof(strainerPropertyMapper));
            var (name, propertyInfo) = GetPropertyInfo(expression);
            _propertyMetadata = new PropertyMetadata
            {
                Name = name,
                PropertyInfo = propertyInfo,
            };
        }

        public virtual IPropertyMetadata Build() => _propertyMetadata;

        public virtual IPropertyBuilder<TEntity> CanFilter()
        {
            _propertyMetadata.IsFilterable = true;
            UpdateMap(_propertyMetadata);

            return this;
        }

        public virtual ISortPropertyBuilder<TEntity> CanSort()
        {
            _propertyMetadata.IsSortable = true;
            UpdateMap(_propertyMetadata);

            return new SortPropertyBuilder<TEntity>(_mapper, _expression, _propertyMetadata);
        }

        public virtual IPropertyBuilder<TEntity> HasDisplayName(string displayName)
        {
            if (string.IsNullOrWhiteSpace(displayName))
            {
                throw new ArgumentException(
                    $"{nameof(displayName)} cannot be null, empty " +
                    $"or contain only whitespace characaters.",
                    nameof(displayName));
            }

            _propertyMetadata.DisplayName = displayName;
            UpdateMap(_propertyMetadata);

            return this;
        }

        protected void UpdateMap(IPropertyMetadata propertyMetadata)
        {
            if (propertyMetadata == null)
            {
                throw new ArgumentNullException(nameof(propertyMetadata));
            }

            _mapper.AddMap<TEntity>(propertyMetadata);
        }

        private (string, PropertyInfo) GetPropertyInfo(Expression<Func<TEntity, object>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

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
