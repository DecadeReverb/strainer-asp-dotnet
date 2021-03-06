using System;
using System.Linq;

namespace Fluorite.Strainer.Models.Sorting
{
    /// <summary>
    /// Represents custom sort method.
    /// </summary>
    /// <typeparam name="T">
    /// The type of entity processed by the custom method.
    /// </typeparam>
    public interface ICustomSortMethod<T> : ICustomSortMethod
    {
        /// <summary>
        /// Gets the function used for custom sorting.
        /// </summary>
        Func<IQueryable<T>, bool, bool, IQueryable<T>> Function { get; }
    }
}
