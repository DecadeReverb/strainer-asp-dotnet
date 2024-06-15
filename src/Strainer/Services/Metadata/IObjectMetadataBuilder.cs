using Fluorite.Strainer.Models.Metadata;

namespace Fluorite.Strainer.Services.Metadata;

public interface IObjectMetadataBuilder<TEntity>
{
    /// <summary>
    /// Builds the object metadata.
    /// </summary>
    /// <returns>
    /// An instance of <see cref="IObjectMetadata"/>.
    /// </returns>
    IObjectMetadata Build();

    IObjectMetadataBuilder<TEntity> IsFilterable();

    IObjectMetadataBuilder<TEntity> IsSortable();

    IObjectMetadataBuilder<TEntity> IsDefaultSortingDescending();
}
