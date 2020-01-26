using Fluorite.Strainer.Models.Metadata;
using System;
using System.Collections.Generic;

namespace Fluorite.Strainer.Services.Metadata
{
    public interface IMetadataStore
    {
        IReadOnlyDictionary<Type, IObjectMetadata> GetAllObjectMetadata();

        IReadOnlyDictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>> GetAllPropertyMetadata();

        IPropertyMetadata GetDefaultPropertyMetadata<TEntity>();

        IPropertyMetadata GetDefaultPropertyMetadata(Type modelType);

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
