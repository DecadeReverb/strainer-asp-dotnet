using Fluorite.Strainer.Models.Filtering;
using System;
using System.Collections.Generic;

namespace Fluorite.Strainer.Services.Filtering
{
    public interface ICustomFilterMethodMapper
    {
        IDictionary<Type, IDictionary<string, ICustomFilterMethod>> Methods { get; }

        void AddMap<TEntity>(ICustomFilterMethod<TEntity> sortMethod);

        ICustomFilterMethodBuilder<TEntity> CustomMethod<TEntity>(string name);
    }
}
