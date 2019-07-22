using FluentAssertions;
using Fluorite.Extensions.DependencyInjection;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Filtering.Terms;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Filtering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Fluorite.Strainer.UnitTests.Extensions.DepedencyInjection
{
    public class StrainerServiceCollectionExtensionsTests
    {
        [Fact]
        public void ExtensionMethod_AddsStrainer()
        {
            // Arrange
            var services = new ServiceCollection();
            var serviceProvider = services.BuildServiceProvider();
            var preExtensionStrainer = serviceProvider.GetService<IStrainerProcessor>();

            // Act
            services.AddStrainer<StrainerProcessor>();
            serviceProvider = services.BuildServiceProvider();
            var postExtensionStrainer = serviceProvider.GetService<IStrainerProcessor>();
            var postExtensionStrainerOptions = serviceProvider.GetService<StrainerOptions>();

            // Assert
            preExtensionStrainer
                .Should()
                .BeNull("Because Strainer has not been registered yet.");
            postExtensionStrainer
                .Should()
                .NotBeNull("Because extension method should add " +
                        "Strainer to the service collection.");
            postExtensionStrainerOptions
                .Should()
                .NotBeNull();
        }

        [Fact]
        public void ExtensionMethod_AddsStrainer_WithCustomOptions_ViaAction()
        {
            // Arrange
            var defaultPageSize = 20;
            var services = new ServiceCollection();
            var serviceProvider = services.BuildServiceProvider();

            // Act
            services.AddStrainer<StrainerProcessor>(options => options.DefaultPageSize = defaultPageSize);
            serviceProvider = services.BuildServiceProvider();
            var postExtensionStrainer = serviceProvider.GetService<IStrainerProcessor>();
            var postExtensionStrainerOptions = serviceProvider.GetService<StrainerOptions>();

            // Assert
            postExtensionStrainer
                .Should()
                .NotBeNull("Because extension method should add " +
                        "Strainer to the service collection.");
            postExtensionStrainerOptions
                .DefaultPageSize
                .Should()
                .Be(defaultPageSize);
        }

        [Fact]
        public void ExtensionMethod_AddsStrainer_WithCustomOptions_ViaConfiguration()
        {
            // Arrange
            var defaultPageSize = 20;
            var services = new ServiceCollection();
            var strainerOptions = new Dictionary<string, string>
            {
                { nameof(StrainerOptions.DefaultPageSize) , defaultPageSize.ToString() },
            };
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(strainerOptions)
                .Build();
            var serviceProvider = services.BuildServiceProvider();

            // Act
            services.AddStrainer<StrainerProcessor>(configuration);
            serviceProvider = services.BuildServiceProvider();
            var postExtensionStrainer = serviceProvider.GetService<IStrainerProcessor>();
            var postExtensionStrainerOptions = serviceProvider.GetService<StrainerOptions>();

            // Assert
            postExtensionStrainer
                .Should()
                .NotBeNull("Because extension method should add " +
                        "Strainer to the service collection.");
            postExtensionStrainerOptions
                .DefaultPageSize
                .Should()
                .Be(defaultPageSize);
        }

        [Fact]
        public void ExtensionMethod_AddsStrainer_WithCustomServiceLifetime()
        {
            // Arrange
            var serviceLifetime = ServiceLifetime.Singleton;
            var services = new ServiceCollection();
            var serviceProvider = services.BuildServiceProvider();

            // Act
            services.AddStrainer<StrainerProcessor>(options => options.ServiceLifetime = serviceLifetime);
            serviceProvider = services.BuildServiceProvider();
            var postExtensionStrainer = serviceProvider.GetService<IStrainerProcessor>();
            var strainerProcessorServiceDescriptor = services.FirstOrDefault(s => s.ServiceType == typeof(IStrainerProcessor));

            // Assert
            postExtensionStrainer
                .Should()
                .NotBeNull("Because extension method should add " +
                        "Strainer to the service collection.");
            strainerProcessorServiceDescriptor
                .Lifetime
                .Should()
                .Be(serviceLifetime);
        }

        [Fact]
        public void ExtensionMethod_AddsStrainer_WithCustomService_FilterTermParser()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddStrainer<StrainerProcessor>();
            services.AddScoped<IFilterTermParser, _TestFilterTermParser>();
            var serviceProvider = services.BuildServiceProvider();
            var postExtensionStrainer = serviceProvider.GetService<IStrainerProcessor>();
            var postExtensionFilterTermParser = serviceProvider.GetService<IFilterTermParser>();

            // Assert
            postExtensionStrainer
                .Should()
                .NotBeNull("Because extension method should add " +
                        "Strainer to the service collection.");
            postExtensionFilterTermParser
                .Should()
                .BeAssignableTo<_TestFilterTermParser>(
                    "Because DI container should return service that was registered last.");
        }
    }

    internal class _TestFilterTermParser : IFilterTermParser
    {
        public IList<IFilterTerm> GetParsedTerms(string input)
        {
            throw new NotImplementedException();
        }
    }
}
