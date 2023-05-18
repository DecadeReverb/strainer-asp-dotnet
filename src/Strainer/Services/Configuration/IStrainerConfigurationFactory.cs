using Fluorite.Strainer.Models.Configuration;

namespace Fluorite.Strainer.Services.Configuration
{
    public interface IStrainerConfigurationFactory
    {
        IStrainerConfiguration Create(IReadOnlyCollection<Type> moduleTypes);
    }
}
