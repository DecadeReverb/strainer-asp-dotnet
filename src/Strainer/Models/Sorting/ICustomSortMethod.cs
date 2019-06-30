using System;
using System.Linq;

namespace Fluorite.Strainer.Models.Sorting
{
    /// <summary>
    /// Represents custom sort method.
    /// </summary>
    /// <typeparam name="TEntity">
    /// The type of entity processed by custom method.
    /// </typeparam>
    public interface ICustomSortMethod<TEntity>
    {
        /// <summary>
        /// Gets the function used for custom sorting.
        /// </summary>
        Func<ICustomSortMethodContext<TEntity>, IQueryable<TEntity>> Function { get; }

        /// <summary>
        /// Gets the custom method name.
        /// </summary>
        string Name { get; }
    }
}
