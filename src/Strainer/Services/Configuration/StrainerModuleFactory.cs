using Fluorite.Strainer.Services.Modules;

namespace Fluorite.Strainer.Services.Configuration;

public class StrainerModuleFactory : IStrainerModuleFactory
{
    public IStrainerModule CreateModule(Type moduleType)
    {
        Guard.Against.Null(moduleType);

        if (!typeof(IStrainerModule).IsAssignableFrom(moduleType))
        {
            throw new ArgumentException(
                $"Provider module type {moduleType.FullName} is not implementing {nameof(IStrainerModule)}.",
                nameof(moduleType));
        }

        try
        {
            return (IStrainerModule)Activator.CreateInstance(moduleType);
        }
        catch (Exception exception)
        {
            throw new InvalidOperationException(
                $"Unable to create instance of {moduleType.FullName}. " +
                $"Ensure that type provides parameterless constructor.",
                exception);
        }
    }
}
