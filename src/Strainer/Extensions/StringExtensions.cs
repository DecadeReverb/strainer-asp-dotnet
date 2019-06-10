using System;

namespace Fluorite.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="string"/>.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Checks whether a specified substring occurs within current one.
        /// </summary>
        /// <param name="source">
        /// Current <see cref="string"/> instance.
        /// </param>
        /// <param name="substring">
        /// The other instance of <see cref="string"/> to check.
        /// </param>
        /// <param name="comparisonType">
        /// String comparison type.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if specified substring occurs within current
        /// one; otherwise <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="substring"/> is <see langword="null"/>.
        /// </exception>
        public static bool Contains(this string source, string substring, StringComparison comparisonType)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (substring == null)
            {
                throw new ArgumentNullException(nameof(substring));
            }

            return source?.IndexOf(substring, comparisonType) >= 0;
        }
    }
}
