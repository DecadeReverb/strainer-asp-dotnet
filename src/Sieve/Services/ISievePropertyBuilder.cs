namespace Sieve.Services
{
    public interface ISievePropertyBuilder<TEntity>
    {
        string FullName { get; }
        bool IsFilterable { get; }
        bool IsSortable { get; }
        string Name { get; }

        ISievePropertyBuilder<TEntity> CanFilter();
        ISievePropertyBuilder<TEntity> HasName(string name);
        ISievePropertyBuilder<TEntity> CanSort();
    }
}
