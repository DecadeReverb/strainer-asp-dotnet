namespace Fluorite.Strainer.Services.Filtering
{
    public interface IFilterOperatorMapper
    {
        IFilterOperatorBuilder Operator(string symbol);
    }
}
