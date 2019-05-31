using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace Sieve.Models
{
    [DataContract]
    public class SieveModel<TSortTerm> : ISieveModel<TSortTerm>
        where TSortTerm : ISortTerm, new()
    {
        private const string EscapedCommaPattern = @"(?<!($|[^\\])(\\\\)*?\\),";

        public SieveModel()
        {

        }

        [DataMember]
        public string Filters { get; set; }

        [DataMember]
        public string Sorts { get; set; }

        [DataMember, Range(1, int.MaxValue)]
        public int? Page { get; set; }

        [DataMember, Range(1, int.MaxValue)]
        public int? PageSize { get; set; }

        // TODO:
        // Move this logic to some kind of sorting parser.
        // A DTO should not have such business logic, nethier deal with parsing.
        public List<TSortTerm> GetSortsParsed()
        {
            if (Sorts != null)
            {
                var value = new List<TSortTerm>();
                foreach (var sort in Regex.Split(Sorts, EscapedCommaPattern))
                {
                    if (string.IsNullOrWhiteSpace(sort))
                    {
                        continue;
                    }

                    var sortTerm = new TSortTerm()
                    {
                        Input = sort
                    };

                    if (!value.Any(s => s.Name == sortTerm.Name))
                    {
                        value.Add(sortTerm);
                    }
                }

                return value;
            }
            else
            {
                return null;
            }
        }
    }
}
