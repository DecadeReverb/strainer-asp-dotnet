namespace Sieve.Services
{
    public interface IPropertyFluentApi<TEntity>
    {
        string FullName { get; }
        bool IsFilterable { get; }
        bool IsSortable { get; }
        string Name { get; }

        IPropertyFluentApi<TEntity> CanFilter();
        IPropertyFluentApi<TEntity> HasName(string name);
        IPropertyFluentApi<TEntity> CanSort();
    }
}
