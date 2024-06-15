namespace Fluorite.Strainer.Models;

/// <summary>
/// Represents Strainer model used to bind filtering, sorting
/// and pagination parameters.
/// </summary>
public interface IStrainerModel
{
    /// <summary>
    /// Gets the filter parameters.
    /// </summary>
    string Filters { get; }

    /// <summary>
    /// Gets the page number.
    /// </summary>
    int? Page { get; }

    /// <summary>
    /// Gets the page size.
    /// </summary>
    int? PageSize { get; }

    /// <summary>
    /// Gets the sorting parameters.
    /// </summary>
    string Sorts { get; }
}
