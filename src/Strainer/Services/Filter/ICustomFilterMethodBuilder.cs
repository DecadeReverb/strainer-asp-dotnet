using Fluorite.Strainer.Models.Filtering;
using System;
using System.Linq;

namespace Fluorite.Strainer.Services.Filter
{
    public interface ICustomFilterMethodBuilder<TEntity>
    {
        Func<ICustomFilterMethodContext<TEntity>, IQueryable<TEntity>> Function { get; }
        string Name { get; }

        ICustomFilterMethod<TEntity> Build();
        ICustomFilterMethodBuilder<TEntity> WithFunction(Func<ICustomFilterMethodContext<TEntity>, IQueryable<TEntity>> function);
    }
}
