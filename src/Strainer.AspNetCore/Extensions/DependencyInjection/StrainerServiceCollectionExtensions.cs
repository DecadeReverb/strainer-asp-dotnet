using Fluorite.Strainer.AspNetCore.Services;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Configuration;
using Fluorite.Strainer.Services.Conversion;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Metadata;
using Fluorite.Strainer.Services.Metadata.Attributes;
using Fluorite.Strainer.Services.Metadata.FluentApi;
using Fluorite.Strainer.Services.Modules;
using Fluorite.Strainer.Services.Pagination;
using Fluorite.Strainer.Services.Pipelines;
using Fluorite.Strainer.Services.Sorting;
using Fluorite.Strainer.Services.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace Fluorite.Extensions.DependencyInjection
{
    /// <summary>
    /// Provides extensions for adding Strainer services to <see cref="IServiceCollection"/>.
    /// </summary>
    public static class StrainerServiceCollectionExtensions
    {
        /// <summary>
        /// The default service lifetime for Strainer services.
        /// </summary>
        public const ServiceLifetime DefaultServiceLifetime = ServiceLifetime.Scoped;

        /// <summary>
        /// Adds Strainer services to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">
        /// Current instance of <see cref="IServiceCollection"/>.
        /// </param>
        /// <param name="serviceLifetime">
        /// The service lifetime for Strainer services.
        /// </param>
        /// <returns>
        /// An instance of <see cref="IServiceCollection"/> with added
        /// Strainer services, so additional calls can be chained.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="services"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Another Strainer processor was already registered within the
        /// current <see cref="IServiceCollection"/>.
        /// </exception>
        public static IServiceCollection AddStrainer(
            this IServiceCollection services,
            ServiceLifetime serviceLifetime = DefaultServiceLifetime)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            return services.AddStrainer(new List<Type>(), serviceLifetime);
        }

        /// <summary>
        /// Adds Strainer services to the <see cref="IServiceCollection"/>
        /// with a collection of assemblies containing Strainer module types.
        /// </summary>
        /// <param name="services">
        /// Current instance of <see cref="IServiceCollection"/>.
        /// </param>
        /// <param name="assembliesToScan">
        /// Assemblies that will be scanned in search for non-abstract classes
        /// deriving from <see cref="StrainerModule"/>. Matching classes will
        /// be added to Strainer as configuration modules.
        /// <para />
        /// Referenced assemblies will not be included.
        /// </param>
        /// <param name="serviceLifetime">
        /// The service lifetime for Strainer services.
        /// </param>
        /// <returns>
        /// An instance of <see cref="IServiceCollection"/> with added
        /// Strainer services, so additional calls can be chained.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="services"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="assembliesToScan"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Another Strainer processor was already registered within the
        /// current <see cref="IServiceCollection"/>.
        /// </exception>
        public static IServiceCollection AddStrainer(
            this IServiceCollection services,
            Assembly[] assembliesToScan,
            ServiceLifetime serviceLifetime = DefaultServiceLifetime)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (assembliesToScan is null)
            {
                throw new ArgumentNullException(nameof(assembliesToScan));
            }

            var moduleTypes = GetModuleTypesFromAssemblies(assembliesToScan);

            services.AddSingleton<IMetadataAssemblySourceProvider>(new AssemblySourceProvider(assembliesToScan));

            return services.AddStrainer(moduleTypes, serviceLifetime);
        }

        /// <summary>
        /// Adds Strainer services to the <see cref="IServiceCollection"/>
        /// with a collection of Strainer module types.
        /// </summary>
        /// <param name="services">
        /// Current instance of <see cref="IServiceCollection"/>.
        /// </param>
        /// <param name="moduleTypes">
        /// The types of Strainer modules.
        /// </param>
        /// <param name="serviceLifetime">
        /// The service lifetime for Strainer services.
        /// </param>
        /// <returns>
        /// An instance of <see cref="IServiceCollection"/> with added
        /// Strainer services, so additional calls can be chained.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="services"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="moduleTypes"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Another Strainer processor was already registered within the
        /// current <see cref="IServiceCollection"/>.
        /// </exception>
        public static IServiceCollection AddStrainer(
            this IServiceCollection services,
            IReadOnlyCollection<Type> moduleTypes,
            ServiceLifetime serviceLifetime = DefaultServiceLifetime)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (moduleTypes is null)
            {
                throw new ArgumentNullException(nameof(moduleTypes));
            }

            RegisterStrainerServices(services, moduleTypes, serviceLifetime);

            return services;
        }

        /// <summary>
        /// Adds Strainer services to the <see cref="IServiceCollection"/>
        /// with a configuration.
        /// </summary>
        /// <param name="services">
        /// Current instance of <see cref="IServiceCollection"/>.
        /// </param>
        /// <param name="configuration">
        /// A configuration used to bind against <see cref="StrainerOptions"/>.
        /// </param>
        /// <param name="serviceLifetime">
        /// The service lifetime for Strainer services.
        /// </param>
        /// <returns>
        /// An instance of <see cref="IServiceCollection"/> with added
        /// Strainer services, so additional calls can be chained.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="services"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="configuration"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Another Strainer processor was already registered within the
        /// current <see cref="IServiceCollection"/>.
        /// </exception>
        public static IServiceCollection AddStrainer(
            this IServiceCollection services,
            IConfiguration configuration,
            ServiceLifetime serviceLifetime = DefaultServiceLifetime)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            return services.AddStrainer(configuration, new List<Type>(), serviceLifetime);
        }

        /// <summary>
        /// Adds Strainer services to the <see cref="IServiceCollection"/>
        /// with a configuration and a collection of Strainer module types.
        /// </summary>
        /// <param name="services">
        /// Current instance of <see cref="IServiceCollection"/>.
        /// </param>
        /// <param name="configuration">
        /// A configuration used to bind against <see cref="StrainerOptions"/>.
        /// </param>
        /// <param name="moduleTypes">
        /// The types of Strainer modules.
        /// </param>
        /// <param name="serviceLifetime">
        /// The service lifetime for Strainer services.
        /// </param>
        /// <returns>
        /// An instance of <see cref="IServiceCollection"/> with added
        /// Strainer services, so additional calls can be chained.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="services"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="configuration"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="moduleTypes"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Another Strainer processor was already registered within the
        /// current <see cref="IServiceCollection"/>.
        /// </exception>
        public static IServiceCollection AddStrainer(
            this IServiceCollection services,
            IConfiguration configuration,
            IReadOnlyCollection<Type> moduleTypes,
            ServiceLifetime serviceLifetime = DefaultServiceLifetime)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (moduleTypes is null)
            {
                throw new ArgumentNullException(nameof(moduleTypes));
            }

            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            services.Configure<StrainerOptions>(configuration);

            return services.AddStrainer(moduleTypes, serviceLifetime);
        }

        /// <summary>
        /// Adds Strainer services to the <see cref="IServiceCollection"/>
        /// with a configuration and a collection of assemblies containing
        /// Strainer module types.
        /// </summary>
        /// <param name="services">
        /// Current instance of <see cref="IServiceCollection"/>.
        /// </param>
        /// <param name="configuration">
        /// A configuration used to bind against <see cref="StrainerOptions"/>.
        /// </param>
        /// <param name="assembliesToScan">
        /// Assemblies that will be scanned in search for non-abstract classes
        /// deriving from <see cref="StrainerModule"/>. Matching classes will
        /// be added to Strainer as configuration modules.
        /// <para />
        /// Referenced assemblies will not be included.
        /// </param>
        /// <param name="serviceLifetime">
        /// The service lifetime for Strainer services.
        /// </param>
        /// <returns>
        /// An instance of <see cref="IServiceCollection"/> with added
        /// Strainer services, so additional calls can be chained.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="services"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="configuration"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="assembliesToScan"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Another Strainer processor was already registered within the
        /// current <see cref="IServiceCollection"/>.
        /// </exception>
        public static IServiceCollection AddStrainer(
            this IServiceCollection services,
            IConfiguration configuration,
            Assembly[] assembliesToScan,
            ServiceLifetime serviceLifetime = DefaultServiceLifetime)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (assembliesToScan is null)
            {
                throw new ArgumentNullException(nameof(assembliesToScan));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            services.Configure<StrainerOptions>(configuration);
            services.AddSingleton<IMetadataAssemblySourceProvider>(new AssemblySourceProvider(assembliesToScan));

            return services.AddStrainer(assembliesToScan, serviceLifetime);
        }

        /// <summary>
        /// Adds Strainer services to the <see cref="IServiceCollection"/>
        /// with a configuration action.
        /// </summary>
        /// <param name="services">
        /// Current instance of <see cref="IServiceCollection"/>.
        /// </param>
        /// <param name="configure">
        /// An action used to configure <see cref="StrainerOptions"/>.
        /// </param>
        /// <param name="serviceLifetime">
        /// The service lifetime for Strainer services.
        /// </param>
        /// <returns>
        /// An instance of <see cref="IServiceCollection"/> with added
        /// Strainer services, so additional calls can be chained.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="services"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="configure"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Another Strainer processor was already registered within the
        /// current <see cref="IServiceCollection"/>.
        /// </exception>
        public static IServiceCollection AddStrainer(
            this IServiceCollection services,
            Action<StrainerOptions> configure,
            ServiceLifetime serviceLifetime = DefaultServiceLifetime)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configure is null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            return services.AddStrainer(configure, new List<Type>(), serviceLifetime);
        }

        /// <summary>
        /// Adds Strainer services to the <see cref="IServiceCollection"/>
        /// with a configuration action and a collection of Strainer module types.
        /// </summary>
        /// <param name="services">
        /// Current instance of <see cref="IServiceCollection"/>.
        /// </param>
        /// <param name="configure">
        /// An action used to configure <see cref="StrainerOptions"/>.
        /// </param>
        /// <param name="moduleTypes">
        /// The types of Strainer modules.
        /// </param>
        /// <param name="serviceLifetime">
        /// The service lifetime for Strainer services.
        /// </param>
        /// <returns>
        /// An instance of <see cref="IServiceCollection"/> with added
        /// Strainer services, so additional calls can be chained.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="services"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="configure"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="moduleTypes"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Another Strainer processor was already registered within the
        /// current <see cref="IServiceCollection"/>.
        /// </exception>
        public static IServiceCollection AddStrainer(
            this IServiceCollection services,
            Action<StrainerOptions> configure,
            IReadOnlyCollection<Type> moduleTypes,
            ServiceLifetime serviceLifetime = DefaultServiceLifetime)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (moduleTypes is null)
            {
                throw new ArgumentNullException(nameof(moduleTypes));
            }

            if (configure is null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            services.AddOptions<StrainerOptions>().Configure(configure);

            return services.AddStrainer(moduleTypes, serviceLifetime);
        }

        /// <summary>
        /// Adds Strainer services to the <see cref="IServiceCollection"/>
        /// with a configuration action and a collection of assemblies containing
        /// Strainer module types.
        /// </summary>
        /// <param name="services">
        /// Current instance of <see cref="IServiceCollection"/>.
        /// </param>
        /// <param name="configure">
        /// An action used to configure <see cref="StrainerOptions"/>.
        /// </param>
        /// <param name="assembliesToScan">
        /// Assemblies that will be scanned in search for non-abstract classes
        /// deriving from <see cref="StrainerModule"/>. Matching classes will
        /// be added to Strainer as configuration modules.
        /// <para />
        /// Referenced assemblies will not be included.
        /// </param>
        /// <param name="serviceLifetime">
        /// The service lifetime for Strainer services.
        /// </param>
        /// <returns>
        /// An instance of <see cref="IServiceCollection"/> with added
        /// Strainer services, so additional calls can be chained.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="services"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="configure"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="assembliesToScan"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Another Strainer processor was already registered within the
        /// current <see cref="IServiceCollection"/>.
        /// </exception>
        public static IServiceCollection AddStrainer(
            this IServiceCollection services,
            Action<StrainerOptions> configure,
            Assembly[] assembliesToScan,
            ServiceLifetime serviceLifetime = DefaultServiceLifetime)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (assembliesToScan is null)
            {
                throw new ArgumentNullException(nameof(assembliesToScan));
            }

            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            services.AddOptions<StrainerOptions>().Configure(configure);
            services.AddSingleton<IMetadataAssemblySourceProvider>(new AssemblySourceProvider(assembliesToScan));

            return services.AddStrainer(assembliesToScan, serviceLifetime);
        }

        private static void RegisterStrainerServices(
            IServiceCollection services,
            IReadOnlyCollection<Type> moduleTypes,
            ServiceLifetime serviceLifetime)
        {
            if (services.Any(d => d.ServiceType == typeof(IStrainerProcessor)))
            {
                throw new InvalidOperationException(
                    "Unable to registrer Strainer services because they have been registered already.");
            }

            services.AddOptions<StrainerOptions>();

            if (serviceLifetime == ServiceLifetime.Singleton)
            {
                services.Add<IStrainerOptionsProvider, AspNetCoreSingletonStrainerOptionsProvider>(serviceLifetime);
            }
            else
            {
                services.Add<IStrainerOptionsProvider, AspNetCoreStrainerOptionsProvider>(serviceLifetime);
            }

            services.Add<IFilterExpressionProvider, FilterExpressionProvider>(serviceLifetime);
            services.Add<IFilterOperatorMapper, FilterOperatorMapper>(serviceLifetime);
            services.Add<IFilterOperatorParser, FilterOperatorParser>(serviceLifetime);
            services.Add<IFilterOperatorValidator, FilterOperatorValidator>(serviceLifetime);
            services.Add<IFilterTermNamesParser, FilterTermNamesParser>(serviceLifetime);
            services.Add<IFilterTermValuesParser, FilterTermValuesParser>(serviceLifetime);
            services.Add<IFilterTermSectionsParser, FilterTermSectionsParser>(serviceLifetime);
            services.Add<IFilterTermParser, FilterTermParser>(serviceLifetime);
            services.Add<ICustomFilteringExpressionProvider, CustomFilteringExpressionProvider>(serviceLifetime);
            services.Add<ICustomFilterMethodMapper, CustomFilterMethodMapper>(serviceLifetime);
            services.Add<IFilterContext, FilterContext>(serviceLifetime);

            services.Add<ISortExpressionProvider, SortExpressionProvider>(serviceLifetime);
            services.Add<ISortExpressionValidator, SortExpressionValidator>(serviceLifetime);
            services.Add<ISortingWayFormatter, DescendingPrefixSortingWayFormatter>(serviceLifetime);
            services.Add<ISortTermValueParser, SortTermValueParser>(serviceLifetime);
            services.Add<ISortTermParser, SortTermParser>(serviceLifetime);
            services.Add<ISortingApplier, SortingApplier>(serviceLifetime);
            services.Add<ICustomSortingExpressionProvider, CustomSortingExpressionProvider>(serviceLifetime);
            services.Add<ICustomSortMethodMapper, CustomSortMethodMapper>(serviceLifetime);
            services.Add<ISortingContext, SortingContext>(serviceLifetime);
            services.Add<IPipelineContext, PipelineContext>(serviceLifetime);

            services.Add<IPageNumberEvaluator, PageNumberEvaluator>(serviceLifetime);
            services.Add<IPageSizeEvaluator, PageSizeEvaluator>(serviceLifetime);

            services.Add<IStrainerPipelineBuilderFactory, StrainerPipelineBuilderFactory>(serviceLifetime);
            services.Add<IFilterPipelineOperation, FilterPipelineOperation>(serviceLifetime);
            services.Add<ISortPipelineOperation, SortPipelineOperation>(serviceLifetime);
            services.Add<IPaginatePipelineOperation, PaginatePipelineOperation>(serviceLifetime);

            services.TryAddSingleton<IMetadataAssemblySourceProvider, AppDomainAssemblySourceProvider>();
            services.Add<IMetadataSourceTypeProvider, MetadataSourceTypeProvider>(serviceLifetime);
            services.Add<ITypeConverter, ComponentModelTypeConverter>(serviceLifetime);
            services.Add<ITypeConverterProvider, TypeConverterProvider>(serviceLifetime);
            services.Add<IAttributePropertyMetadataBuilder, AttributePropertyMetadataBuilder>(serviceLifetime);
            services.Add<IAttributeMetadataRetriever, AttributeMetadataRetriever>(serviceLifetime);
            services.Add<IStrainerAttributeProvider, StrainerAttributeProvider>(serviceLifetime);
            services.Add<IPropertyMetadataDictionaryProvider, PropertyMetadataDictionaryProvider>(serviceLifetime);
            services.Add<IPropertyInfoProvider, PropertyInfoProvider>(serviceLifetime);
            services.Add<IMetadataSourceChecker, MetadataSourceChecker>(serviceLifetime);
            services.Add<IMetadataProvider, FluentApiMetadataProvider>(serviceLifetime);
            services.Add<IMetadataProvider, AttributeMetadataProvider>(serviceLifetime);
            services.Add<IMetadataFacade, MetadataFacade>(serviceLifetime);

            services.Add<IStrainerConfigurationFactory, StrainerConfigurationFactory>(serviceLifetime);
            services.Add<IStrainerModuleFactory, StrainerModuleFactory>(serviceLifetime);
            services.Add<IStrainerModuleLoader, StrainerModuleLoader>(serviceLifetime);
            services.Add<IGenericModuleLoader, GenericModuleLoader>(serviceLifetime);
            services.Add<IStrainerModuleTypeValidator, StrainerModuleTypeValidator>(serviceLifetime);
            services.Add<IStrainerConfigurationValidator, StrainerConfigurationValidator>(serviceLifetime);
            services.Add<IConfigurationCustomMethodsProvider, ConfigurationCustomMethodsProvider>(serviceLifetime);
            services.Add<IConfigurationFilterOperatorsProvider, ConfigurationFilterOperatorsProvider>(serviceLifetime);
            services.Add<IConfigurationMetadataProvider, ConfigurationMetadataProvider>(serviceLifetime);

            services.Add<IMetadataMapper, MetadataMapper>(serviceLifetime);
            services.Add<IStrainerContext, StrainerContext>(serviceLifetime);
            services.Add<IStrainerProcessor, StrainerProcessor>(serviceLifetime);

            services.AddSingleton<IStrainerConfigurationProvider, StrainerConfigurationProvider>(serviceProvider =>
            {
                using var scope = serviceProvider.CreateScope();
                var configurationFactory = scope.ServiceProvider.GetRequiredService<IStrainerConfigurationFactory>();
                var configurationValidator = scope.ServiceProvider.GetRequiredService<IStrainerConfigurationValidator>();

                try
                {
                    var strainerConfiguration = configurationFactory.Create(moduleTypes);
                    configurationValidator.Validate(strainerConfiguration);

                    return new StrainerConfigurationProvider(strainerConfiguration);
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Unable to add configuration from Strainer modules.", ex);
                }
            });
        }

        private static void Add<TServiceType, TImplementationType>(
            this IServiceCollection services,
            ServiceLifetime serviceLifetime)
            where TImplementationType : TServiceType
        {
            services.Add(new ServiceDescriptor(typeof(TServiceType), typeof(TImplementationType), serviceLifetime));
        }

        private static List<Type> GetModuleTypesFromAssemblies(IReadOnlyCollection<Assembly> assemblies)
        {
            return assemblies
                .Distinct()
                .SelectMany(a => a.GetTypes())
                .SelectMany(type => new[] { type }.Union(type.GetNestedTypes()))
                .Where(type => !type.IsAbstract && typeof(IStrainerModule).IsAssignableFrom(type))
                .ToList();
        }
    }
}
