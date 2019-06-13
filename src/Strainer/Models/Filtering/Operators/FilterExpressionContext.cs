using System.Linq.Expressions;

namespace Fluorite.Strainer.Models.Filtering.Operators
{
    public class FilterExpressionContext : IFilterExpressionContext
    {
        public FilterExpressionContext(Expression filterValue, Expression propertyValue)
        {
            FilterValue = filterValue ?? throw new System.ArgumentNullException(nameof(filterValue));
            PropertyValue = propertyValue ?? throw new System.ArgumentNullException(nameof(propertyValue));
        }

        public Expression FilterValue { get; }

        public Expression PropertyValue { get; }
    }
}
