using Fluorite.Strainer.Models.Sorting;
using Fluorite.Strainer.Models.Sorting.Terms;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Sorting;

public class CustomSortMethodBuilder<TEntity> : ICustomSortMethodBuilder<TEntity>
{
    public CustomSortMethodBuilder()
    {
    }

    protected Expression<Func<TEntity, object>>? Expression { get; set; }

    protected Func<ISortTerm, Expression<Func<TEntity, object>>>? ExpressionProvider { get; set; }

    protected string? Name { get; set; }

    public ICustomSortMethod<TEntity> Build()
    {
        Guard.Against.NullOrWhiteSpace(Name);

        if (ExpressionProvider is null)
        {
            Guard.Against.Null(Expression);

            return new CustomSortMethod<TEntity>(Name, Expression);
        }
        else
        {
            Guard.Against.Null(ExpressionProvider);

            return new CustomSortMethod<TEntity>(Name, ExpressionProvider);
        }
    }

    public ICustomSortMethodBuilder<TEntity> HasFunction(
        Expression<Func<TEntity, object>> expression)
    {
        Expression = Guard.Against.Null(expression);
        ExpressionProvider = null;

        return this;
    }

    public ICustomSortMethodBuilder<TEntity> HasFunction(Func<ISortTerm, Expression<Func<TEntity, object>>> expressionProvider)
    {
        ExpressionProvider = Guard.Against.Null(expressionProvider);
        Expression = null;

        return this;
    }

    public ICustomSortMethodBuilder<TEntity> HasName(string name)
    {
        Name = Guard.Against.NullOrWhiteSpace(name);

        return this;
    }
}