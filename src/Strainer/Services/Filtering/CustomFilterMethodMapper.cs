using Fluorite.Strainer.Models.Filtering;

namespace Fluorite.Strainer.Services.Filtering;

// TODO: What for is this mapper? It is not used anywhere.
public class CustomFilterMethodMapper : ICustomFilterMethodMapper
{
    public CustomFilterMethodMapper()
    {
        Methods = new Dictionary<Type, IDictionary<string, ICustomFilterMethod>>();
    }

    public IDictionary<Type, IDictionary<string, ICustomFilterMethod>> Methods { get; }

    public void AddMap<TEntity>(ICustomFilterMethod<TEntity> customMethod)
    {
        Guard.Against.Null(customMethod);

        if (!Methods.ContainsKey(typeof(TEntity)))
        {
            Methods[typeof(TEntity)] = new Dictionary<string, ICustomFilterMethod>();
        }

        Methods[typeof(TEntity)][customMethod.Name] = customMethod;
    }

    public void CustomMethod<TEntity>(Func<ICustomFilterMethodBuilder<TEntity>, ICustomFilterMethod<TEntity>> buildingDelegate)
    {
        Guard.Against.Null(buildingDelegate);

        var builder = new CustomFilterMethodBuilder<TEntity>();
        var customMethod = buildingDelegate.Invoke(builder);

        if (!Methods.ContainsKey(typeof(TEntity)))
        {
            Methods[typeof(TEntity)] = new Dictionary<string, ICustomFilterMethod>();
        }

        Methods[typeof(TEntity)][customMethod.Name] = customMethod;
    }
}