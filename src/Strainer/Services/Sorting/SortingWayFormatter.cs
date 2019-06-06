using System;

namespace Fluorite.Strainer.Services.Sorting
{
    public class SortingWayFormatter : ISortingWayFormatter
    {
        public static readonly string DescendingWaySortingPrefix = "-";

        public SortingWayFormatter()
        {

        }

        public string Format(string input, bool isDescending)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            return isDescending
                ? DescendingWaySortingPrefix + input
                : input;
        }

        public bool IsDescending(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            return input.StartsWith(DescendingWaySortingPrefix);
        }

        public string Unformat(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }
            else
            {
                return input.StartsWith(DescendingWaySortingPrefix)
                    ? input.Substring(1)
                    : input;
            }
        }
    }
}
