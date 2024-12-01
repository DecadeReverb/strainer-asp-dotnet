using Fluorite.Strainer.Models.Sorting.Terms;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Models.Sorting;

/// <summary>
/// Represents custom sort method.
/// </summary>
/// <typeparam name="T">
/// The type of entity processed by the custom method.
/// </typeparam>
public interface ICustomSortMethod<T> : ICustomSortMethod
{
    /// <summary>
    /// Gets the expression used for custom sorting.
    /// </summary>
    Expression<Func<T, object>>? Expression { get; }

    /// <summary>
    /// Gets the provider of expression used for custom sorting.
    /// </summary>
    Func<ISortTerm, Expression<Func<T, object>>>? ExpressionProvider { get; }
}
