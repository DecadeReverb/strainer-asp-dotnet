using Fluorite.Strainer.Models.Filtering.Operators;

namespace Fluorite.Strainer.Services.Filtering
{
    public interface IConfigurationFilterOperatorsProvider
    {
        /// <summary>
        /// Gets the object filter operator dictionary.
        /// </summary>
        IReadOnlyDictionary<string, IFilterOperator> GetFilterOperators();
    }
}
