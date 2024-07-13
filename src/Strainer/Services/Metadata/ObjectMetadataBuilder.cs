using Fluorite.Strainer.Models.Metadata;
using System.Linq.Expressions;
using System.Reflection;

namespace Fluorite.Strainer.Services.Metadata;

public class ObjectMetadataBuilder<TEntity> : IObjectMetadataBuilder<TEntity>
{
    private readonly IDictionary<Type, IObjectMetadata> _objectMetadata;
    private readonly string _defaultSortingPropertyName;
    private readonly PropertyInfo _defaultSortingPropertyInfo;

    public ObjectMetadataBuilder(
        IPropertyInfoProvider propertyInfoProvider,
        IDictionary<Type, IObjectMetadata> objectMetadata,
        Expression<Func<TEntity, object>> defaultSortingPropertyExpression)
    {
        Guard.Against.Null(propertyInfoProvider);
        Guard.Against.Null(objectMetadata);
        Guard.Against.Null(defaultSortingPropertyExpression);

        (_defaultSortingPropertyInfo, _defaultSortingPropertyName) = propertyInfoProvider.GetPropertyInfoAndFullName(defaultSortingPropertyExpression);
        _objectMetadata = objectMetadata;
        Save(Build());
    }

    protected bool IsDefaultSortingDescendingValue { get; set; }

    protected bool IsFilterableValue { get; set; }

    protected bool IsSortableValue { get; set; }

    public IObjectMetadata Build()
    {
        return new ObjectMetadata
        {
            DefaultSortingPropertyInfo = _defaultSortingPropertyInfo,
            DefaultSortingPropertyName = _defaultSortingPropertyName,
            IsDefaultSortingDescending = IsDefaultSortingDescendingValue,
            IsFilterable = IsFilterableValue,
            IsSortable = IsSortableValue,
        };
    }

    public IObjectMetadataBuilder<TEntity> IsFilterable()
    {
        IsFilterableValue = true;
        Save(Build());

        return this;
    }

    public IObjectMetadataBuilder<TEntity> IsSortable()
    {
        IsSortableValue = true;
        Save(Build());

        return this;
    }

    public IObjectMetadataBuilder<TEntity> IsDefaultSortingDescending()
    {
        IsDefaultSortingDescendingValue = true;
        Save(Build());

        return this;
    }

    protected void Save(IObjectMetadata objectMetadata)
    {
        _objectMetadata[typeof(TEntity)] = Guard.Against.Null(objectMetadata);
    }
}
