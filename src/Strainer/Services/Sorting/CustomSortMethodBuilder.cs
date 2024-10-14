using Fluorite.Strainer.Models.Sorting;
using Fluorite.Strainer.Models.Sorting.Terms;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Sorting;

public class CustomSortMethodBuilder<TEntity> : ICustomSortMethodBuilder<TEntity>
{
    private readonly IDictionary<Type, IDictionary<string, ICustomSortMethod>> _customMethods;

    public CustomSortMethodBuilder(
        IDictionary<Type, IDictionary<string, ICustomSortMethod>> customSortMethodsDictionary,
        string name)
    {
        _customMethods = Guard.Against.Null(customSortMethodsDictionary);
        Name = Guard.Against.NullOrWhiteSpace(name);
    }

    protected Expression<Func<TEntity, object>> Expression { get; set; }

    protected Func<ISortTerm, Expression<Func<TEntity, object>>> ExpressionProvider { get; set; }

    protected string Name { get; set; }

    public ICustomSortMethod<TEntity> Build() => new CustomSortMethod<TEntity>
    {
        Expression = Expression,
        Name = Name,
        ExpressionProvider = ExpressionProvider,
    };

    public ICustomSortMethodBuilder<TEntity> HasFunction(
        Expression<Func<TEntity, object>> expression)
    {
        Expression = Guard.Against.Null(expression);
        ExpressionProvider = null;

        Save(Build());

        return this;
    }

    public ICustomSortMethodBuilder<TEntity> HasFunction(Func<ISortTerm, Expression<Func<TEntity, object>>> expressionProvider)
    {
        ExpressionProvider = Guard.Against.Null(expressionProvider);
        Expression = null;

        Save(Build());

        return this;
    }

    protected void Save(ICustomSortMethod<TEntity> customSortMethod)
    {
        Guard.Against.Null(customSortMethod);

        if (!_customMethods.ContainsKey(typeof(TEntity)))
        {
            _customMethods[typeof(TEntity)] = new Dictionary<string, ICustomSortMethod>();
        }

        _customMethods[typeof(TEntity)][customSortMethod.Name] = customSortMethod;
    }
}