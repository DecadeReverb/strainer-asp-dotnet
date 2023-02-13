using Fluorite.Strainer.Models.Sorting;

namespace Fluorite.Strainer.Services.Sorting
{
    public interface ICustomSortMethodBuilder<TEntity>
    {
        ICustomSortMethod<TEntity> Build();

        ICustomSortMethodBuilder<TEntity> HasFunction(
            Func<IQueryable<TEntity>, bool, bool, IQueryable<TEntity>> function);
    }
}
