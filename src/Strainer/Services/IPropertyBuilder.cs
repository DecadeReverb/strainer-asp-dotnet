using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services.Sorting;

namespace Fluorite.Strainer.Services
{
    public interface IPropertyBuilder<TEntity>
    {
        IPropertyMetadata Build();
        IPropertyBuilder<TEntity> CanFilter();
        ISortPropertyBuilder<TEntity> CanSort();
        IPropertyBuilder<TEntity> HasDisplayName(string displayName);
    }
}
