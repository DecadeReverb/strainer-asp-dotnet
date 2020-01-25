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
    public class CustomSortMethod<TEntity> : CustomSortMethod, ICustomSortMethod<TEntity>
    {
        /// <summary>
        /// Initializes new instance of the <see cref="CustomSortMethod{TEntity}"/>
        /// class.
        /// </summary>
        public CustomSortMethod()
        {

        }

        /// <summary>
        /// Gets or sets the function used for custom sorting.
        /// </summary>
        public Func<IQueryable<TEntity>, bool, bool, IQueryable<TEntity>> Function { get; set; }
    }
}