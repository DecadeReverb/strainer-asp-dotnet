using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Fluorite.Strainer.Models
{
    public class StrainerModel : IStrainerModel
    {
        public StrainerModel()
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
