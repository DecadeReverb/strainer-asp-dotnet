namespace Fluorite.Strainer.Services
{
    public interface IStrainerPropertyBuilder<TEntity>
    {
        string FullName { get; }
        bool IsFilterable { get; }
        bool IsSortable { get; }
        string Name { get; }

        IStrainerPropertyBuilder<TEntity> CanFilter();
        IStrainerPropertyBuilder<TEntity> HasName(string name);
        IStrainerPropertyBuilder<TEntity> CanSort();
    }
}
