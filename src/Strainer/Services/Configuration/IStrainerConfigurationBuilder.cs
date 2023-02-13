using Fluorite.Strainer.Models.Configuration;

namespace Fluorite.Strainer.Services.Configuration
{
    public interface IStrainerConfigurationBuilder
    {
        IStrainerConfiguration Build(IReadOnlyCollection<Type> moduleTypes);
    }
}
