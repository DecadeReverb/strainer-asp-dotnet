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

        public PropertyBuilder(IPropertyMapper strainerPropertyMapper, Expression<Func<TEntity, object>> expression)
        {
            _expression = expression ?? throw new ArgumentNullException(nameof(expression));
            _mapper = strainerPropertyMapper ?? throw new ArgumentNullException(nameof(strainerPropertyMapper));
            (NameProperty, PropertyInfoProperty) = GetPropertyInfo(expression);
        }

        protected string DisplayNameProperty { get; set; }
        protected bool IsDefaultSortingProperty { get; set; }
        protected bool IsDefaultSortingDescendingProperty { get; set; }
        protected bool IsFilterableProperty { get; set; }
        protected bool IsSortableProperty { get; set; }
        protected string NameProperty { get; set; }
        protected PropertyInfo PropertyInfoProperty { get; set; }

        public virtual IPropertyMetadata Build()
        {
            return new PropertyMetadata
            {
                DisplayName = DisplayNameProperty,
                IsDefaultSorting = IsDefaultSortingProperty,
                IsDefaultSortingDescending = IsDefaultSortingDescendingProperty,
                IsFilterable = IsFilterableProperty,
                IsSortable = IsSortableProperty,
                Name = NameProperty,
                PropertyInfo = PropertyInfoProperty,
            };
        }

        public virtual IPropertyBuilder<TEntity> IsFilterable()
        {
            IsFilterableProperty = true;
            UpdateMap(Build());

            return this;
        }

        public virtual ISortPropertyBuilder<TEntity> IsSortable()
        {
            IsSortableProperty = true;
            UpdateMap(Build());

            return new SortPropertyBuilder<TEntity>(_mapper, _expression, Build());
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

            DisplayNameProperty = displayName;
            UpdateMap(Build());

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
