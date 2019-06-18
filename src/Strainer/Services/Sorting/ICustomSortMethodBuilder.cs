using Fluorite.Strainer.Models.Sorting;
using System;
using System.Linq;

namespace Fluorite.Strainer.Services.Sorting
{
    public interface ICustomSortMethodBuilder<TEntity>
    {
        Func<ICustomSortMethodContext<TEntity>, IQueryable<TEntity>> Function { get; }
        string Name { get; }

        ICustomSortMethod<TEntity> Build();
        ICustomSortMethodBuilder<TEntity> WithFunction(Func<ICustomSortMethodContext<TEntity>, IQueryable<TEntity>> function);
    }
}
