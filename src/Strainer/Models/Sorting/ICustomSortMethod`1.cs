using System;
using System.Linq;

namespace Fluorite.Strainer.Models.Sorting
{
    /// <summary>
    /// Represents custom sort method.
    /// </summary>
    /// <typeparam name="TEntity">
    /// The type of entity processed by the custom method.
    /// </typeparam>
    public interface ICustomSortMethod<TEntity> : ICustomSortMethod
    {
        /// <summary>
        /// Gets the function used for custom sorting.
        /// </summary>
        Func<IQueryable<TEntity>, bool, bool, IQueryable<TEntity>> Function { get; }
    }
}
