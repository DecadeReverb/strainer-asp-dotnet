using Fluorite.Strainer.Models.Filter.Operators;

namespace Fluorite.Strainer.Services.Filter
{
    public interface IFilterOperatorParser
    {
        IFilterOperator GetParsedOperator(string symbol);
    }
}
