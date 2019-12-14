using Fluorite.Strainer.Models.Metadata;
using System;
using System.Collections.Generic;

namespace Fluorite.Strainer.Services.Metadata
{
    public interface IMetadataProvidersFacade
    {
        IPropertyMetadata GetDefaultMetadata<TEntity>();

        IPropertyMetadata GetDefaultMetadata(Type modelType);

        IPropertyMetadata GetMetadata<TEntity>(
            bool isSortableRequired,
            bool isFilterableRequired,
            string name);

        IPropertyMetadata GetMetadata(
            Type modelType,
            bool isSortableRequired,
            bool isFilterableRequired,
            string name);

        IEnumerable<IPropertyMetadata> GetMetadatas<TEntity>();

        IEnumerable<IPropertyMetadata> GetMetadatas(Type modelType);
    }
}
