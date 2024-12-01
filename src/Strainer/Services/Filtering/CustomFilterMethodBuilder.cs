using Fluorite.Strainer.Models.Filtering;
using Fluorite.Strainer.Models.Filtering.Terms;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Filtering;

public class CustomFilterMethodBuilder<TEntity> : ICustomFilterMethodBuilder<TEntity>
{
    public CustomFilterMethodBuilder()
    {
    }

    public CustomFilterMethodBuilder(string name)
    {
        Name = Guard.Against.NullOrWhiteSpace(name);
    }

    protected Expression<Func<TEntity, bool>>? Expression { get; set; }

    protected Func<IFilterTerm, Expression<Func<TEntity, bool>>>? FilterTermExpression { get; set; }

    protected string? Name { get; set; }

    public ICustomFilterMethod<TEntity> Build()
    {
        Guard.Against.NullOrWhiteSpace(Name);

        if (FilterTermExpression is null)
        {
            Guard.Against.Null(Expression);

            return new CustomFilterMethod<TEntity>(Name, Expression);
        }
        else
        {
            Guard.Against.Null(FilterTermExpression);

            return new CustomFilterMethod<TEntity>(Name, FilterTermExpression);
        }
    }

    public ICustomFilterMethodBuilder<TEntity> HasFunction(Expression<Func<TEntity, bool>> expression)
    {
        Expression = Guard.Against.Null(expression);

        return this;
    }

    public ICustomFilterMethodBuilder<TEntity> HasFunction(
        Func<IFilterTerm, Expression<Func<TEntity, bool>>> filterTermExpression)
    {
        FilterTermExpression = Guard.Against.Null(filterTermExpression);

        return this;
    }

    public ICustomFilterMethodBuilder<TEntity> HasName(string name)
    {
        Name = Guard.Against.NullOrWhiteSpace(name);

        return this;
    }
}
