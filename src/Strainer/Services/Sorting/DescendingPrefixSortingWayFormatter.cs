using Fluorite.Extensions;
using Fluorite.Strainer.Models.Sorting;

namespace Fluorite.Strainer.Services.Sorting;

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
    public const string DescendingPrefix = "-";

    /// <summary>
    /// Initializes a new instance of the <see cref="DescendingPrefixSortingWayFormatter"/>
    /// class.
    /// </summary>
    public DescendingPrefixSortingWayFormatter()
    {

    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="input"/> is <see langword="null" />.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="sortingWay"/> is <see cref="SortingWay.Unknown"/>.
    /// </exception>
    public string Format(string input, SortingWay sortingWay)
    {
        Guard.Against.Null(input);

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

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="input"/> is <see langword="null" />.
    /// </exception>
    public SortingWay GetSortingWay(string input)
    {
        Guard.Against.Null(input);

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

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="input"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="sortingWay"/> is <see cref="SortingWay.Unknown"/>.
    /// </exception>
    public string Unformat(string input, SortingWay sortingWay)
    {
        Guard.Against.Null(input);

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

        return input.TrimStartOnce(DescendingPrefix);
    }
}
