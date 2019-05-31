using System.Linq;
using Sieve.Models;

namespace Sieve.Services
{
    public interface ISieveProcessor<TSieveModel>
        where TSieveModel : class, ISieveModel
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
