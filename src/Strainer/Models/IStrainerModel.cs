namespace Strainer.Models
{
    public interface IStrainerModel
    {
        string Filters { get; set; }

        int? Page { get; set; }

        int? PageSize { get; set; }

        string Sorts { get; set; }
    }
}
