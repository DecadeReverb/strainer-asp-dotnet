namespace Fluorite.Strainer.Models.Sorting.Terms;

/// <summary>
/// Provides detailed information about sorting expression.
/// </summary>
public interface ISortTerm
{
    /// <summary>
    /// Gets the original input, based on which current sort term was created.
    /// </summary>
    string? Input { get; }

    /// <summary>
    /// Gets a value indicating whether current sorting
    /// direction is descending.
    /// </summary>
    bool IsDescending { get; }

    /// <summary>
    /// Gets the name of sorting method.
    /// </summary>
    string Name { get; }
}
