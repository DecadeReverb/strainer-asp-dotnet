using Fluorite.Extensions.DependencyInjection;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Configuration;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Metadata;
using Fluorite.Strainer.Services.Metadata.FluentApi;
using Fluorite.Strainer.Services.Modules;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace Fluorite.Strainer.IntegrationTests.DependencyInjection;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void ModuleConfiguration_Works_For_Adding_Property()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddStrainer([typeof(PropertyTestModule)]);
        using var serviceProvider = services.BuildServiceProvider();
        var metadataProviders = serviceProvider.GetRequiredService<IEnumerable<IMetadataProvider>>();
        var fluentApiMetadataProvider = metadataProviders
            .OfType<FluentApiMetadataProvider>()
            .Single();

        // Act
        var metadatas = fluentApiMetadataProvider.GetPropertyMetadatas(typeof(Post));

        // Assert
        metadatas.Should().NotBeNullOrEmpty();
        metadatas.Should().HaveSameCount(typeof(Post).GetProperties());
        metadatas[0].Name.Should().Be(nameof(Post.Id));
    }

    [Fact]
    public void ModuleConfiguration_Works_For_Adding_Object()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddStrainer([typeof(PropertyTestModule)]);
        using var serviceProvider = services.BuildServiceProvider();
        var metadataProviders = serviceProvider.GetRequiredService<IEnumerable<IMetadataProvider>>();
        var fluentApiMetadataProvider = metadataProviders
            .OfType<FluentApiMetadataProvider>()
            .Single();

        // Act
        var metadata = fluentApiMetadataProvider.GetDefaultMetadata(typeof(Comment));

        // Assert
        metadata.Should().NotBeNull();
        metadata.Name.Should().Be(nameof(Comment.Id));
    }

    [Fact]
    public void ModuleConfiguration_Works_For_Adding_Filter_Operator()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddStrainer([typeof(PropertyTestModule)]);
        using var serviceProvider = services.BuildServiceProvider();
        var filterOperatorsProvider = serviceProvider.GetRequiredService<IConfigurationFilterOperatorsProvider>();

        // Act
        var result = filterOperatorsProvider.GetFilterOperators();

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().HaveCountGreaterThan(FilterOperatorMapper.DefaultOperators.Count);
    }

    [Fact]
    public void ModuleConfiguration_Works_For_Adding_Custom_Filter_Method()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddStrainer([typeof(PropertyTestModule)]);
        using var serviceProvider = services.BuildServiceProvider();
        var customMethodsProvider = serviceProvider.GetRequiredService<IConfigurationCustomMethodsProvider>();

        // Act
        var customFilterMethods = customMethodsProvider.GetCustomFilterMethods();

        // Assert
        customFilterMethods.Should().NotBeNullOrEmpty();
        customFilterMethods.Should().ContainSingle(m => m.Key == typeof(Post));
    }

    [Fact]
    public void ModuleConfiguration_Works_For_Adding_Custom_Sort_Method()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddStrainer([typeof(PropertyTestModule)]);
        using var serviceProvider = services.BuildServiceProvider();
        var customMethodsProvider = serviceProvider.GetRequiredService<IConfigurationCustomMethodsProvider>();

        // Act
        var customSortMethods = customMethodsProvider.GetCustomSortMethods();

        // Assert
        customSortMethods.Should().NotBeNullOrEmpty();
        customSortMethods.Should().ContainSingle(m => m.Key == typeof(Post));
    }

    [Fact]
    public void EveryStrainerService_IsResolvable()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddStrainer();
        using var serviceProvider = services.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();

        // Assert
        var exceptions = new List<InvalidOperationException>();
        var strainerServices = services
            .Where(x => x.ServiceType.Assembly.Equals(typeof(IStrainerProcessor).Assembly))
            .ToList();

        foreach (var serviceDescriptor in strainerServices)
        {
            var serviceType = serviceDescriptor.ServiceType;
            try
            {
                scope.ServiceProvider.GetRequiredService(serviceType);
            }
            catch (InvalidOperationException e)
            {
                exceptions.Add(e);
            }
        }

        exceptions.Should().BeEmpty();
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
        public override void Load(IStrainerModuleBuilder builder)
        {
            builder.AddProperty<Post>(post => post.Id)
                .IsFilterable()
                .IsSortable()
                .IsDefaultSort();

            builder.AddObject<Comment>(comment => comment.Id)
                .IsFilterable()
                .IsSortable();

            builder.AddFilterOperator(b => b
                .HasSymbol("###")
                .HasName("hash")
                .HasExpression(context => Expression.Constant(true))
                .Build());

            builder.AddCustomFilterMethod<Post>(b => b
                .HasName("TestCustomFilterMethod")
                .HasFunction(post => post.Id == 1)
                .Build());

            builder.AddCustomSortMethod<Post>(b => b
                .HasName("TestCustomSortMethod")
                .HasFunction(post => post.Id)
                .Build());
        }
    }
}
