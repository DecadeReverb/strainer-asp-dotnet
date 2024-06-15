using Fluorite.Strainer.Models.Sorting;

namespace Fluorite.Strainer.Services.Sorting;

/// <summary>
/// Provides means of two-way formatting and recognizing a sorting
/// direction on input value.
/// </summary>
public interface ISortingWayFormatter
{
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
    string Format(string input, SortingWay sortingWay);

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
    SortingWay GetSortingWay(string input);

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
    string Unformat(string input, SortingWay sortingWay);
}
