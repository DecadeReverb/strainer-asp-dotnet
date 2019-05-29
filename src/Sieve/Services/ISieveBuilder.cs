using Microsoft.Extensions.DependencyInjection;

namespace Sieve.Services
{
    public interface ISieveBuilder
    {
        IServiceCollection Services { get; }
    }
}
