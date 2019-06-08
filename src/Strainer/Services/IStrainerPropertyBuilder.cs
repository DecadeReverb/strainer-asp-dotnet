namespace Fluorite.Strainer.Services
{
    public interface IStrainerPropertyBuilder<TEntity>
    {
        string DisplayName { get; }
        bool IsFilterable { get; }
        bool IsSortable { get; }
        string Name { get; }

        IStrainerPropertyBuilder<TEntity> CanFilter();
        IStrainerPropertyBuilder<TEntity> CanSort();
        IStrainerPropertyBuilder<TEntity> HasDisplayName(string displayName);
    }
}
