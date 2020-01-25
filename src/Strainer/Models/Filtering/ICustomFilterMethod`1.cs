using System;
using System.Linq;

namespace Fluorite.Strainer.Models.Filtering
{
    /// <summary>
    /// Represents custom filter method.
    /// </summary>
    /// <typeparam name="TEntity">
    /// The type of entity processed by the custom method.
    /// </typeparam>
    public interface ICustomFilterMethod<TEntity> : ICustomFilterMethod
    {
        /// <summary>
        /// Gets the function used for custom filtering.
        /// </summary>
        Func<IQueryable<TEntity>, string, IQueryable<TEntity>> Function { get; }
    }
}
