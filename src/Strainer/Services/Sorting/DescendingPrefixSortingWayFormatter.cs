using Fluorite.Extensions;
using Fluorite.Strainer.Models.Sorting;
using System;

namespace Fluorite.Strainer.Services.Sorting
{
    /// <summary>
    /// Provides sorting way formatter based on presence of descending prefix.
    /// </summary>
    public class DescendingPrefixSortingWayFormatter : ISortingWayFormatter
    {
        /// <summary>
        /// The prefix used to mark a descending sorting term by this formatter.
        /// <para/>
        /// This field is readonly.
        /// </summary>
        public static readonly string DescendingPrefix = "-";

        /// <summary>
        /// Initializes new instance of <see cref="DescendingPrefixSortingWayFormatter"/>
        /// class.
        /// </summary>
        public DescendingPrefixSortingWayFormatter()
        {

        }

        /// <summary>
        /// Applies formatting to input value according to specified sorting way.
        /// </summary>
        /// <param name="input">
        /// The sorting value to be formatted.
        /// </param>
        /// <param name="sortingWay">
        /// The sorting way which format will be applied upon the input.
        /// </param>
        /// <returns>
        /// A formatted value.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="sortingWay"/> is <see cref="SortingWay.Unknown"/>.
        /// </exception>
        public string Format(string input, SortingWay sortingWay)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (sortingWay == SortingWay.Unknown)
            {
                throw new ArgumentException(
                    $"{nameof(sortingWay)} cannot be {nameof(SortingWay.Unknown)}.",
                    nameof(sortingWay));
            }

            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            return sortingWay == SortingWay.Descending
                ? DescendingPrefix + input
                : input;
        }

        /// <summary>
        /// Gets the sorting way based on input.
        /// <para/>
        /// </summary>
        /// <param name="input">
        /// The value to check for sorting way.
        /// </param>
        /// <returns>
        /// <see cref="SortingWay.Ascending"/> if the input is formatted in
        /// ascending way; <see cref="SortingWay.Descending"/> if the input
        /// is formatted in descending way; <see cref="SortingWay.Unknown"/>
        /// if the sorting way cannot be established (e.g. the input was
        /// <see langword="null"/>).
        /// </returns>
        public SortingWay GetSortingWay(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return SortingWay.Unknown;
            }

            if (input.StartsWith(DescendingPrefix))
            {
                return SortingWay.Descending;
            }

            return SortingWay.Ascending;
        }

        /// <summary>
        /// Removes sorting way formatting from provided input value.
        /// </summary>
        /// <param name="input">
        /// The input value to be unformatted.
        /// </param>
        /// <param name="sortingWay">
        /// The sorting way of which format will be removed from the input.
        /// </param>
        /// <returns>
        /// An unformatted value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="sortingWay"/> is <see cref="SortingWay.Unknown"/>.
        /// </exception>
        public string Unformat(string input, SortingWay sortingWay)
        {
            if (input is null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (sortingWay == SortingWay.Unknown)
            {
                throw new ArgumentException(
                    $"{nameof(sortingWay)} cannot be {nameof(SortingWay.Unknown)}.",
                    nameof(sortingWay));
            }

            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            if (input.StartsWith(DescendingPrefix))
            {
                return input.TrimStartOnce(DescendingPrefix);
            }

            return input;
        }
    }
}
