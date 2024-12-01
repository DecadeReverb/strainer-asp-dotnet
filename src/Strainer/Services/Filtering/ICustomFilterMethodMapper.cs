using Fluorite.Strainer.Models.Filtering;

namespace Fluorite.Strainer.Services.Filtering;

public interface ICustomFilterMethodMapper
{
    IDictionary<Type, IDictionary<string, ICustomFilterMethod>> Methods { get; }

    void AddMap<TEntity>(ICustomFilterMethod<TEntity> sortMethod);

    void CustomMethod<TEntity>(Func<ICustomFilterMethodBuilder<TEntity>, ICustomFilterMethod<TEntity>> buildingDelegate);
}
