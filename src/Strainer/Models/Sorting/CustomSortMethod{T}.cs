namespace Fluorite.Strainer.Models.Sorting
{
    /// <summary>
    /// Represents custom sort method.
    /// </summary>
    /// <typeparam name="T">
    /// The type of entity processed by the custom method.
    /// </typeparam>
    public class CustomSortMethod<T> : CustomSortMethod, ICustomSortMethod<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomSortMethod{TEntity}"/>
        /// class.
        /// </summary>
        public CustomSortMethod()
        {

        }

        /// <summary>
        /// Gets or sets the function used for custom sorting.
        /// </summary>
        public Func<IQueryable<T>, bool, bool, IQueryable<T>> Function { get; set; }
    }
}