using Fluorite.Strainer.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services
{
    public interface IPropertyMapper
    {
        void AddMetadata<TEntity>(IPropertyMetadata metadata);

        IReadOnlyDictionary<Type, IEnumerable<IPropertyMetadata>> GetAllMetadata();

        IEnumerable<IPropertyMetadata> GetAllMetadata<TEntity>();

        IPropertyMetadata GetMetadata<TEntity>(bool isSortableRequired, bool isFilterableRequired, string name);

        IPropertyMetadata GetDefaultMetadata<TEntity>();

        IPropertyBuilder<TEntity> Property<TEntity>(Expression<Func<TEntity, object>> expression);
    }
}
