using System.Linq;
using Fluorite.Strainer.Models;

namespace Fluorite.Strainer.Services
{
    public interface IStrainerProcessor
    {
        IQueryable<TEntity> Apply<TEntity>(
            IStrainerModel model,
            IQueryable<TEntity> source,
            object[] dataForCustomMethods = null,
            bool applyFiltering = true,
            bool applySorting = true,
            bool applyPagination = true);
    }
}
