using Fluorite.Strainer.Models.Filtering.Operators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fluorite.Strainer.Models.Filtering.Terms
{
    /// <summary>
    /// Provides detailed information about filter expression.
    /// </summary>
    public class FilterTerm : IFilterTerm, IEquatable<FilterTerm>
    {
        /// <summary>
        /// Initializes new instance of <see cref="FilterTerm"/> class.
        /// </summary>
        public FilterTerm(string input)
        {
            Input = input;
            Names = new List<string>();
            Values = new List<string>();
        }

        /// <summary>
        /// Gets the original input, based on which current filter term was created.
        /// </summary>
        public string Input { get; }

        /// <summary>
        /// Gets or sets the list of names.
        /// </summary>
        public IList<string> Names { get; set; }

        /// <summary>
        /// Gets or sets the unparsed <see cref="string"/> filter operator.
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="bool"/> value indictating whether used
        /// operator is case insensitive.
        /// </summary>
        public bool OperatorIsCaseInsensitive { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="bool"/> value indictating whether used
        /// operator is a negated version of a different operator.
        /// </summary>
        public bool OperatorIsNegated { get; set; }

        /// <summary>
        /// Gets or sets the parsed filter operator.
        /// </summary>
        public IFilterOperator OperatorParsed { get; set; }

        /// <summary>
        /// Gets or sets the list of values.
        /// </summary>
        public IList<string> Values { get; set; }

        /// <summary>
        /// Checks if current instance of <see cref="FilterTerm"/> is equal
        /// to other <see cref="FilterTerm"/> instance.
        /// </summary>
        /// <param name="other">
        /// Other <see cref="FilterTerm"/> instance.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if provided other <see cref="object"/>
        /// instance is equal to the current one; otherwise <see langword="false"/>.
        /// </returns>
        public bool Equals(FilterTerm other)
        {
            return other != null
                && Names.SequenceEqual(other.Names)
                && Operator == other.Operator
                && Values.SequenceEqual(other.Values);
        }
    }
}
