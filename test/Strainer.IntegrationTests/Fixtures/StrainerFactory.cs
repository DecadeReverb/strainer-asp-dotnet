using Fluorite.Extensions.DependencyInjection;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Modules;
using Microsoft.Extensions.DependencyInjection;

namespace Fluorite.Strainer.IntegrationTests.Fixtures;

public class StrainerFactory : IDisposable
{
    public const ServiceLifetime ServicesLifetime = ServiceLifetime.Singleton;

    private readonly List<ServiceProvider> _serviceProviders;

    private bool _disposed;

    public StrainerFactory()
    {
        _serviceProviders = new ();
    }

    public IStrainerProcessor CreateDefaultProcessor<TModule>()
        where TModule : class, IStrainerModule
    {
        return CreateDefaultProcessor(typeof(TModule));
    }

    public IStrainerProcessor CreateDefaultProcessor(params Type[] strainerModuleTypes)
    {
        var serviceProvider = BuildStrainerServiceProvider(_ => { }, _ => { }, strainerModuleTypes);

        return serviceProvider.GetRequiredService<IStrainerProcessor>();
    }

    public IStrainerProcessor CreateDefaultProcessor<TModule>(Action<StrainerOptions> configureOptions)
        where TModule : class, IStrainerModule
    {
        return CreateDefaultProcessor(configureOptions, typeof(TModule));
    }

    public IStrainerProcessor CreateDefaultProcessor(Action<StrainerOptions> configureOptions, params Type[] strainerModuleTypes)
    {
        var serviceProvider = BuildStrainerServiceProvider(configureOptions, _ => { }, strainerModuleTypes);

        return serviceProvider.GetRequiredService<IStrainerProcessor>();
    }

    public IStrainerProcessor CreateProcessor<TModule>(Action<IServiceCollection> servicesConfig)
        where TModule : class, IStrainerModule
    {
        return CreateProcessor(servicesConfig, typeof(TModule));
    }

    public IStrainerProcessor CreateProcessor(Action<IServiceCollection> servicesConfig, params Type[] strainerModuleTypes)
    {
        var serviceProvider = BuildStrainerServiceProvider(_ => { }, servicesConfig, strainerModuleTypes);

        return serviceProvider.GetRequiredService<IStrainerProcessor>();
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected IStrainerContext CreateDefaultContext(
        Action<StrainerOptions> options,
        Action<IServiceCollection> services,
        params Type[] strainerModuleTypes)
    {
        var serviceProvider = BuildStrainerServiceProvider(options, services, strainerModuleTypes);

        return serviceProvider.GetRequiredService<IStrainerContext>();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                foreach (var serviceProvider in _serviceProviders)
                {
                    serviceProvider.Dispose();
                }

                _serviceProviders.Clear();
            }

            _disposed = true;
        }
    }

    private ServiceProvider BuildStrainerServiceProvider(
        Action<StrainerOptions> optionsConfig,
        Action<IServiceCollection> servicesConfig,
        Type[] strainerModuleTypes)
    {
        var services = new ServiceCollection();
        services.AddStrainer(optionsConfig, strainerModuleTypes, ServicesLifetime);
        servicesConfig(services);

        var serviceProvider = services.BuildServiceProvider();
        _serviceProviders.Add(serviceProvider);

        return serviceProvider;
    }
}