using Fluorite.Strainer.Models.Sorting;
using System;
using System.Collections.Generic;

namespace Fluorite.Strainer.Services.Sorting
{
    public interface ICustomSortMethodMapper
    {
        IReadOnlyDictionary<Type, IReadOnlyDictionary<string, object>> Methods { get; }

        void AddMap<TEntity>(ICustomSortMethod<TEntity> sortMethod);

        ICustomSortMethodBuilder<TEntity> CustomMethod<TEntity>(string name);

        ICustomSortMethod<TEntity> GetMethod<TEntity>(string name);
    }
}
