using Fluorite.Strainer.Models.Filtering.Operators;
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
        /// Gets the unparsed <see cref="string"/> filter operator.
        /// </summary>
        string Operator { get; }

        /// <summary>
        /// Gets a <see cref="bool"/> value indictating whether used
        /// operator is case insensitive.
        /// </summary>
        bool OperatorIsCaseInsensitive { get; }

        /// <summary>
        /// Gets a <see cref="bool"/> value indictating whether used
        /// operator is a negated version of a different operator.
        /// </summary>
        bool OperatorIsNegated { get; }

        /// <summary>
        /// Gets the parsed filter operator.
        /// </summary>
        IFilterOperator OperatorParsed { get; }

        /// <summary>
        /// Gets the list of values.
        /// </summary>
        IList<string> Values { get; }
    }
}
