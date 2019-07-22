using System;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Models.Filter.Operators
{
    public class FilterExpressionContext : IFilterExpressionContext
    {
        public FilterExpressionContext(Expression filterValue, Expression propertyValue)
        {
            FilterValue = filterValue ?? throw new ArgumentNullException(nameof(filterValue));
            PropertyValue = propertyValue ?? throw new ArgumentNullException(nameof(propertyValue));
        }

        public Expression FilterValue { get; }

        public Expression PropertyValue { get; }
    }
}
