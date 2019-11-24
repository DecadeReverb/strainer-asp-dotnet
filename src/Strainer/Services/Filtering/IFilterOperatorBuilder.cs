using Fluorite.Strainer.Models.Filter.Operators;
using System;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Filtering
{
    public interface IFilterOperatorBuilder
    {
        IFilterOperator Build();

        IFilterOperatorBuilder HasExpression(Func<IFilterExpressionContext, Expression> expression);

        IFilterOperatorBuilder HasName(string name);

        IFilterOperatorBuilder IsCaseInsensitive();

        IFilterOperatorBuilder IsDefault();

        IFilterOperatorBuilder IsStringBased();
    }
}
