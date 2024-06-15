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
        IDictionary<Type, IObjectMetadata> objectMetadata,
        Expression<Func<TEntity, object>> defaultSortingPropertyExpression)
    {
        if (defaultSortingPropertyExpression is null)
        {
            throw new ArgumentNullException(nameof(defaultSortingPropertyExpression));
        }

        (_defaultSortingPropertyName, _defaultSortingPropertyInfo) = GetPropertyInfo(defaultSortingPropertyExpression);
        _objectMetadata = objectMetadata ?? throw new ArgumentNullException(nameof(objectMetadata));

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
        if (objectMetadata == null)
        {
            throw new ArgumentNullException(nameof(objectMetadata));
        }

        _objectMetadata[typeof(TEntity)] = objectMetadata;
    }

    private (string, PropertyInfo) GetPropertyInfo(Expression<Func<TEntity, object>> expression)
    {
        if (expression == null)
        {
            throw new ArgumentNullException(nameof(expression));
        }

        if (expression.Body is not MemberExpression body)
        {
            var ubody = expression.Body as UnaryExpression;
            body = ubody.Operand as MemberExpression;
        }

        var propertyInfo = body?.Member as PropertyInfo;
        var stack = new Stack<string>();
        while (body != null)
        {
            stack.Push(body.Member.Name);
            body = body.Expression as MemberExpression;
        }

        return (string.Join(".", stack.ToArray()), propertyInfo);
    }
}
