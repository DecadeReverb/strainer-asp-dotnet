using System;
using System.Collections.Generic;
using System.Linq;

namespace Fluorite.Strainer.Models
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
            return Names.SequenceEqual(other.Names)
                && Values.SequenceEqual(other.Values)
                && Operator == other.Operator;
        }
    }
}
