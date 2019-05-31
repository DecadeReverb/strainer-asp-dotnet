namespace Sieve.Models
{
    public interface ISieveModel
    {
        string Filters { get; set; }

        int? Page { get; set; }

        int? PageSize { get; set; }

        string Sorts { get; set; }
    }
}
