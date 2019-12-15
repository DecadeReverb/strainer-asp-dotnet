using Fluorite.Strainer.Models.Metadata;
using System;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Metadata
{
    public interface IMetadataMapper
    {
        void AddObjectMetadata<TEntity>(IObjectMetadata objectMetadata);

        void AddPropertyMetadata<TEntity>(IPropertyMetadata propertyMetadata);

        IObjectMetadataBuilder<TEntity> Object<TEntity>(Expression<Func<TEntity, object>> defaultSortingPropertyExpression);

        IPropertyMetadataBuilder<TEntity> Property<TEntity>(Expression<Func<TEntity, object>> propertyExpression);
    }
}
