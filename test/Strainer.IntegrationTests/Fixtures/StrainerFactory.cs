using Fluorite.Extensions.DependencyInjection;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Configuration;
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
        _serviceProviders = [];
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

    public IStrainerConfigurationProvider CreateDefaultConfigurationProvider<TModule>()
        where TModule : class, IStrainerModule
    {
        return CreateDefaultConfigurationProvider(typeof(TModule));
    }

    public IStrainerConfigurationProvider CreateDefaultConfigurationProvider(params Type[] strainerModuleTypes)
    {
        return CreateDefaultConfigurationProvider(o => { }, strainerModuleTypes);
    }

    public IStrainerConfigurationProvider CreateDefaultConfigurationProvider<TModule>(Action<StrainerOptions> options)
        where TModule : class, IStrainerModule
    {
        return CreateDefaultConfigurationProvider(options, typeof(TModule));
    }

    public IStrainerConfigurationProvider CreateDefaultConfigurationProvider(Action<StrainerOptions> options, params Type[] strainerModuleTypes)
    {
        return CreateDefaultConfigurationProvider(options, s => { }, strainerModuleTypes);
    }

    public IStrainerConfigurationProvider CreateDefaultConfigurationProvider<TModule>(Action<StrainerOptions> options, Action<IServiceCollection> services)
        where TModule : class, IStrainerModule
    {
        return CreateDefaultConfigurationProvider(options, services, typeof(TModule));
    }

    public IStrainerConfigurationProvider CreateDefaultConfigurationProvider(
        Action<StrainerOptions> options,
        Action<IServiceCollection> services,
        params Type[] strainerModuleTypes)
    {
        var serviceProvider = BuildStrainerServiceProvider(options, services, strainerModuleTypes);

        return serviceProvider.GetRequiredService<IStrainerConfigurationProvider>();
    }

    public IStrainerContext CreateDefaultContext<TModule>()
        where TModule : class, IStrainerModule
    {
        return CreateDefaultContext(typeof(TModule));
    }

    public IStrainerContext CreateDefaultContext(params Type[] strainerModuleTypes)
    {
        return CreateDefaultContext(o => { }, strainerModuleTypes);
    }

    public IStrainerContext CreateDefaultContext<TModule>(Action<StrainerOptions> options)
        where TModule : class, IStrainerModule
    {
        return CreateDefaultContext(options, typeof(TModule));
    }

    public IStrainerContext CreateDefaultContext(Action<StrainerOptions> options, params Type[] strainerModuleTypes)
    {
        return CreateDefaultContext(options, s => { }, strainerModuleTypes);
    }

    public IStrainerContext CreateDefaultContext<TModule>(Action<StrainerOptions> options, Action<IServiceCollection> services)
        where TModule : class, IStrainerModule
    {
        return CreateDefaultContext(options, services, typeof(TModule));
    }

    public IStrainerContext CreateDefaultContext(
        Action<StrainerOptions> options,
        Action<IServiceCollection> services,
        params Type[] strainerModuleTypes)
    {
        var serviceProvider = BuildStrainerServiceProvider(options, services, strainerModuleTypes);

        return serviceProvider.GetRequiredService<IStrainerContext>();
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
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