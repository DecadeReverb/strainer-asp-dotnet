using Microsoft.Extensions.DependencyInjection;

namespace Fluorite.Strainer.Services
{
    public interface IStrainerBuilder
    {
        IServiceCollection Services { get; }
    }
}
