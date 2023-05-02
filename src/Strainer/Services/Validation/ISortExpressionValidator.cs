using Fluorite.Strainer.Models.Metadata;

namespace Fluorite.Strainer.Services.Validation
{
    public interface ISortExpressionValidator
    {
        void Validate(IReadOnlyDictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>> propertyMetadata);
    }
}
