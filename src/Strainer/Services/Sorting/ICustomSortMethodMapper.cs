using Fluorite.Strainer.Models.Sorting;

namespace Fluorite.Strainer.Services.Sorting
{
    public interface ICustomSortMethodMapper
    {
        void AddMap<TEntity>(ICustomSortMethod<TEntity> sortMethod);
        ICustomSortMethodBuilder<TEntity> CustomMethod<TEntity>(string name);
        ICustomSortMethod<TEntity> GetMethod<TEntity>(string name);
    }
}
