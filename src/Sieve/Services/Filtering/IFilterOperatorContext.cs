namespace Sieve.Services.Filtering
{
    public interface IFilterOperatorContext
    {
        IFilterOperatorParser Parser { get; }
        IFilterOperatorProvider Provider { get; }
        IFilterOperatorValidator Validator { get; }
    }
}
