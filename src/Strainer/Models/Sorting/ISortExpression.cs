﻿namespace Fluorite.Strainer.Models.Sorting
{
    /// <summary>
    /// Provides additional information about expression used for sorting.
    /// </summary>
    public interface ISortExpression
    {
        /// <summary>
        /// Gets a <see cref="bool"/> value indictating whether related
        /// expression should be used for ordering in descending way.
        /// </summary>
        bool IsDescending { get; }

        /// <summary>
        /// Gets a <see cref="bool"/> value indictating whether related
        /// expression should be used for subsequent ordering e.g. using
        /// ThenBy() or ThenByDescending().
        /// </summary>
        bool IsSubsequent { get; }
    }
}