using System.Linq;
using Sieve.Models;

namespace Sieve.Services
{
    public interface ISieveProcessor<TSieveModel, TSortTerm>
        where TSieveModel : class, ISieveModel<TSortTerm>
        where TSortTerm : ISortTerm, new()
    {
        IQueryable<TEntity> Apply<TEntity>(
            TSieveModel model,
            IQueryable<TEntity> source,
            object[] dataForCustomMethods = null,
            bool applyFiltering = true,
            bool applySorting = true,
            bool applyPagination = true);
    }
}
