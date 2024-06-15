using Fluorite.Strainer.Models.Filtering;
using Fluorite.Strainer.Models.Filtering.Terms;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Filtering;

public class CustomFilterMethodBuilder<TEntity> : ICustomFilterMethodBuilder<TEntity>
{
    private readonly IDictionary<Type, IDictionary<string, ICustomFilterMethod>> _customMethods;

    public CustomFilterMethodBuilder(
        IDictionary<Type, IDictionary<string, ICustomFilterMethod>> customFilterMethodsDictionary,
        string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(
                $"{nameof(name)} cannot be null, empty " +
                $"or contain only whitespace characters.",
                nameof(name));
        }

        _customMethods = customFilterMethodsDictionary
            ?? throw new ArgumentNullException(nameof(customFilterMethodsDictionary));
        Name = name;
    }

    protected Expression<Func<TEntity, bool>> Expression { get; set; }

    protected Func<IFilterTerm, Expression<Func<TEntity, bool>>> FilterTermExpression { get; set; }

    protected string Name { get; set; }

    public ICustomFilterMethod<TEntity> Build() => new CustomFilterMethod<TEntity>
    {
        Expression = Expression,
        FilterTermExpression = FilterTermExpression,
        Name = Name,
    };

    public ICustomFilterMethodBuilder<TEntity> HasFunction(
        Expression<Func<TEntity, bool>> expression)
    {
        Expression = expression ?? throw new ArgumentNullException(nameof(expression));

        Save(Build());

        return this;
    }

    public ICustomFilterMethodBuilder<TEntity> HasFunction(
        Func<IFilterTerm, Expression<Func<TEntity, bool>>> filterTermExpression)
    {
        FilterTermExpression = filterTermExpression ?? throw new ArgumentNullException(nameof(filterTermExpression));

        Save(Build());

        return this;
    }

    protected void Save(ICustomFilterMethod<TEntity> customFilterMethod)
    {
        if (customFilterMethod == null)
        {
            throw new ArgumentNullException(nameof(customFilterMethod));
        }

        if (!_customMethods.ContainsKey(typeof(TEntity)))
        {
            _customMethods[typeof(TEntity)] = new Dictionary<string, ICustomFilterMethod>();
        }

        _customMethods[typeof(TEntity)][customFilterMethod.Name] = customFilterMethod;
    }
}
