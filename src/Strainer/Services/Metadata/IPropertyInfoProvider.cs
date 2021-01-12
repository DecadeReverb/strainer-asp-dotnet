using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Fluorite.Strainer.Services.Metadata
{
    public interface IPropertyInfoProvider
    {
        (string FullPropertyName, PropertyInfo PropertyInfo) GetPropertyInfoAndFullName<TEntity>(
            Expression<Func<TEntity, object>> expression);
    }
}
