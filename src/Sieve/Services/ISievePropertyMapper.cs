using System;
using System.Linq.Expressions;
using System.Reflection;
using Sieve.Models;

namespace Sieve.Services
{
    public interface ISievePropertyMapper
    {
        void AddMap<TEntity>(PropertyInfo propertyInfo, ISievePropertyMetadata metadata);
        (string, PropertyInfo) FindProperty<TEntity>(bool canSortRequired, bool canFilterRequired, string name, bool isCaseSensitive);
        ISievePropertyBuilder<TEntity> Property<TEntity>(Expression<Func<TEntity, object>> expression);
    }
}
