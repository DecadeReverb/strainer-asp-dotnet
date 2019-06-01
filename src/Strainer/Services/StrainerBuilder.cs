using Microsoft.Extensions.DependencyInjection;

namespace Fluorite.Strainer.Services
{
    public class StrainerBuilder : IStrainerBuilder
    {
        public StrainerBuilder(IServiceCollection services)
        {
            Services = services ?? throw new System.ArgumentNullException(nameof(services));
        }

        public IServiceCollection Services { get; }
    }
}
