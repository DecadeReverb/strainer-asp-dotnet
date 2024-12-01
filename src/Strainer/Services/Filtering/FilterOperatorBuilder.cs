using Fluorite.Strainer.Models.Filtering.Operators;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Filtering;

public class FilterOperatorBuilder : IFilterOperatorBuilder
{
    public FilterOperatorBuilder()
    {
    }

    public FilterOperatorBuilder(string symbol)
    {
        Symbol = Guard.Against.NullOrWhiteSpace(symbol);
    }

    protected Func<IFilterExpressionContext, Expression>? Expression { get; set; }

    protected bool IsCaseInsensitive1 { get; set; }

    protected bool IsStringBased1 { get; set; }

    protected string? Name { get; set; }

    protected string? Symbol { get; set; }

    public IFilterOperator Build()
    {
        Guard.Against.NullOrWhiteSpace(Name, message: "Missing or invalid filter operator name.");
        Guard.Against.NullOrWhiteSpace(Symbol, message: "Missing or invalid filter operator symbol.");
        Guard.Against.Null(Expression, message: "Missing filter operator expression.");

        return new FilterOperator(Name, Symbol, Expression)
        {
            IsCaseInsensitive = IsCaseInsensitive1,
            IsStringBased = IsStringBased1,
        };
    }

    public IFilterOperatorBuilder HasExpression(Func<IFilterExpressionContext, Expression> expression)
    {
        Expression = Guard.Against.Null(expression);

        return this;
    }

    public IFilterOperatorBuilder HasName(string name)
    {
        Name = Guard.Against.NullOrWhiteSpace(name);

        return this;
    }

    public IFilterOperatorBuilder HasSymbol(string symbol)
    {
        Symbol = Guard.Against.NullOrWhiteSpace(symbol);

        return this;
    }

    public IFilterOperatorBuilder IsCaseInsensitive()
    {
        IsCaseInsensitive1 = true;

        return this;
    }

    public IFilterOperatorBuilder IsStringBased()
    {
        IsStringBased1 = true;

        return this;
    }
}
