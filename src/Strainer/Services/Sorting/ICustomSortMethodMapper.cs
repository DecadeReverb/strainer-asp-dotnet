using Fluorite.Strainer.Models.Sorting;

namespace Fluorite.Strainer.Services.Sorting
{
    public interface ICustomSortMethodMapper
    {
        void AddMethod<TEntity>(ICustomSortMethod<TEntity> sortMethod);
        ICustomSortMethod<TEntity> GetMethod<TEntity>(string name);
        ICustomSortMethodBuilder<TEntity> CustomMethod<TEntity>(string name);
    }
}
