using Fluorite.Strainer.Attributes;
using System.Reflection;

namespace Fluorite.Strainer.Services.Metadata.Attributes;

public class AttributeCriteriaChecker : IAttributeCriteriaChecker
{
    public bool CheckIfObjectAttributeIsMatching(
        StrainerObjectAttribute? attribute,
        PropertyInfo? propertyInfo,
        bool isSortableRequired,
        bool isFilterableRequired)
    {
        // An object attribue is matching when all of these conditions are met:
        // - StrainerObjectAttribute was found
        // - property info was found
        // - attribute has sortable flag enabled or sorting is not required
        // - attribute has filterable flag enabled or filtering is not required
        return attribute != null
            && propertyInfo != null
            && (!isSortableRequired || attribute.IsSortable)
            && (!isFilterableRequired || attribute.IsFilterable);
    }

    public bool CheckIfPropertyAttributeIsMatching(
        StrainerPropertyAttribute? attribute,
        PropertyInfo? propertyInfo,
        bool isSortableRequired,
        bool isFilterableRequired,
        string name)
    {
        // An property attribue is matching when all of these conditions are met:
        // - StrainerPropertyAttribute was found
        // - property info was found
        // - attribute has sortable flag enabled or sorting is not required
        // - attribute has filterable flag enabled or filtering is not required
        // - attribute/property name equals provided name
        return attribute is not null
            && propertyInfo != null
            && (!isSortableRequired || attribute.IsSortable)
            && (!isFilterableRequired || attribute.IsFilterable)
            && string.Equals(attribute.DisplayName ?? attribute.Name, name);
    }
}
