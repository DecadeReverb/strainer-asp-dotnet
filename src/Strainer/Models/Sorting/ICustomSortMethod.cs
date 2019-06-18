using Fluorite.Strainer.Models.Sorting;
using System;
using System.Linq;

namespace Fluorite.Strainer.Models.Sorting
{
    public interface ICustomSortMethod<TEntity>
    {
        Func<ICustomSortMethodContext<TEntity>, IQueryable<TEntity>> Function { get; }
        string Name { get; }
    }
}
