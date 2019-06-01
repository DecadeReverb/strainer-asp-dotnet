using System;
using System.Linq.Expressions;
using System.Reflection;
using Strainer.Models;

namespace Strainer.Services
{
    public interface IStrainerPropertyMapper
    {
        void AddMap<TEntity>(PropertyInfo propertyInfo, IStrainerPropertyMetadata metadata);
        (string, PropertyInfo) FindProperty<TEntity>(bool canSortRequired, bool canFilterRequired, string name, bool isCaseSensitive);
        IStrainerPropertyBuilder<TEntity> Property<TEntity>(Expression<Func<TEntity, object>> expression);
    }
}
