using FluentAssertions;
using Fluorite.Extensions.DependencyInjection;
using Fluorite.Strainer.Models.Filtering;
using Fluorite.Strainer.Models.Filtering.Operators;
using Fluorite.Strainer.Models.Sorting;
using Fluorite.Strainer.Services.Metadata;
using Fluorite.Strainer.Services.Modules;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace Fluorite.Strainer.IntegrationTests.Services.Metadata
{
    public class ModulesConfigurationTests
    {
        [Fact]
        public void ModuleConfiguration_Works_For_Adding_Property()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddStrainer(new[] { typeof(PropertyTestModule) });
            var serviceProvider = services.BuildServiceProvider();
            var metadataProviders = serviceProvider.GetRequiredService<IEnumerable<IMetadataProvider>>();
            var fluentApiMetadataProvider = metadataProviders
                .OfType<FluentApiMetadataProvider>()
                .Single();

            // Act
            var metadatas = fluentApiMetadataProvider.GetPropertyMetadatas<Post>();

            // Assert
            metadatas.Should().NotBeNullOrEmpty();
            metadatas.Should().HaveSameCount(typeof(Post).GetProperties());
            metadatas.First().Name.Should().Be(nameof(Post.Id));
        }

        [Fact]
        public void ModuleConfiguration_Works_For_Adding_Object()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddStrainer(new[] { typeof(PropertyTestModule) });
            var serviceProvider = services.BuildServiceProvider();
            var metadataProviders = serviceProvider.GetRequiredService<IEnumerable<IMetadataProvider>>();
            var fluentApiMetadataProvider = metadataProviders
                .OfType<FluentApiMetadataProvider>()
                .Single();

            // Act
            var metadata = fluentApiMetadataProvider.GetDefaultMetadata<Comment>();

            // Assert
            metadata.Should().NotBeNull();
            metadata.Name.Should().Be(nameof(Comment.Id));
        }

        [Fact]
        public void ModuleConfiguration_Works_For_Adding_Filter_Operator()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddStrainer(new[] { typeof(PropertyTestModule) });
            var operatorSymbol = "###";
            var serviceProvider = services.BuildServiceProvider();
            var filterOperators = serviceProvider.GetRequiredService<IReadOnlyDictionary<string, IFilterOperator>>();

            // Act
            var result = filterOperators.TryGetValue(operatorSymbol, out var filterOperator);

            // Assert
            result.Should().BeTrue();
            filterOperator.Should().NotBeNull();
            filterOperator.Symbol.Should().Be(operatorSymbol);
        }

        [Fact]
        public void ModuleConfiguration_Works_For_Adding_Custom_Filter_Method()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddStrainer(new[] { typeof(PropertyTestModule) });
            var customMethodName = "TestCustomFilterMethod";
            var serviceProvider = services.BuildServiceProvider();
            var customFilterMethods = serviceProvider
                .GetRequiredService<IReadOnlyDictionary<Type, IReadOnlyDictionary<string, ICustomFilterMethod>>>();

            // Act
            var result = customFilterMethods.TryGetValue(typeof(Post), out var customFilterMethodsForPost);
            var result2 = customFilterMethodsForPost.TryGetValue(customMethodName, out var customFilterMethod);

            // Assert
            result.Should().BeTrue();
            result2.Should().BeTrue();
            customFilterMethodsForPost.Should().NotBeNullOrEmpty();
            customFilterMethod.Should().NotBeNull();
            customFilterMethod.Name.Should().Be(customMethodName);
        }

        [Fact]
        public void ModuleConfiguration_Works_For_Adding_Custom_Sort_Method()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddStrainer(new[] { typeof(PropertyTestModule) });
            var customMethodName = "TestCustomSortMethod";
            var serviceProvider = services.BuildServiceProvider();
            var customSortMethods = serviceProvider
                .GetRequiredService<IReadOnlyDictionary<Type, IReadOnlyDictionary<string, ICustomSortMethod>>>();

            // Act
            var result = customSortMethods.TryGetValue(typeof(Post), out var customSortMethodsForPost);
            var result2 = customSortMethodsForPost.TryGetValue(customMethodName, out var customSortMethod);

            // Assert
            result.Should().BeTrue();
            result2.Should().BeTrue();
            customSortMethodsForPost.Should().NotBeNullOrEmpty();
            customSortMethod.Should().NotBeNull();
            customSortMethod.Name.Should().Be(customMethodName);
        }

        private class Post
        {
            public int Id { get; set; }
        }

        private class Comment
        {
            public int Id { get; set; }
        }

        private class PropertyTestModule : StrainerModule
        {
            public override void Load()
            {
                AddProperty<Post>(post => post.Id)
                    .IsFilterable()
                    .IsSortable()
                    .IsDefaultSort();

                AddObject<Comment>(comment => comment.Id)
                    .IsFilterable()
                    .IsSortable();

                AddFilterOperator(symbol: "###")
                    .HasName("hash")
                    .HasExpression(context => Expression.Constant(true));

                AddCustomFilterMethod<Post>("TestCustomFilterMethod")
                    .HasExpression(context => context.Source.Where(post => post.Id == 1));

                AddCustomSortMethod<Post>("TestCustomSortMethod")
                    .HasExpression(context => context.Source.OrderBy(post => post.Id));
            }
        }
    }
}
