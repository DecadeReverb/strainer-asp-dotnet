namespace Fluorite.Strainer.Services.Sorting
{
    /// <summary>
    /// Provides means of two-way formatting and recognizing a sorting
    /// direction on input value.
    /// </summary>
    public interface ISortingWayFormatter
    {
        string Format(string input, bool isDescending);

        /// <summary>
        /// Checks whether provided value is formatted in a descending way.
        /// <para/>
        /// Take notice that even if provided value is not formatted
        /// in a descending way, this method may still return <see langword="true"/>
        /// if it fallbacks to its default (descending) sorting way.
        /// </summary>
        /// <param name="input">
        /// The value to check for sorting way.
        /// </param>
        /// <returns>
        /// A <see cref="bool"/> value, <see langword="true"/> if the value
        /// is descending; otherwise - <see langword="false"/>.
        /// </returns>
        bool IsDescending(string input);

        string Unformat(string input);
    }
}
