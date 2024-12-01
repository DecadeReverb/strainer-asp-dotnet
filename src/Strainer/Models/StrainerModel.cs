namespace Fluorite.Strainer.Models;

/// <summary>
/// Represents default Strainer model used to bind filtering, sorting
/// and pagination parameters.
/// </summary>
public class StrainerModel : IStrainerModel
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StrainerModel"/> class.
    /// </summary>
    public StrainerModel()
    {
    }

    /// <summary>
    /// Gets or sets the filters.
    /// </summary>
    public virtual string? Filters { get; set; }

    /// <summary>
    /// Gets or sets the page number.
    /// </summary>
    public virtual int? Page { get; set; }

    /// <summary>
    /// Gets or sets the page size.
    /// </summary>
    public virtual int? PageSize { get; set; }

    /// <summary>
    /// Gets or sets the sortings.
    /// </summary>
    public virtual string? Sorts { get; set; }
}
