namespace Fluorite.Strainer.Models.Sorting;

/// <summary>
/// Provides information about expression used for sorting.
/// </summary>
public class SortExpression : ISortExpression
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SortExpression"/> class.
    /// </summary>
    public SortExpression()
    {

    }

    /// <summary>
    /// Gets or sets a value indicating whether related
    /// expression should be used as a default one.
    /// </summary>
    public bool IsDefault { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether related
    /// expression should be used for ordering in descending way.
    /// </summary>
    public bool IsDescending { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether related
    /// expression should be used for subsequent ordering e.g. using
    /// ThenBy() or ThenByDescending().
    /// </summary>
    public bool IsSubsequent { get; set; }
}
