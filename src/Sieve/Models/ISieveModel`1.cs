namespace Sieve.Models
{
    public interface ISieveModel<TSortTerm>
        where TSortTerm : ISortTerm
    {
        string Filters { get; set; }

        int? Page { get; set; }

        int? PageSize { get; set; }

        string Sorts { get; set; }
    }
}
