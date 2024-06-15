using Fluorite.Strainer.Exceptions;
using Fluorite.Strainer.Models.Metadata;

namespace Fluorite.Strainer.Services.Validation;

public class SortExpressionValidator : ISortExpressionValidator
{
    public void Validate(IReadOnlyDictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>> propertyMetadata)
    {
        if (propertyMetadata == null)
        {
            throw new ArgumentNullException(nameof(propertyMetadata));
        }

        foreach (var typeKeyPair in propertyMetadata)
        {
            var type = typeKeyPair.Key;
            var properties = typeKeyPair.Value.Values;
            var defaultSortingProperties = properties.Where(x => x.IsDefaultSorting).ToList();

            if (!defaultSortingProperties.Any())
            {
                var exceptionMessage =
                    $"No default sort expression found for type {type.FullName}.\n" +
                    $"Mark a property as default sorting to enable fallbacking " +
                    $"to it when no sorting information is provided.";

                throw new StrainerSortExpressionValidatorException(
                    type,
                    exceptionMessage);
            }

            if (defaultSortingProperties.Count > 1)
            {
                var defaultProperties = string.Join(
                    Environment.NewLine,
                    defaultSortingProperties.Select(x => x.Name));
                var exceptionMessage =
                    $"Too many default sort properties found for type {type.FullName}.\n" +
                    $"Only one property can be marked as default.\n" +
                    $"Default properties:\n" +
                    $"{defaultProperties}";

                throw new StrainerSortExpressionValidatorException(
                    type,
                    exceptionMessage);
            }
        }
    }
}
