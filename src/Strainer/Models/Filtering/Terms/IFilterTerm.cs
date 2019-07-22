using Fluorite.Strainer.Models.Filter.Operators;
using System.Collections.Generic;

namespace Fluorite.Strainer.Models.Filtering.Terms
{
    /// <summary>
    /// Provides detailed information about filter expression.
    /// </summary>
    public interface IFilterTerm
    {
        /// <summary>
        /// Gets the original input, based on which current filter term was created.
        /// </summary>
        string Input { get; }

        /// <summary>
        /// Gets the list of names.
        /// </summary>
        IList<string> Names { get; }

        /// <summary>
        /// Gets the filter operator.
        /// </summary>
        IFilterOperator Operator { get; }

        /// <summary>
        /// Gets the list of values.
        /// </summary>
        IList<string> Values { get; }
    }
}
