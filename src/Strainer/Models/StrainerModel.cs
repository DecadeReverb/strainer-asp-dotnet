﻿namespace Fluorite.Strainer.Models
{
    /// <summary>
    /// Represents default strainer model used to bind filtering, sorting
    /// and pagination data.
    /// </summary>
    public class StrainerModel : IStrainerModel
    {
        /// <summary>
        /// Initializes new instance of <see cref="StrainerModel"/> class.
        /// </summary>
        public StrainerModel()
        {

        }

        /// <summary>
        /// Gets or sets the filters.
        /// </summary>
        public virtual string Filters { get; set; }

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
        public virtual string Sorts { get; set; }
    }
}
