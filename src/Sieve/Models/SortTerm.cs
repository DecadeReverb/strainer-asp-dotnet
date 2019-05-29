using System;

namespace Sieve.Models
{
    public class SortTerm : ISortTerm, IEquatable<SortTerm>
    {
        private const string DescendingWaySortingPrefix = "-";

        private string _sort;

        public SortTerm()
        {

        }

        // TODO:
        // Getting the real name without hardcoded descending sorting way
        // prefix. A DTO should not do this, neither have such business logic.
        public string Sort
        {
            set
            {
                _sort = value;
            }
        }

        public string Name => _sort.StartsWith(DescendingWaySortingPrefix)
            ? _sort.Substring(1)
            : _sort;

        public bool Descending => _sort.StartsWith(DescendingWaySortingPrefix);

        public bool Equals(SortTerm other)
        {
            return Name == other.Name
                && Descending == other.Descending;
        }
    }
}
