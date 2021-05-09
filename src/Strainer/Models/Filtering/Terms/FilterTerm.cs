using Fluorite.Strainer.Models.Filtering.Operators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fluorite.Strainer.Models.Filtering.Terms
{
    /// <summary>
    /// Provides detailed information about filter term.
    /// </summary>
    public class FilterTerm : IFilterTerm, IEquatable<FilterTerm>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterTerm"/> class.
        /// </summary>
        /// <param name="input">
        /// The filter input from Strainer model.
        /// </param>
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
        /// Gets or sets the filter operator.
        /// </summary>
        public IFilterOperator Operator { get; set; }

        /// <summary>
        /// Gets or sets the list of values.
        /// </summary>
        public IList<string> Values { get; set; }

        public static bool operator ==(FilterTerm term1, FilterTerm term2)
        {
            return EqualityComparer<FilterTerm>.Default.Equals(term1, term2);
        }

        public static bool operator !=(FilterTerm term1, FilterTerm term2)
        {
            return !(term1 == term2);
        }

        /// <summary>
        /// Checks if current instance of <see cref="FilterTerm"/>
        /// is equal to other <see cref="object"/> instance.
        /// </summary>
        /// <param name="obj">
        /// Other <see cref="object"/> instance.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if provided other <see cref="object"/>
        /// instance is equal to the current one; otherwise <see langword="false"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as FilterTerm);
        }

        /// <summary>
        /// Checks if current instance of <see cref="FilterTerm"/>
        /// is equal to other <see cref="FilterTerm"/> instance.
        /// </summary>
        /// <param name="other">
        /// Other <see cref="FilterTerm"/> instance.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if provided other <see cref="FilterTerm"/>
        /// instance is equal to the current one; otherwise <see langword="false"/>.
        /// </returns>
        public bool Equals(FilterTerm other)
        {
            return other != null &&
                   Names.SequenceEqual(other.Names) &&
                   EqualityComparer<IFilterOperator>.Default.Equals(Operator, other.Operator) &&
                   Values.SequenceEqual(other.Values);

        }

        /// <summary>
        /// Gets <see cref="int"/> hash code representation of current
        /// <see cref="FilterTerm"/>.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="FilterTerm"/>.
        /// </returns>
        public override int GetHashCode()
        {
            var hashCode = 215681951;
            hashCode = (hashCode * -1521134295) + EqualityComparer<IList<string>>.Default.GetHashCode(Names);
            hashCode = (hashCode * -1521134295) + EqualityComparer<IFilterOperator>.Default.GetHashCode(Operator);
            hashCode = (hashCode * -1521134295) + EqualityComparer<IList<string>>.Default.GetHashCode(Values);

            return hashCode;
        }
    }
}
