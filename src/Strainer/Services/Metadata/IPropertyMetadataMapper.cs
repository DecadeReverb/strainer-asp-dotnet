using Fluorite.Strainer.Models;
using System;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Metadata
{
    public interface IPropertyMetadataMapper
    {
        void AddMetadata<TEntity>(IPropertyMetadata metadata);

        IPropertyMetadataBuilder<TEntity> Property<TEntity>(Expression<Func<TEntity, object>> expression);
    }
}
