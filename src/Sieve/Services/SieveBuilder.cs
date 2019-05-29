using Microsoft.Extensions.DependencyInjection;

namespace Sieve.Services
{
    public class SieveBuilder : ISieveBuilder
    {
        public SieveBuilder(IServiceCollection services)
        {
            Services = services ?? throw new System.ArgumentNullException(nameof(services));
        }

        public IServiceCollection Services { get; }
    }
}
