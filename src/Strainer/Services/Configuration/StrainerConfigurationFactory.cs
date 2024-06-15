using Fluorite.Strainer.Models.Configuration;

namespace Fluorite.Strainer.Services.Configuration;

public class StrainerConfigurationFactory : IStrainerConfigurationFactory
{
    private readonly IStrainerModuleLoader _strainerModuleLoader;
    private readonly IStrainerModuleFactory _strainerModuleFactory;
    private readonly IStrainerModuleTypeValidator _strainerModuleTypeValidator;

    public StrainerConfigurationFactory(
        IStrainerModuleLoader strainerModuleLoader,
        IStrainerModuleFactory strainerModuleFactory,
        IStrainerModuleTypeValidator strainerModuleTypeValidator)
    {
        _strainerModuleLoader = Guard.Against.Null(strainerModuleLoader);
        _strainerModuleFactory = Guard.Against.Null(strainerModuleFactory);
        _strainerModuleTypeValidator = Guard.Against.Null(strainerModuleTypeValidator);
    }

    public IStrainerConfiguration Create(IReadOnlyCollection<Type> moduleTypes)
    {
        Guard.Against.Null(moduleTypes);

        var validModuleTypes = _strainerModuleTypeValidator.GetValidModuleTypes(moduleTypes);
        var modules = validModuleTypes
            .Select(type => _strainerModuleFactory.CreateModule(type))
            .Where(instance => instance != null)
            .ToList();

        modules.ForEach(strainerModule => _strainerModuleLoader.Load(strainerModule));

        return new StrainerConfigurationBuilder()
            .WithPropertyMetadata(modules)
            .WithDefaultMetadata(modules)
            .WithObjectMetadata(modules)
            .WithFilterOperators(modules)
            .WithCustomFilterMethods(modules)
            .WithCustomSortMethods(modules)
            .Build();
    }
}
