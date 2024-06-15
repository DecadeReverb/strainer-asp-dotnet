using Fluorite.Extensions;
using Fluorite.Strainer.Models.Sorting;

namespace Fluorite.Strainer.Services.Sorting;

/// <summary>
/// Provides sorting way formatter based on a suffix indicator.
/// </summary>
public class SuffixSortingWayFormatter : ISortingWayFormatter
{
    /// <summary>
    /// The prefix used to mark by this formatter.
    /// <para/>
    /// This field is readonly.
    /// </summary>
    public const string AscendingSuffix = "_asc";

    /// <summary>
    /// The suffix used to mark by this formatter.
    /// <para/>
    /// This field is readonly.
    /// </summary>
    public const string DescendingSuffix = "_desc";

    /// <summary>
    /// Initializes a new instance of the <see cref="SuffixSortingWayFormatter"/>
    /// class.
    /// </summary>
    public SuffixSortingWayFormatter()
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
    /// <exception cref="ArgumentNullException">
    /// <paramref name="input"/> is <see langword="null"/>.
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

        return input + GetSuffix(sortingWay);
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
    /// empty).
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="input"/> is <see langword="null"/>.
    /// </exception>
    public SortingWay GetSortingWay(string input)
    {
        Guard.Against.Null(input);

        if (string.IsNullOrWhiteSpace(input))
        {
            return SortingWay.Unknown;
        }

        if (input.EndsWith(DescendingSuffix))
        {
            return SortingWay.Descending;
        }

        if (input.EndsWith(AscendingSuffix))
        {
            return SortingWay.Ascending;
        }

        return SortingWay.Unknown;
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

        switch (sortingWay)
        {
            case SortingWay.Ascending:
                input = input.TrimEndOnce(AscendingSuffix);
                break;
            case SortingWay.Descending:
                input = input.TrimEndOnce(DescendingSuffix);
                break;
        }

        return input;
    }

    private string GetSuffix(SortingWay sortingWay) => sortingWay switch
    {
        SortingWay.Descending => DescendingSuffix,
        SortingWay.Ascending => AscendingSuffix,
        _ => throw new NotSupportedException($"{nameof(sortingWay)} with value '{sortingWay}' is not supported."),
    };
}
