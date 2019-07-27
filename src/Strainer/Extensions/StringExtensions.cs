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

        /// <summary>
        /// Trims all trailing occurences of specified string in current string.
        /// </summary>
        /// <param name="source">
        /// Current <see cref="string"/> instance.
        /// </param>
        /// <param name="trimString">
        /// The string value to trim.
        /// </param>
        /// <returns>
        /// Trimmed string value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="trimString"/> is <see langword="null"/>.
        /// </exception>
        public static string TrimEnd(this string source, string trimString)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (trimString == null)
            {
                throw new ArgumentNullException(nameof(trimString));
            }

            if (source == string.Empty || trimString == string.Empty)
            {
                return source;
            }

            var result = source;
            while (result.EndsWith(trimString))
            {
                result = result.Substring(0, result.Length - trimString.Length);
            }

            return result;
        }

        /// <summary>
        /// Trims only one trailing occurence of specified string in current string.
        /// </summary>
        /// <param name="source">
        /// Current <see cref="string"/> instance.
        /// </param>
        /// <param name="trimString">
        /// The string value to trim.
        /// </param>
        /// <returns>
        /// Trimmed string value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="trimString"/> is <see langword="null"/>.
        /// </exception>
        public static string TrimEndOnce(this string source, string trimString)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (trimString == null)
            {
                throw new ArgumentNullException(nameof(trimString));
            }

            if (source == string.Empty || trimString == string.Empty)
            {
                return source;
            }

            if (source.EndsWith(trimString))
            {
                return source.Substring(0, source.Length - trimString.Length);
            }
            else
            {
                return source;
            }
        }

        /// <summary>
        /// Trims all leading occurences of specified string in current string.
        /// </summary>
        /// <param name="source">
        /// Current <see cref="string"/> instance.
        /// </param>
        /// <param name="trimString">
        /// The string value to trim.
        /// </param>
        /// <returns>
        /// Trimmed string value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="trimString"/> is <see langword="null"/>.
        /// </exception>
        public static string TrimStart(this string source, string trimString)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (trimString == null)
            {
                throw new ArgumentNullException(nameof(trimString));
            }

            if (source == string.Empty || trimString == string.Empty)
            {
                return source;
            }

            var result = source;
            while (result.StartsWith(trimString))
            {
                result = result.Substring(trimString.Length);
            }

            return result;
        }

        /// <summary>
        /// Trims only one leading occurence of specified string in current string.
        /// </summary>
        /// <param name="source">
        /// Current <see cref="string"/> instance.
        /// </param>
        /// <param name="trimString">
        /// The string value to trim.
        /// </param>
        /// <returns>
        /// Trimmed string value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="trimString"/> is <see langword="null"/>.
        /// </exception>
        public static string TrimStartOnce(this string source, string trimString)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (trimString == null)
            {
                throw new ArgumentNullException(nameof(trimString));
            }

            if (source == string.Empty || trimString == string.Empty)
            {
                return source;
            }

            if (source.StartsWith(trimString))
            {
                return source.Substring(trimString.Length);
            }
            else
            {
                return source;
            }
        }
    }
}
