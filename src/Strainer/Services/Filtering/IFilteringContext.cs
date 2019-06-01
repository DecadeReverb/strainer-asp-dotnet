namespace Fluorite.Strainer.Services.Filtering
{
    public interface IFilteringContext
    {
        IFilterOperatorParser OperatorParser { get; }
        IFilterOperatorProvider OperatorProvider { get; }
        IFilterOperatorValidator OperatorValidator { get; }
        IFilterTermParser TermParser { get; }
    }
}
