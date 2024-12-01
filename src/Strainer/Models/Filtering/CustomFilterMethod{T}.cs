using Fluorite.Strainer.Models.Filtering.Terms;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Models.Filtering;

/// <summary>
/// Represents custom filter method.
/// </summary>
/// <typeparam name="T">
/// The type of entity processed by the custom method.
/// </typeparam>
public class CustomFilterMethod<T> : CustomFilterMethod, ICustomFilterMethod<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomFilterMethod{TEntity}"/>
    /// class.
    /// </summary>
    /// <param name="name">
    /// The custom method name.
    /// </param>
    /// <param name="expression">
    /// The expression used for custom filtering.
    /// </param>
    public CustomFilterMethod(string name, Expression<Func<T, bool>> expression) : base(name)
    {
        Expression = Guard.Against.Null(expression);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomFilterMethod{TEntity}"/>
    /// class.
    /// </summary>
    /// <param name="name">
    /// The custom method name.
    /// </param>
    /// <param name="filterTermExpression">
    /// The expression used for custom filtering.
    /// </param>
    public CustomFilterMethod(string name, Func<IFilterTerm, Expression<Func<T, bool>>> filterTermExpression) : base(name)
    {
        FilterTermExpression = Guard.Against.Null(filterTermExpression);
    }

    /// <summary>
    /// Gets the expression used for custom filtering.
    /// </summary>
    public Expression<Func<T, bool>>? Expression { get; }

    /// <summary>
    /// Gets the func leading to expression used for custom filtering.
    /// </summary>
    public Func<IFilterTerm, Expression<Func<T, bool>>>? FilterTermExpression { get; }
}
