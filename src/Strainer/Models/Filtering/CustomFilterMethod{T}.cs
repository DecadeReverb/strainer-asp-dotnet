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
    public class CustomFilterMethod<T> : CustomFilterMethod, ICustomFilterMethod<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomFilterMethod{TEntity}"/>
        /// class.
        /// </summary>
        public CustomFilterMethod()
        {

        }

        /// <summary>
        /// Gets or sets the expression used for custom filtering.
        /// </summary>
        public Expression<Func<T, bool>> Expression { get; set; }

        /// <summary>
        /// Gets or sets the expression used for custom filtering.
        /// </summary>
        public Func<IFilterTerm, Expression<Func<T, bool>>> FilterTermExpression { get; set; }
    }
}
