using Fluorite.Strainer.Models.Metadata;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Metadata;

public interface IMetadataMapper
{
    IDictionary<Type, IPropertyMetadata> DefaultMetadata { get; }

    IDictionary<Type, IDictionary<string, IPropertyMetadata>> PropertyMetadata { get; }

    IDictionary<Type, IObjectMetadata> ObjectMetadata { get; }

    void AddObjectMetadata<TEntity>(IObjectMetadata objectMetadata);

    void AddPropertyMetadata<TEntity>(IPropertyMetadata propertyMetadata);

    IObjectMetadataBuilder<TEntity> Object<TEntity>(Expression<Func<TEntity, object>> defaultSortingPropertyExpression);

    IPropertyMetadataBuilder<TEntity> Property<TEntity>(Expression<Func<TEntity, object>> propertyExpression);
}
