using Fluorite.Strainer.Models.Metadata;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Fluorite.Strainer.Services.Metadata
{
    public class ObjectMetadataBuilder<TEntity> : IObjectMetadataBuilder<TEntity>
    {
        private readonly IDictionary<Type, IObjectMetadata> _objectMetadata;

        protected readonly string defaultSortingPropertyName;
        protected readonly PropertyInfo defaultSortingPropertyInfo;

        protected bool isDefaultSortingDescending;
        protected bool isFilterable;
        protected bool isSortable;

        public ObjectMetadataBuilder(
            IDictionary<Type, IObjectMetadata> objectMetadata,
            Expression<Func<TEntity, object>> defaultSortingPropertyExpression)
        {
            if (defaultSortingPropertyExpression is null)
            {
                throw new ArgumentNullException(nameof(defaultSortingPropertyExpression));
            }

            (defaultSortingPropertyName, defaultSortingPropertyInfo) = GetPropertyInfo(defaultSortingPropertyExpression);
            _objectMetadata = objectMetadata ?? throw new ArgumentNullException(nameof(objectMetadata));

            Save(Build());
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
            Save(Build());

            return this;
        }

        public IObjectMetadataBuilder<TEntity> IsSortable()
        {
            isSortable = true;
            Save(Build());

            return this;
        }

        public IObjectMetadataBuilder<TEntity> IsDefaultSortingDescending()
        {
            isDefaultSortingDescending = true;
            Save(Build());

            return this;
        }

        protected void Save(IObjectMetadata objectMetadata)
        {
            if (objectMetadata == null)
            {
                throw new ArgumentNullException(nameof(objectMetadata));
            }

            _objectMetadata[typeof(TEntity)] = objectMetadata;
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
