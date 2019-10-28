using Fluorite.Extensions;
using System;

namespace Fluorite.Strainer.Services.Sorting
{
    /// <summary>
    /// Provides means of two-way formatting and recognizing a sorting
    /// direction on input value based on prefix added on when descending ordering.
    /// </summary>
    public class DescendingPrefixSortingWayFormatter : ISortingWayFormatter
    {
        /// <summary>
        /// The prefix used to mark by this formatter.
        /// <para/>
        /// This field is readonly.
        /// </summary>
        public static readonly string Prefix = "-";

        /// <summary>
        /// Initializes new instance of <see cref="DescendingPrefixSortingWayFormatter"/>
        /// class.
        /// </summary>
        public DescendingPrefixSortingWayFormatter()
        {

        }

        public string Format(string input, bool isDescending)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            return isDescending
                ? Prefix + input
                : input;
        }

        /// <summary>
        /// Checks whether provided value is formatted in a descending way.
        /// <para/>
        /// Take notice that even if provided value is not formatted
        /// in a descending way, this method may still return <see langword="true"/>
        /// if it fallbacks to its default (descending) sorting way.
        /// </summary>
        /// <param name="input">
        /// The value to check for sorting way.
        /// </param>
        /// <returns>
        /// A <see cref="bool"/> value, <see langword="true"/> if the value
        /// is descending; otherwise - <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> is <see langword="null"/>.
        /// </exception>
        public bool IsDescending(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            return input.StartsWith(Prefix);
        }

        public string Unformat(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }
            else
            {
                return input.TrimStartOnce(Prefix);
            }
        }
    }
}
