using System;

namespace Fluorite.Strainer.Models.Sorting.Terms
{
    public class SortTerm : ISortTerm, IEquatable<SortTerm>
    {
        public SortTerm()
        {

        }

        public string Input { get; set; }

        public bool IsDescending { get; set; }

        public string Name { get; set; }

        public bool Equals(SortTerm other)
        {
            return other != null
                && IsDescending == other.IsDescending
                && Name == other.Name;
        }
    }
}
