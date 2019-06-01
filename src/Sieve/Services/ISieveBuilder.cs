using Microsoft.Extensions.DependencyInjection;

namespace Strainer.Services
{
    public interface IStrainerBuilder
    {
        IServiceCollection Services { get; }
    }
}
