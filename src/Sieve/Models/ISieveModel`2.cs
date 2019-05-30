using System.Collections.Generic;

namespace Sieve.Models
{
    public interface ISieveModel<TFilterTerm, TSortTerm>
        where TFilterTerm : IFilterTerm
        where TSortTerm : ISortTerm
    {
        string Filters { get; set; }

        int? Page { get; set; }

        int? PageSize { get; set; }

        string Sorts { get; set; }

        List<TSortTerm> GetSortsParsed();
    }
}
