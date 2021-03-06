using System;
using System.Linq;

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
        /// Gets the function used for custom filtering.
        /// </summary>
        Func<IQueryable<T>, string, IQueryable<T>> Function { get; }
    }
}
