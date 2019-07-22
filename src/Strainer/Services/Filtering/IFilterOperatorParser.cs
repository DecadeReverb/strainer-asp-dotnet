using Fluorite.Strainer.Models.Filter.Operators;

namespace Fluorite.Strainer.Services.Filtering
{
    public interface IFilterOperatorParser
    {
        IFilterOperator GetParsedOperator(string symbol);
    }
}
