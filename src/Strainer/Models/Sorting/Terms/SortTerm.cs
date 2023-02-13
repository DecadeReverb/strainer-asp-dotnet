using System.Diagnostics;

namespace Fluorite.Strainer.Models.Sorting.Terms
{
    /// <summary>
    /// Provides detailed information about sorting expression.
    /// </summary>
    [DebuggerDisplay("\\{" +
            nameof(Name) + " = " + "{" + nameof(Name) + "}, " +
            nameof(IsDescending) + " = " + "{" + nameof(IsDescending) + "}" + "\\}")]
    public class SortTerm : ISortTerm, IEquatable<SortTerm>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SortTerm"/> class.
        /// </summary>
        public SortTerm()
        {

        }

        /// <summary>
        /// Gets or sets the original input, based on which current sort term was created.
        /// </summary>
        public string Input { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether current sorting
        /// direction is descending.
        /// </summary>
        public bool IsDescending { get; set; }

        /// <summary>
        /// Gets or sets the name of sorting method.
        /// </summary>
        public string Name { get; set; }

        public static bool operator ==(SortTerm term1, SortTerm term2)
        {
            return EqualityComparer<SortTerm>.Default.Equals(term1, term2);
        }

        public static bool operator !=(SortTerm term1, SortTerm term2)
        {
            return !(term1 == term2);
        }

        /// <summary>
        /// Checks if current instance of <see cref="SortTerm"/>
        /// is equal to other <see cref="object"/> instance.
        /// </summary>
        /// <param name="obj">
        /// Other <see cref="object"/> instance.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if provided other <see cref="object"/>
        /// instance is equal to the current one; otherwise <see langword="false"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as SortTerm);
        }

        /// <summary>
        /// Checks if current instance of <see cref="SortTerm"/> is equal
        /// to other <see cref="SortTerm"/> instance.
        /// </summary>
        /// <param name="other">
        /// Other <see cref="SortTerm"/> instance.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if provided other <see cref="SortTerm"/>
        /// instance is equal to the current one; otherwise <see langword="false"/>.
        /// </returns>
        public bool Equals(SortTerm other)
        {
            return other != null &&
                   IsDescending == other.IsDescending &&
                   Name == other.Name;
        }

        /// <summary>
        /// Gets <see cref="int"/> hash code representation of current
        /// <see cref="SortTerm"/>.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="SortTerm"/>.
        /// </returns>
        public override int GetHashCode()
        {
            var hashCode = 1436560617;
            hashCode = (hashCode * -1521134295) + IsDescending.GetHashCode();
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(Name);
            return hashCode;
        }
    }
}
