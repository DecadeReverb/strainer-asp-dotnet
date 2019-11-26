using Fluorite.Strainer.Models.Metadata;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Fluorite.Strainer.Services.Metadata
{
    public class ObjectMetadataBuilder<TEntity> : IObjectMetadataBuilder<TEntity>
    {
        private readonly IMetadataMapper _mapper;

        protected readonly string defaultSortingPropertyName;
        protected readonly PropertyInfo defaultSortingPropertyInfo;

        protected bool isDefaultSortingDescending;
        protected bool isFilterable;
        protected bool isSortable;

        public ObjectMetadataBuilder(
            IMetadataMapper propertyMetadataMapper,
            Expression<Func<TEntity, object>> defaultSortingPropertyExpression)
        {
            if (defaultSortingPropertyExpression is null)
            {
                throw new ArgumentNullException(nameof(defaultSortingPropertyExpression));
            }

            _mapper = propertyMetadataMapper ?? throw new ArgumentNullException(nameof(propertyMetadataMapper));

            (defaultSortingPropertyName, defaultSortingPropertyInfo) = GetPropertyInfo(defaultSortingPropertyExpression);
        }

        public IObjectMetadata Build()
        {
            return new ObjectMetadata
            {
                DefaultSortingPropertyInfo = defaultSortingPropertyInfo,
                DefaultSortingPropertyName = defaultSortingPropertyName,
                IsDefaultSortingDescending = isDefaultSortingDescending,
                IsFilterable = isFilterable,
                IsSortable = isSortable,
            };
        }

        public IObjectMetadataBuilder<TEntity> IsFilterable()
        {
            isFilterable = true;
            UpdateMap(Build());

            return this;
        }

        public IObjectMetadataBuilder<TEntity> IsSortable()
        {
            isSortable = true;
            UpdateMap(Build());

            return this;
        }

        public IObjectMetadataBuilder<TEntity> IsDefaultSortingDescending()
        {
            isDefaultSortingDescending = true;
            UpdateMap(Build());

            return this;
        }

        protected void UpdateMap(IObjectMetadata objectMetadata)
        {
            if (objectMetadata == null)
            {
                throw new ArgumentNullException(nameof(objectMetadata));
            }

            _mapper.AddObjectMetadata<TEntity>(objectMetadata);
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
