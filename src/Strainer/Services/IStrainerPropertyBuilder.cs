using Fluorite.Strainer.Models;

namespace Fluorite.Strainer.Services
{
    public interface IStrainerPropertyBuilder<TEntity>
    {
        IStrainerPropertyMetadata Build();
        IStrainerPropertyBuilder<TEntity> CanFilter();
        IStrainerPropertyBuilder<TEntity> CanSort();
        IStrainerPropertyBuilder<TEntity> HasDisplayName(string displayName);
    }
}
