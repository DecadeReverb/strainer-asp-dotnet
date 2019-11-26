﻿using Fluorite.Strainer.Models;
using System;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Metadata
{
    public interface IPropertyMetadataMapper
    {
        void AddPropertyMetadata<TEntity>(IPropertyMetadata propertyMetadata);

        void AddObjectMetadata<TEntity>(IObjectMetadata objectMetadata);

        IPropertyMetadataBuilder<TEntity> Property<TEntity>(Expression<Func<TEntity, object>> expression);

        IObjectMetadataBuilder<TEntity> Object<TEntity>(Expression<Func<TEntity, object>> expression);
    }
}
