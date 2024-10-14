using Fluorite.Strainer.Models.Filtering.Operators;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Filtering;

public class FilterOperatorBuilder : IFilterOperatorBuilder
{
    private readonly IDictionary<string, IFilterOperator> _filterOperators;

    public FilterOperatorBuilder(string symbol)
    {
        _filterOperators = new Dictionary<string, IFilterOperator>();
        Symbol = Guard.Against.NullOrWhiteSpace(symbol);

        Save(Build()); // Is this really needed?
    }

    public FilterOperatorBuilder(
        IDictionary<string, IFilterOperator> filterOperators,
        string symbol)
    {
        _filterOperators = Guard.Against.Null(filterOperators);
        Symbol = Guard.Against.NullOrWhiteSpace(symbol);

        Save(Build()); // Is this really needed?
    }

    protected Func<IFilterExpressionContext, Expression> Expression { get; set; }

    protected bool IsCaseInsensitive1 { get; set; }

    protected bool IsStringBased1 { get; set; }

    protected string Name { get; set; }

    protected string Symbol { get; set; }

    public IFilterOperator Build() => new FilterOperator
    {
        Expression = Expression,
        IsCaseInsensitive = IsCaseInsensitive1,
        IsStringBased = IsStringBased1,
        Name = Name,
        Symbol = Symbol,
    };

    public IFilterOperatorBuilder HasExpression(Func<IFilterExpressionContext, Expression> expression)
    {
        Expression = Guard.Against.Null(expression);
        Save(Build());

        return this;
    }

    public IFilterOperatorBuilder HasName(string name)
    {
        Name = Guard.Against.NullOrWhiteSpace(name);
        Save(Build());

        return this;
    }

    public IFilterOperatorBuilder IsCaseInsensitive()
    {
        IsCaseInsensitive1 = true;
        Save(Build());

        return this;
    }

    public IFilterOperatorBuilder IsStringBased()
    {
        IsStringBased1 = true;
        Save(Build());

        return this;
    }

    protected void Save(IFilterOperator filterOperator)
    {
        Guard.Against.Null(filterOperator);

        _filterOperators[Symbol] = filterOperator;
    }
}
