using Fluorite.Strainer.Models.Filtering.Terms;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Models.Filtering
{
    /// <summary>
    /// Represents custom filter method.
    /// </summary>
    /// <typeparam name="T">
    /// The type of entity processed by the custom method.
    /// </typeparam>
    public interface ICustomFilterMethod<T> : ICustomFilterMethod
    {
        /// <summary>
        /// Gets the expression used for custom filtering.
        /// </summary>
        Expression<Func<T, bool>> Expression { get; }

        /// <summary>
        /// Gets the expression used for custom filtering.
        /// </summary>
        Func<IFilterTerm, Expression<Func<T, bool>>> FilterTermExpression { get; }
    }
}
