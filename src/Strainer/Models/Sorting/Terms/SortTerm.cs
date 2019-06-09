using System;

namespace Fluorite.Strainer.Models.Sorting.Terms
{
    /// <summary>
    /// Provides detailed information about sorting expression.
    /// </summary>
    public class SortTerm : ISortTerm, IEquatable<SortTerm>
    {
        /// <summary>
        /// Initializes new instance of <see cref="SortTerm"/> class.
        /// </summary>
        public SortTerm()
        {

        }

        /// <summary>
        /// Gets or sets the original input, based on which current sort term was created.
        /// </summary>
        public string Input { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="bool"/> value indictating whether current sorting
        /// direction is descending.
        /// </summary>
        public bool IsDescending { get; set; }

        /// <summary>
        /// Gets or sets the name of sorting method.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Checks if current instance of <see cref="SortTerm"/> is equal
        /// to other <see cref="SortTerm"/> instance.
        /// </summary>
        /// <param name="other">
        /// Other <see cref="SortTerm"/> instance.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if provided other <see cref="object"/>
        /// instance is equal to the current one; otherwise <see langword="false"/>.
        /// </returns>
        public bool Equals(SortTerm other)
        {
            return other != null
                && IsDescending == other.IsDescending
                && Name == other.Name;
        }
    }
}
