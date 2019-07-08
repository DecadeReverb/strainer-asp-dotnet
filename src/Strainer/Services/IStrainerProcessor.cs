using System.Linq;
using Fluorite.Strainer.Models;

namespace Fluorite.Strainer.Services
{
    public interface IStrainerProcessor
    {
        IQueryable<TEntity> Apply<TEntity>(
            IStrainerModel model,
            IQueryable<TEntity> source,
            bool applyFiltering = true,
            bool applySorting = true,
            bool applyPagination = true);

        IQueryable<TEntity> ApplyFiltering<TEntity>(IStrainerModel model, IQueryable<TEntity> source);

        IQueryable<TEntity> ApplyPagination<TEntity>(IStrainerModel model, IQueryable<TEntity> source);

        IQueryable<TEntity> ApplySorting<TEntity>(IStrainerModel model, IQueryable<TEntity> source);
    }
}
