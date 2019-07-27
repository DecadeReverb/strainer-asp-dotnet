using Fluorite.Strainer.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services
{
    public interface IPropertyMapper
    {
        IReadOnlyDictionary<Type, IEnumerable<IPropertyMetadata>> Properties { get; }

        void AddMap<TEntity>(IPropertyMetadata metadata);
        IPropertyMetadata FindProperty<TEntity>(bool isSortableRequired, bool isFilterableRequired, string name);
        IPropertyBuilder<TEntity> Property<TEntity>(Expression<Func<TEntity, object>> expression);
    }
}
