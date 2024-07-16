using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Services.Sorting;
using System.Reflection;

namespace Fluorite.Strainer.Services.Metadata;

public class PropertyMetadataBuilder<TEntity> : IPropertyMetadataBuilder<TEntity>
{
    private readonly IDictionary<Type, IDictionary<string, IPropertyMetadata>> _propertyMetadata;
    private readonly IDictionary<Type, IPropertyMetadata> _defaultMetadata;

    public PropertyMetadataBuilder(
        IDictionary<Type, IDictionary<string, IPropertyMetadata>> propertyMetadata,
        IDictionary<Type, IPropertyMetadata> defaultMetadata,
        PropertyInfo propertyInfo,
        string fullName)
    {
        _propertyMetadata = Guard.Against.Null(propertyMetadata);
        _defaultMetadata = Guard.Against.Null(defaultMetadata);
        PropertyInfo = Guard.Against.Null(propertyInfo);
        FullName = Guard.Against.NullOrWhiteSpace(fullName);

        Save(Build());
    }

    protected string DisplayName { get; set; }

    protected string FullName { get; }

    protected bool IsDefaultSorting { get; set; }

    protected bool IsDefaultSortingDescending { get; set; }

    protected bool IsFilterableValue { get; set; }

    protected bool IsSortableValue { get; set; }

    protected PropertyInfo PropertyInfo { get; }

    public virtual IPropertyMetadata Build()
    {
        return new PropertyMetadata
        {
            DisplayName = DisplayName,
            IsDefaultSorting = IsDefaultSorting,
            IsDefaultSortingDescending = IsDefaultSortingDescending,
            IsFilterable = IsFilterableValue,
            IsSortable = IsSortableValue,
            Name = FullName,
            PropertyInfo = PropertyInfo,
        };
    }

    public virtual IPropertyMetadataBuilder<TEntity> IsFilterable()
    {
        IsFilterableValue = true;
        Save(Build());

        return this;
    }

    public virtual ISortPropertyMetadataBuilder<TEntity> IsSortable()
    {
        IsSortableValue = true;
        Save(Build());

        return new SortPropertyMetadataBuilder<TEntity>(_propertyMetadata, _defaultMetadata, PropertyInfo, FullName, Build());
    }

    public virtual IPropertyMetadataBuilder<TEntity> HasDisplayName(string displayName)
    {
        Guard.Against.NullOrWhiteSpace(displayName);

        DisplayName = displayName;
        Save(Build());

        return this;
    }

    protected void Save(IPropertyMetadata propertyMetadata)
    {
        Guard.Against.Null(propertyMetadata);

        if (!_propertyMetadata.ContainsKey(typeof(TEntity)))
        {
            _propertyMetadata[typeof(TEntity)] = new Dictionary<string, IPropertyMetadata>();
        }

        if (propertyMetadata.IsDefaultSorting)
        {
            _defaultMetadata[typeof(TEntity)] = propertyMetadata;
        }

        var metadataKey = propertyMetadata.DisplayName ?? propertyMetadata.Name;

        _propertyMetadata[typeof(TEntity)][metadataKey] = propertyMetadata;
    }
}
