using System.Linq.Expressions;

namespace Fluorite.Strainer.Models.Filter.Operators
{
    public interface IFilterExpressionContext
    {
        Expression FilterValue { get; }
        Expression PropertyValue { get; }
    }
}
