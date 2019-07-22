using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services.Sorting;

namespace Fluorite.Strainer.Services
{
    public interface IStrainerPropertyBuilder<TEntity>
    {
        IStrainerPropertyMetadata Build();
        IStrainerPropertyBuilder<TEntity> CanFilter();
        ISortPropertyBuilder<TEntity> CanSort();
        IStrainerPropertyBuilder<TEntity> HasDisplayName(string displayName);
    }
}
