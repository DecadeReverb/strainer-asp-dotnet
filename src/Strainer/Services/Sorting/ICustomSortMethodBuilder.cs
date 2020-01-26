using Fluorite.Strainer.Models.Sorting;
using System;
using System.Linq;

namespace Fluorite.Strainer.Services.Sorting
{
    public interface ICustomSortMethodBuilder<TEntity>
    {
        ICustomSortMethod<TEntity> Build();

        ICustomSortMethodBuilder<TEntity> HasFunction(
            Func<IQueryable<TEntity>, bool, bool, IQueryable<TEntity>> function);
    }
}
