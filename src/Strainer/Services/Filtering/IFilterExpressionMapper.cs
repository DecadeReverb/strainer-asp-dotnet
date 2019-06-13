using Fluorite.Strainer.Models.Filtering.Operators;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Filtering
{
    public interface IFilterExpressionMapper
    {
        Expression GetDefaultExpression(dynamic filterValue, dynamic propertyValue);
        Expression GetExpression(IFilterOperator filterOperator, dynamic filterValue, dynamic propertyValue);
    }
}
