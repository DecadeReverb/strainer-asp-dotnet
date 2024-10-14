using Fluorite.Strainer.Models.Configuration;
using Fluorite.Strainer.Services.Modules;

namespace Fluorite.Strainer.Services.Configuration;

public interface IStrainerConfigurationBuilder
{
    IStrainerConfiguration Build();

    IStrainerConfigurationBuilder WithCustomFilterMethods(ICollection<IStrainerModule> modules);

    IStrainerConfigurationBuilder WithCustomSortMethods(ICollection<IStrainerModule> modules);

    IStrainerConfigurationBuilder WithDefaultMetadata(ICollection<IStrainerModule> modules);

    IStrainerConfigurationBuilder WithCustomFilterOperators(ICollection<IStrainerModule> modules);

    IStrainerConfigurationBuilder WithObjectMetadata(ICollection<IStrainerModule> modules);

    IStrainerConfigurationBuilder WithPropertyMetadata(ICollection<IStrainerModule> modules);

    IStrainerConfigurationBuilder WithoutBuiltInFilterOperators(ICollection<IStrainerModule> modules);
}