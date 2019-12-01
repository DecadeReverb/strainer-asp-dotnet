using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Services.Sorting;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Fluorite.Strainer.Services.Metadata
{
    public class PropertyMetadataBuilder<TEntity> : IPropertyMetadataBuilder<TEntity>
    {
        private readonly Expression<Func<TEntity, object>> _expression;
        private readonly IMetadataMapper _mapper;

        protected string displayName;
        protected bool isDefaultSorting;
        protected bool isDefaultSortingDescending;
        protected bool isFilterable;
        protected bool isSortable;
        protected string name;
        protected PropertyInfo propertyInfo;

        public PropertyMetadataBuilder(IMetadataMapper strainerPropertyMapper, Expression<Func<TEntity, object>> expression)
        {
            _expression = expression ?? throw new ArgumentNullException(nameof(expression));
            _mapper = strainerPropertyMapper ?? throw new ArgumentNullException(nameof(strainerPropertyMapper));

            (name, propertyInfo) = GetPropertyInfo(expression);

            UpdateMap(Build());
        }

        public virtual IPropertyMetadata Build()
        {
            return new PropertyMetadata
            {
                DisplayName = displayName,
                IsDefaultSorting = isDefaultSorting,
                IsDefaultSortingDescending = isDefaultSortingDescending,
                IsFilterable = isFilterable,
                IsSortable = isSortable,
                Name = name,
                PropertyInfo = propertyInfo,
            };
        }

        public virtual IPropertyMetadataBuilder<TEntity> IsFilterable()
        {
            isFilterable = true;
            UpdateMap(Build());

            return this;
        }

        public virtual ISortPropertyMetadataBuilder<TEntity> IsSortable()
        {
            isSortable = true;
            UpdateMap(Build());

            return new SortPropertyMetadataBuilder<TEntity>(_mapper, _expression, Build());
        }

        public virtual IPropertyMetadataBuilder<TEntity> HasDisplayName(string displayName)
        {
            if (string.IsNullOrWhiteSpace(displayName))
            {
                throw new ArgumentException(
                    $"{nameof(displayName)} cannot be null, empty " +
                    $"or contain only whitespace characaters.",
                    nameof(displayName));
            }

            this.displayName = displayName;
            UpdateMap(Build());

            return this;
        }

        protected void UpdateMap(IPropertyMetadata propertyMetadata)
        {
            if (propertyMetadata == null)
            {
                throw new ArgumentNullException(nameof(propertyMetadata));
            }

            _mapper.AddPropertyMetadata<TEntity>(propertyMetadata);
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
