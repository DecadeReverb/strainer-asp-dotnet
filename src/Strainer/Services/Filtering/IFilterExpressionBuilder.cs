using Fluorite.Strainer.Models.Filtering.Operators;
using System;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Filtering
{
    public interface IFilterExpressionBuilder<TOperator>
        where TOperator : IFilterOperator
    {
        IFilterExpressionBuilder<TOperator> HasExpression(Func<IFilterExpressionContext, Expression> expression);
    }
}