namespace Fluorite.Strainer.Services.Sorting
{
    public interface ISortPropertyBuilder<TEntity> : IPropertyBuilder<TEntity>
    {
        ISortPropertyBuilder<TEntity> IsDefaultSort(bool isAscending = true);
    }
}
