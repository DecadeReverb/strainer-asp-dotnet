using Fluorite.Strainer.Models.Sorting;

namespace Fluorite.Strainer.Services.Sorting
{
    public interface ICustomSortMethodMapper
    {
        IDictionary<Type, IDictionary<string, ICustomSortMethod>> Methods { get; }

        void AddMap<TEntity>(ICustomSortMethod<TEntity> sortMethod);

        ICustomSortMethodBuilder<TEntity> CustomMethod<TEntity>(string name);
    }
}
