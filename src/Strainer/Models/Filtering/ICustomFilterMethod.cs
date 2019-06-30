using System;
using System.Linq;

namespace Fluorite.Strainer.Models.Filtering
{
    /// <summary>
    /// Represents custom filter method.
    /// </summary>
    /// <typeparam name="TEntity">
    /// The type of entity processed by custom method.
    /// </typeparam>
    public interface ICustomFilterMethod<TEntity>
    {
        /// <summary>
        /// Gets the function used for custom filtering.
        /// </summary>
        Func<ICustomFilterMethodContext<TEntity>, IQueryable<TEntity>> Function { get; }

        /// <summary>
        /// Gets the custom method name.
        /// </summary>
        string Name { get; }
    }
}
