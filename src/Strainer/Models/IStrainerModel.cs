namespace Fluorite.Strainer.Models
{
    /// <summary>
    /// Represents stariner model used to bind filtering, sorting
    /// and pagination data.
    /// </summary>
    public interface IStrainerModel
    {
        /// <summary>
        /// Gets the filters.
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
        /// Gets the sortings.
        /// </summary>
        string Sorts { get; }
    }
}
