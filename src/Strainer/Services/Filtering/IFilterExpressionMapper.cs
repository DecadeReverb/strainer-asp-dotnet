using Fluorite.Strainer.Models.Filtering.Operators;
using System;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Filtering
{
    public interface IFilterExpressionMapper
    {
        void Add(Type filterOperatorType, Func<IFilterExpressionContext, Expression> expression);
        Expression GetDefaultExpression(Expression filterValue, Expression propertyValue);
        Expression GetExpression(IFilterOperator filterOperator, Expression filterValue, Expression propertyValue);
        IFilterExpressionBuilder<TOperator> Operator<TOperator>() where TOperator : IFilterOperator;
    }
}
