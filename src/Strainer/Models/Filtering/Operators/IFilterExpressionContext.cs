using System.Linq.Expressions;

namespace Fluorite.Strainer.Models.Filtering.Operators
{
    public interface IFilterExpressionContext
    {
        Expression FilterValue { get; }
        Expression PropertyValue { get; }
    }
}
