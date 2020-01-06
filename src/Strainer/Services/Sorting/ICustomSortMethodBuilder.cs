using Fluorite.Strainer.Models.Sorting;
using System;
using System.Linq;

namespace Fluorite.Strainer.Services.Sorting
{
    public interface ICustomSortMethodBuilder<TEntity>
    {
        ICustomSortMethod<TEntity> Build();

        ICustomSortMethodBuilder<TEntity> HasExpression(
            Func<ICustomSortMethodContext<TEntity>, IQueryable<TEntity>> function);
    }
}
