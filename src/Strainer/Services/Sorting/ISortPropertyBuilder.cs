namespace Fluorite.Strainer.Services.Sorting
{
    public interface ISortPropertyBuilder<TEntity> : IStrainerPropertyBuilder<TEntity>
    {
        ISortPropertyBuilder<TEntity> IsDefaultSort(bool isAscending = true);
    }
}
