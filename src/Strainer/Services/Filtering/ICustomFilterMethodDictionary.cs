using Fluorite.Strainer.Models.Filtering;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Fluorite.Strainer.Services.Filtering
{
    public interface ICustomFilterMethodDictionary:
        IReadOnlyDictionary<Type, IReadOnlyDictionary<string, ICustomFilterMethod>>,
        IReadOnlyCollection<KeyValuePair<Type, IReadOnlyDictionary<string, ICustomFilterMethod>>>,
        IEnumerable<KeyValuePair<Type, IReadOnlyDictionary<string, ICustomFilterMethod>>>,
        IEnumerable
    {
        bool TryGetMethod<TEntity>(string name, out ICustomFilterMethod<TEntity> customMethod);
    }
}