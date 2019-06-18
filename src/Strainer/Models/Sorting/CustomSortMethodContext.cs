using System.Linq;

namespace Fluorite.Strainer.Models.Sorting
{
    public class CustomSortMethodContext<TEntity> : ICustomSortMethodContext<TEntity>
    {
        public CustomSortMethodContext()
        {

        }

        public bool IsDescending { get; set; }

        public bool IsSubsequent { get; set; }

        public IQueryable<TEntity> Source { get; set; }
    }
}
