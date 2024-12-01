using Fluorite.Strainer.Models.Sorting.Terms;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Models.Sorting;

/// <summary>
/// Represents custom sort method.
/// </summary>
/// <typeparam name="T">
/// The type of entity processed by the custom method.
/// </typeparam>
public class CustomSortMethod<T> : CustomSortMethod, ICustomSortMethod<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomSortMethod{T}"/> class.
    /// </summary>
    /// <param name="name">
    /// The custom sort method name.
    /// </param>
    /// <param name="expression">
    /// The expression used for custom sorting.
    /// </param>
    public CustomSortMethod(string name, Expression<Func<T, object>> expression) : base(name)
    {
        Expression = Guard.Against.Null(expression);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomSortMethod{T}"/> class.
    /// </summary>
    /// <param name="name">
    /// The custom sort method name.
    /// </param>
    /// <param name="expressionProvider">
    /// The expression used for custom sorting.
    /// </param>
    public CustomSortMethod(string name, Func<ISortTerm, Expression<Func<T, object>>> expressionProvider) : base(name)
    {
        ExpressionProvider = Guard.Against.Null(expressionProvider);
    }

    /// <summary>
    /// Gets the expression used for custom sorting.
    /// </summary>
    public Expression<Func<T, object>>? Expression { get; }

    /// <summary>
    /// Gets the provider of expression used for custom sorting.
    /// </summary>
    public Func<ISortTerm, Expression<Func<T, object>>>? ExpressionProvider { get; }
}