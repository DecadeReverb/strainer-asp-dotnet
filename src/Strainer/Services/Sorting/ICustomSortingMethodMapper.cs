using Fluorite.Strainer.Models.Sorting;

namespace Fluorite.Strainer.Services.Sorting
{
    public interface ICustomSortingMethodMapper
    {
        void AddMap<TEntity>(ICustomSortMethod<TEntity> sortMethod);
        ICustomSortMethod<TEntity> GetMethod<TEntity>(string name);
        ICustomSortingMethodBuilder<TEntity> CustomMethod<TEntity>(string name);
    }
}
