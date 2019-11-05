﻿using FluentAssertions;
using Fluorite.Extensions.DependencyInjection;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Filtering.Terms;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Sorting;
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
            var postExtensionStrainerOptions = serviceProvider.GetService<IOptions<StrainerOptions>>()?.Value;

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
            var postExtensionStrainerOptions = serviceProvider.GetService<IOptions<StrainerOptions>>()?.Value;

            // Assert
            postExtensionStrainer
                .Should()
                .NotBeNull("Because extension method should add " +
                        "Strainer to the service collection.");
            postExtensionStrainerOptions
                .Should()
                .NotBeNull();
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

            // Act
            services.AddStrainer<StrainerProcessor>(configuration);
            var serviceProvider = services.BuildServiceProvider();
            var postExtensionStrainer = serviceProvider.GetService<IStrainerProcessor>();
            var postExtensionStrainerOptions = serviceProvider.GetService<IOptions<StrainerOptions>>()?.Value;

            // Assert
            postExtensionStrainer
                .Should()
                .NotBeNull("Because extension method should add " +
                        "Strainer to the service collection.");
            postExtensionStrainerOptions
                .Should()
                .NotBeNull();
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
            services.AddStrainer<StrainerProcessor>(serviceLifetime);
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
            services.AddScoped<IFilterTermParser, TestFilterTermParser>();
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
                .BeAssignableTo<TestFilterTermParser>(
                    "Because DI container should return service that was registered last.");
        }

        [Fact]
        public void CustomSortingWayFormatter_Works_When_AddedToServiceCollection()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddStrainer<StrainerProcessor>();
            services.AddScoped<ISortingWayFormatter, TestSortingWayFormatter>();
            var serviceProvider = services.BuildServiceProvider();

            // Act
            var formatter = serviceProvider.GetRequiredService<ISortingWayFormatter>();

            // Assert
            formatter.Should().BeAssignableTo<TestSortingWayFormatter>();
        }

        private class TestFilterTermParser : IFilterTermParser
        {
            public IList<IFilterTerm> GetParsedTerms(string input)
            {
                throw new NotImplementedException();
            }
        }

        private class TestSortingWayFormatter : ISortingWayFormatter
        {
            public bool IsDescendingDefaultSortingWay => throw new NotImplementedException();

            public string Format(string input, bool isDescending)
            {
                throw new NotImplementedException();
            }

            public bool IsDescending(string input)
            {
                throw new NotImplementedException();
            }

            public string Unformat(string input)
            {
                throw new NotImplementedException();
            }
        }
    }
}
