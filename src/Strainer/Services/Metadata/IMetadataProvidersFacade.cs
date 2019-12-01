﻿using Fluorite.Strainer.Models.Metadata;

namespace Fluorite.Strainer.Services.Metadata
{
    public interface IMetadataProvidersFacade
    {
        IPropertyMetadata GetDefaultMetadata<TEntity>();

        IPropertyMetadata GetMetadata<TEntity>(
            bool isSortingRequired,
            bool isFilteringRequired,
            string name);
    }
}
