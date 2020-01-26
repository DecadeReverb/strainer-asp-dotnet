using Fluorite.Strainer.Models.Metadata;
using System;
using System.Collections.Generic;

namespace Fluorite.Strainer.Services.Metadata
{
    public interface IMetadataProvider
    {
        IReadOnlyDictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>> GetAllPropertyMetadata();

        IPropertyMetadata GetDefaultMetadata<TEntity>();

        IPropertyMetadata GetDefaultMetadata(Type modelType);

        IPropertyMetadata GetPropertyMetadata<TEntity>(
            bool isSortableRequired,
            bool isFilterableRequired,
            string name);

        IPropertyMetadata GetPropertyMetadata(
            Type modelType,
            bool isSortableRequired,
            bool isFilterableRequired,
            string name);

        IEnumerable<IPropertyMetadata> GetPropertyMetadatas<TEntity>();

        IEnumerable<IPropertyMetadata> GetPropertyMetadatas(Type modelType);
    }
}
