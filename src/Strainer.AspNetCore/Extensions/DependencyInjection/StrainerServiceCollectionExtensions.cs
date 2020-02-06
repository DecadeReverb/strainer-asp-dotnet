﻿using Fluorite.Strainer.AspNetCore.Services;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Filtering;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Models.Sorting;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Metadata;
using Fluorite.Strainer.Services.Modules;
using Fluorite.Strainer.Services.Sorting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public static IStrainerBuilder AddStrainer(
            this IServiceCollection services,
            ServiceLifetime serviceLifetime = DefaultServiceLifetime)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            return services.AddStrainer(Enumerable.Empty<Type>(), serviceLifetime);
        }

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
        public static IStrainerBuilder AddStrainer(
            this IServiceCollection services,
            IEnumerable<Type> moduleTypes,
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

            if (services.Any(d => d.ServiceType == typeof(IStrainerProcessor)))
            {
                throw new InvalidOperationException(
                    $"Unable to registrer {nameof(IStrainerProcessor)} " +
                    $"because there is already registered one.");
            }

            // Add Strainer options only if they weren't configured yet.
            if (!services.ContainsServiceOfType<StrainerOptions>())
            {
                services.AddOptions<StrainerOptions>();
            }

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
            services.Add<IFilterTermParser, FilterTermParser>(serviceLifetime);
            services.Add<IFilterContext, FilterContext>(serviceLifetime);

            services.Add<ISortExpressionProvider, SortExpressionProvider>(serviceLifetime);
            services.Add<ISortExpressionValidator, SortExpressionValidator>(serviceLifetime);
            services.Add<ISortingWayFormatter, DescendingPrefixSortingWayFormatter>(serviceLifetime);
            services.Add<ISortTermParser, SortTermParser>(serviceLifetime);
            services.Add<ISortingContext, SortingContext>(serviceLifetime);

            services.Add<ICustomFilterMethodMapper, CustomFilterMethodMapper>(serviceLifetime);
            services.Add<ICustomSortMethodMapper, CustomSortMethodMapper>(serviceLifetime);
            services.Add<ICustomMethodsContext, CustomMethodsContext>(serviceLifetime);

            services.Add<IMetadataProvider, FluentApiMetadataProvider>(serviceLifetime);
            services.Add<IMetadataProvider, AttributeMetadataProvider>(serviceLifetime);
            services.Add<IMetadataFacade, MetadataFacade>(serviceLifetime);

            services.Add<IMetadataMapper, MetadataMapper>(serviceLifetime);
            services.Add<IStrainerContext, StrainerContext>(serviceLifetime);
            services.Add<IStrainerProcessor, StrainerProcessor>(serviceLifetime);

            try
            {
                AddModulesConfiguration(services, moduleTypes);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Unable to add configuration from modules. " + ex.Message, ex);
            }

            return new StrainerBuilder(services, serviceLifetime);
        }

        public static IStrainerBuilder AddStrainer(
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

            return services.AddStrainer(configuration, Enumerable.Empty<Assembly>(), serviceLifetime);
        }

        public static IStrainerBuilder AddStrainer(
            this IServiceCollection services,
            IConfiguration configuration,
            IEnumerable<Type> moduleTypes,
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

            var assemblies = moduleTypes
                .Distinct()
                .Select(type => type.Assembly);

            return services.AddStrainer(configuration, assemblies, serviceLifetime);
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
        public static IStrainerBuilder AddStrainer(
            this IServiceCollection services,
            IConfiguration configuration,
            IEnumerable<Assembly> moduleAssemblies,
            ServiceLifetime serviceLifetime = DefaultServiceLifetime)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (moduleAssemblies is null)
            {
                throw new ArgumentNullException(nameof(moduleAssemblies));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            services.Configure<StrainerOptions>(configuration);

            var moduleTypes = GetTypesFromAssemblies(moduleAssemblies);
            var builder = services.AddStrainer(moduleTypes, serviceLifetime);

            return builder;
        }

        public static IStrainerBuilder AddStrainer(
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

            return services.AddStrainer(configure, Enumerable.Empty<Assembly>(), serviceLifetime);
        }

        public static IStrainerBuilder AddStrainer(
            this IServiceCollection services,
            Action<StrainerOptions> configure,
            IEnumerable<Type> moduleTypes,
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

            var assemblies = moduleTypes
                .Distinct()
                .Select(type => type.Assembly);

            return services.AddStrainer(configure, assemblies, serviceLifetime);
        }

        /// <summary>
        /// Adds Strainer services to the <see cref="IServiceCollection"/>.
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
        public static IStrainerBuilder AddStrainer(
            this IServiceCollection services,
            Action<StrainerOptions> configure,
            IEnumerable<Assembly> moduleAssemblies,
            ServiceLifetime serviceLifetime = DefaultServiceLifetime)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (moduleAssemblies is null)
            {
                throw new ArgumentNullException(nameof(moduleAssemblies));
            }

            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            services.AddOptions<StrainerOptions>().Configure(configure);

            var assemblyTypes = GetTypesFromAssemblies(moduleAssemblies);
            var builder = services.AddStrainer(assemblyTypes, serviceLifetime);

            return builder;
        }

        private static object GetModuleTypes()
        {
            throw new NotImplementedException();
        }

        private static void Add<TServiceType, TImplementationType>(
            this IServiceCollection services,
            ServiceLifetime serviceLifetime)
        {
            services.Add(new ServiceDescriptor(typeof(TServiceType), typeof(TImplementationType), serviceLifetime));
        }

        private static bool ContainsServiceOfType<TImplementationType>(
            this IServiceCollection services)
        {
            return services.Any(d => d.ServiceType == typeof(TImplementationType));
        }

        private static IEnumerable<Type> GetTypesFromAssemblies(IEnumerable<Assembly> assemblies)
        {
            return assemblies
                .Distinct()
                .Where(a => a.GetReferencedAssemblies()
                    .All(name => !name.FullName.StartsWith("Microsoft.IntelliTrace.Core")))
                .SelectMany(a => a.GetTypes())
                .SelectMany(type => new[] { type }.Union(type.GetNestedTypes()));
        }

        private static void AddModulesConfiguration(
            IServiceCollection services,
            IEnumerable<Type> moduleTypes)
        {
            using (var serviceProvider = services.BuildServiceProvider())
            {
                var optionsProvider = serviceProvider.GetRequiredService<IStrainerOptionsProvider>();
                var options = optionsProvider.GetStrainerOptions();

                var validModuleTypes = moduleTypes
                    .Where(type => !type.IsAbstract && type.IsSubclassOf(typeof(StrainerModule)));

                var invalidModuleTypes = moduleTypes.Except(validModuleTypes);

                if (invalidModuleTypes.Any())
                {
                    throw new InvalidOperationException(
                        string.Format(
                                "Valid Strainer module cannot be an abstract class and must be deriving from {0}. " +
                                "Invalid types:\n{1}",
                            typeof(StrainerModule).FullName,
                            string.Join("\n", invalidModuleTypes.Select(invalidType => invalidType.FullName))));
                }

                var modules = validModuleTypes
                    .Select(type => Activator.CreateInstance(type) as StrainerModule)
                    .Where(instance => instance != null)
                    .ToList();

                modules.ForEach(strainerModule =>
                {
                    var moduleBuilder = new StrainerModuleBuilder(strainerModule, options);

                    strainerModule.Load(moduleBuilder);
                });

                var customFilerMethods = modules
                    .SelectMany(module => module
                        .CustomFilterMethods
                        .Select(pair =>
                            new KeyValuePair<Type, IReadOnlyDictionary<string, ICustomFilterMethod>>(
                                pair.Key, pair.Value.ToReadOnly())))
                    .Merge()
                    .ToReadOnly();
                var customSortMethods = modules
                     .SelectMany(module => module
                        .CustomSortMethods
                        .Select(pair =>
                            new KeyValuePair<Type, IReadOnlyDictionary<string, ICustomSortMethod>>(
                                pair.Key, pair.Value.ToReadOnly())))
                    .Merge()
                    .ToReadOnly();
                var defaultMetadata = modules
                    .SelectMany(module => module.DefaultMetadata)
                    .Merge()
                    .ToReadOnly();
                var filterOperators = modules
                    .SelectMany(module => module.FilterOperators)
                    .Union(FilterOperatorMapper.DefaultOperators)
                    .Merge()
                    .ToReadOnly();
                var objectMetadata = modules
                    .SelectMany(module => module.ObjectMetadata.ToReadOnly())
                    .Merge()
                    .ToReadOnly();
                var propertyMetadata = modules
                     .SelectMany(module => module
                        .PropertyMetadata
                        .Select(pair =>
                            new KeyValuePair<Type, IReadOnlyDictionary<string, IPropertyMetadata>>(
                                pair.Key, pair.Value.ToReadOnly())))
                    .Merge()
                    .ToReadOnly();

                services.AddSingleton(customFilerMethods);
                services.AddSingleton(customSortMethods);
                services.AddSingleton(defaultMetadata);
                services.AddSingleton(filterOperators);
                services.AddSingleton(objectMetadata);
                services.AddSingleton(propertyMetadata);
            }
        }
    }
}
