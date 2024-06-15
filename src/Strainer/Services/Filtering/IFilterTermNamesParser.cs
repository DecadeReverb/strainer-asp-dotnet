namespace Fluorite.Strainer.Services.Filtering;

public interface IFilterTermNamesParser
{
    IList<string> Parse(string input);
}
