using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Services.Sorting;

namespace Fluorite.Strainer.Services.Metadata;

public interface IPropertyMetadataBuilder<TEntity>
{
    /// <summary>
    /// Builds the property metadata.
    /// </summary>
    /// <returns>
    /// An instance of <see cref="IPropertyMetadata"/>.
    /// </returns>
    IPropertyMetadata Build();

    IPropertyMetadataBuilder<TEntity> IsFilterable();

    ISortPropertyMetadataBuilder<TEntity> IsSortable();

    IPropertyMetadataBuilder<TEntity> HasDisplayName(string displayName);
}
