namespace Fluorite.Strainer.Services
{
    public interface IStrainerCustomMethodsContext
    {
        IStrainerCustomFilterMethods FilterMethods { get; }
        IStrainerCustomSortMethods SortMethods { get; }
    }
}
