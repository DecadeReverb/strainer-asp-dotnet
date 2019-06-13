using Fluorite.Strainer.Models.Filtering.Operators;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Filtering
{
    public interface IFilterExpressionMapper
    {
        Expression GetDefaultExpression(Expression filterValue, Expression propertyValue);
        Expression GetExpression(IFilterOperator filterOperator, Expression filterValue, Expression propertyValue);
    }
}
