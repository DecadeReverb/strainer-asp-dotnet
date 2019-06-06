using Fluorite.Strainer.Models.Filtering.Operators;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fluorite.Strainer.Models.Filtering.Terms
{
    public class FilterTerm : IFilterTerm, IEquatable<FilterTerm>
    {
        public FilterTerm()
        {
            Names = new List<string>();
            Values = new List<string>();
        }

        public string Input { get; set; }

        public IList<string> Names { get; set; }

        public string Operator { get; set; }

        public bool OperatorIsCaseInsensitive { get; set; }

        public bool OperatorIsNegated { get; set; }

        public IFilterOperator OperatorParsed { get; set; }

        public IList<string> Values { get; set; }

        public bool Equals(FilterTerm other)
        {
            return other != null
                && Names.SequenceEqual(other.Names)
                && Operator == other.Operator
                && Values.SequenceEqual(other.Values);
        }
    }
}
