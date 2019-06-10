using Fluorite.Strainer.Models.Filtering.Operators;

namespace Fluorite.Strainer.Services.Filtering
{
    public interface IFilterOperatorParser
    {
        IFilterOperator GetParsedOperator(string symbol);
    }
}
