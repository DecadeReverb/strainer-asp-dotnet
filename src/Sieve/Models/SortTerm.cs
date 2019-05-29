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

        public bool IsDescending => _sort.StartsWith(DescendingWaySortingPrefix);

        public string Name => _sort.StartsWith(DescendingWaySortingPrefix)
            ? _sort.Substring(1)
            : _sort;

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

        public bool Equals(SortTerm other)
        {
            return Name == other.Name && IsDescending == other.IsDescending;
        }
    }
}
