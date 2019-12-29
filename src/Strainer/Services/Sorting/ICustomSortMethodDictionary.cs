using Fluorite.Strainer.Models.Sorting;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Fluorite.Strainer.Services.Sorting
{
    public interface ICustomSortMethodDictionary :
        IReadOnlyDictionary<Type, IReadOnlyDictionary<string, ICustomSortMethod>>,
        IReadOnlyCollection<KeyValuePair<Type, IReadOnlyDictionary<string, ICustomSortMethod>>>,
        IEnumerable<KeyValuePair<Type, IReadOnlyDictionary<string, ICustomSortMethod>>>,
        IEnumerable
    {
        bool TryGetMethod<TEntity>(string name, out ICustomSortMethod<TEntity> customMethod);
    }
}