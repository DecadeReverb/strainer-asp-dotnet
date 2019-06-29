using Fluorite.Strainer.Models;
using System;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services
{
    public interface IStrainerPropertyMapper
    {
        void AddMap<TEntity>(IStrainerPropertyMetadata metadata);
        IStrainerPropertyMetadata FindProperty<TEntity>(bool canSortRequired, bool canFilterRequired, string name, bool isCaseSensitive);
        IStrainerPropertyBuilder<TEntity> Property<TEntity>(Expression<Func<TEntity, object>> expression);
    }
}
