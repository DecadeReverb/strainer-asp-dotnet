using System.Collections.Generic;

namespace Fluorite.Strainer.Services.Filtering
{
    public interface IFilterTermValuesParser
    {
        IList<string> Parse(string input);
    }
}
