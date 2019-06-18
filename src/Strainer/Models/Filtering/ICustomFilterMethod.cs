using System;
using System.Linq;

namespace Fluorite.Strainer.Models.Filtering
{
    public interface ICustomFilterMethod<TEntity>
    {
        Func<ICustomFilterMethodContext<TEntity>, IQueryable<TEntity>> Function { get; }
        string Name { get; }
    }
}
