using Fluorite.Strainer.Models.Sorting;

namespace Fluorite.Strainer.Services.Sorting;

public class CustomSortMethodMapper : ICustomSortMethodMapper
{
    public CustomSortMethodMapper()
    {
        Methods = new Dictionary<Type, IDictionary<string, ICustomSortMethod>>();
    }

    public IDictionary<Type, IDictionary<string, ICustomSortMethod>> Methods { get; }

    public void AddMap<TEntity>(ICustomSortMethod<TEntity> sortMethod)
    {
        if (sortMethod == null)
        {
            throw new ArgumentNullException(nameof(sortMethod));
        }

        if (!Methods.ContainsKey(typeof(TEntity)))
        {
            Methods[typeof(TEntity)] = new Dictionary<string, ICustomSortMethod>();
        }

        Methods[typeof(TEntity)][sortMethod.Name] = sortMethod;
    }

    public ICustomSortMethodBuilder<TEntity> CustomMethod<TEntity>(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(
                $"{nameof(name)} cannot be null, empty " +
                $"or contain only whitespace characters.",
                nameof(name));
        }

        if (!Methods.ContainsKey(typeof(TEntity)))
        {
            Methods[typeof(TEntity)] = new Dictionary<string, ICustomSortMethod>();
        }

        return new CustomSortMethodBuilder<TEntity>(Methods, name);
    }
}