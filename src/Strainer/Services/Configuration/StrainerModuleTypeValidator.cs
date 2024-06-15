using Fluorite.Strainer.Services.Modules;

namespace Fluorite.Strainer.Services.Configuration;

public class StrainerModuleTypeValidator : IStrainerModuleTypeValidator
{
    public ICollection<Type> GetValidModuleTypes(IReadOnlyCollection<Type> types)
    {
        var validModuleTypes = types
            .Where(type => !type.IsAbstract && typeof(IStrainerModule).IsAssignableFrom(type))
            .ToList();

        var invalidModuleTypes = types.Except(validModuleTypes);
        if (invalidModuleTypes.Any())
        {
            throw new InvalidOperationException(
                string.Format(
                    "Valid Strainer module must be a non-abstract class implementing `{0}`. " +
                    "Invalid types:\n{1}",
                    typeof(IStrainerModule).FullName,
                    string.Join("\n", invalidModuleTypes.Select(invalidType => invalidType.FullName))));
        }

        return validModuleTypes;
    }
}
