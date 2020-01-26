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
    public class CustomFilterMethod<TEntity> : CustomFilterMethod, ICustomFilterMethod<TEntity>
    {
        /// <summary>
        /// Initializes new instance of the <see cref="CustomFilterMethod{TEntity}"/>
        /// class.
        /// </summary>
        public CustomFilterMethod()
        {

        }

        /// <summary>
        /// Gets or sets the function used for custom filtering.
        /// </summary>
        public Func<IQueryable<TEntity>, string, IQueryable<TEntity>> Function { get; set; }
    }
}
