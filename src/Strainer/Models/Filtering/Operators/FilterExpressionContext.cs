using System.Linq.Expressions;

namespace Fluorite.Strainer.Models.Filtering.Operators;

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
        FilterValue = Guard.Against.Null(filterValue);
        PropertyValue = Guard.Against.Null(propertyValue);
    }

    /// <inheritdoc/>
    public Expression FilterValue { get; }

    /// <inheritdoc/>
    public Expression PropertyValue { get; }
}
