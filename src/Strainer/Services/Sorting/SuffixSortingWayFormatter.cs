using Fluorite.Extensions;
using System;

namespace Fluorite.Strainer.Services.Sorting
{
    public class SuffixSortingWayFormatter : ISortingWayFormatter
    {
        /// <summary>
        /// The prefix used to mark by this formatter.
        /// <para/>
        /// This field is readonly.
        /// </summary>
        public static readonly string AscendingSuffix = "_asc";

        /// <summary>
        /// The suffix used to mark by this formatter.
        /// <para/>
        /// This field is readonly.
        /// </summary>
        public static readonly string DescendingSuffix = "_desc";

        /// <summary>
        /// Initializes new instance of <see cref="SuffixSortingWayFormatter"/>
        /// class.
        /// </summary>
        public SuffixSortingWayFormatter()
        {

        }

        public bool IsDescendingDefaultSortingWay => true;

        public string Format(string input, bool isDescending)
        {
            if (input is null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            return input + GetSuffix(isDescending);
        }

        public bool IsDescending(string input)
        {
            if (input is null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (input.EndsWith(DescendingSuffix))
            {
                return true;
            }

            if (input.EndsWith(AscendingSuffix))
            {
                return false;
            }

            return IsDescendingDefaultSortingWay;
        }

        public string Unformat(string input)
        {
            if (input is null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            if (input.EndsWith(AscendingSuffix))
            {
                return input.TrimEndOnce(AscendingSuffix);
            }
            else
            {
                if (input.EndsWith(DescendingSuffix))
                {
                    return input.TrimEndOnce(DescendingSuffix);
                }

                return input;
            }
        }

        private string GetSuffix(bool isDescending)
        {
            return isDescending
                ? DescendingSuffix
                : AscendingSuffix;
        }
    }
}
