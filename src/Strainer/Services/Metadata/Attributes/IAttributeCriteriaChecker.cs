using Fluorite.Strainer.Attributes;
using System.Reflection;

namespace Fluorite.Strainer.Services.Metadata.Attributes;

public interface IAttributeCriteriaChecker
{
    bool CheckIfObjectAttributeIsMatching(
        StrainerObjectAttribute? attribute,
        PropertyInfo? propertyInfo,
        bool isSortableRequired,
        bool isFilterableRequired);

    bool CheckIfPropertyAttributeIsMatching(
        StrainerPropertyAttribute? attribute,
        PropertyInfo? propertyInfo,
        bool isSortableRequired,
        bool isFilterableRequired,
        string name);
}
