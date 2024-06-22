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

    protected Func<ISortTerm, Expression<Func<TEntity, object>>> SortTermExpression { get; set; }

    protected string Name { get; set; }

    public ICustomSortMethod<TEntity> Build() => new CustomSortMethod<TEntity>
    {
        Expression = Expression,
        Name = Name,
        SortTermExpression = SortTermExpression,
    };

    public ICustomSortMethodBuilder<TEntity> HasFunction(
        Expression<Func<TEntity, object>> expression)
    {
        Expression = Guard.Against.Null(expression);
        SortTermExpression = null;

        Save(Build());

        return this;
    }

    public ICustomSortMethodBuilder<TEntity> HasFunction(Func<ISortTerm, Expression<Func<TEntity, object>>> sortTermExpression)
    {
        SortTermExpression = Guard.Against.Null(sortTermExpression);
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