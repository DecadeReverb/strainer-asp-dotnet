using System;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Models.Filtering.Operators
{
    /// <summary>
    /// Represents default information context for filter expression.
    /// </summary>
    public class FilterExpressionContext : IFilterExpressionContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterExpressionContext"/>
        /// class.
        /// </summary>
        /// <param name="filterValue">
        /// The filter value expression.
        /// </param>
        /// <param name="propertyValue">
        /// The property access expression.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="filterValue"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="propertyValue"/> is <see langword="null"/>.
        /// </exception>
        public FilterExpressionContext(Expression filterValue, Expression propertyValue)
        {
            FilterValue = filterValue ?? throw new ArgumentNullException(nameof(filterValue));
            PropertyValue = propertyValue ?? throw new ArgumentNullException(nameof(propertyValue));
        }

        /// <summary>
        /// Gets the expression for filter value being processed.
        /// </summary>
        public Expression FilterValue { get; }

        /// <summary>
        /// Gets the expression for property value access.
        /// </summary>
        public Expression PropertyValue { get; }
    }
}
