using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

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
    }
}
