using System.Linq;

namespace Fluorite.Strainer.Models.Sorting
{
    public interface ICustomSortMethodContext<TEntity>
    {
        bool IsDescending { get; }
        bool IsSubsequent { get; }
        IQueryable<TEntity> Source { get; }
    }
}
