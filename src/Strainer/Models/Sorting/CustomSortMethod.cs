using System;
using System.Linq;

namespace Fluorite.Strainer.Models.Sorting
{
    public class CustomSortMethod<TEntity> : ICustomSortMethod<TEntity>
    {
        public CustomSortMethod()
        {

        }

        public Func<ICustomSortMethodContext<TEntity>, IQueryable<TEntity>> Function { get; set; }

        public string Name { get; set; }
    }
}