using Fluorite.Strainer.Models.Sorting;

namespace Fluorite.Strainer.Services.Sorting;

// TODO: What for is this mapper? It is not used anywhere.
public class CustomSortMethodMapper : ICustomSortMethodMapper
{
    public CustomSortMethodMapper()
    {
        Methods = new Dictionary<Type, IDictionary<string, ICustomSortMethod>>();
    }

    public IDictionary<Type, IDictionary<string, ICustomSortMethod>> Methods { get; }

    public void AddMap<TEntity>(ICustomSortMethod<TEntity> sortMethod)
    {
        Guard.Against.Null(sortMethod);

        if (!Methods.ContainsKey(typeof(TEntity)))
        {
            Methods[typeof(TEntity)] = new Dictionary<string, ICustomSortMethod>();
        }

        Methods[typeof(TEntity)][sortMethod.Name] = sortMethod;
    }

    public void CustomMethod<TEntity>(Func<ICustomSortMethodBuilder<TEntity>, ICustomSortMethod<TEntity>> buildingDelegate)
    {
        Guard.Against.Null(buildingDelegate);

        var builder = new CustomSortMethodBuilder<TEntity>();
        var customMethod = buildingDelegate.Invoke(builder);

        if (!Methods.ContainsKey(typeof(TEntity)))
        {
            Methods[typeof(TEntity)] = new Dictionary<string, ICustomSortMethod>();
        }

        Methods[typeof(TEntity)][customMethod.Name] = customMethod;
    }
}