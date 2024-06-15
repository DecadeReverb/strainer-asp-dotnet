using Fluorite.Strainer.Models.Filtering;

namespace Fluorite.Strainer.Services.Filtering;

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

    public ICustomFilterMethodBuilder<TEntity> CustomMethod<TEntity>(string name)
    {
        Guard.Against.NullOrWhiteSpace(name);

        if (!Methods.ContainsKey(typeof(TEntity)))
        {
            Methods[typeof(TEntity)] = new Dictionary<string, ICustomFilterMethod>();
        }

        return new CustomFilterMethodBuilder<TEntity>(Methods, name);
    }
}