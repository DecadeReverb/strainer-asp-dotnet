using System;
using System.Linq;

namespace Fluorite.Strainer.Models.Filtering
{
    public class CustomFilterMethod<TEntity> : ICustomFilterMethod<TEntity>
    {
        public CustomFilterMethod()
        {

        }

        public Func<ICustomFilterMethodContext<TEntity>, IQueryable<TEntity>> Function { get; set; }

        public string Name { get; set; }
    }
}
