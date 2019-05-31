namespace Sieve.Services
{
    public interface ISieveCustomMethodsContext
    {
        ISieveCustomFilterMethods FilterMethods { get; }
        ISieveCustomSortMethods SortMethods { get; }
    }
}
