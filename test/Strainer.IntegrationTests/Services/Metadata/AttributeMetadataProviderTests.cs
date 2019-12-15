using FluentAssertions;
using Fluorite.Extensions.DependencyInjection;
using Fluorite.Strainer.Attributes;
using Fluorite.Strainer.Services.Metadata;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Xunit;

namespace Fluorite.Strainer.IntegrationTests.Services.Metadata
{
    public class AttributeMetadataProviderTests : StrainerFixtureBase
    {
        public AttributeMetadataProviderTests(StrainerFactory factory) : base(factory)
        {

        }

        [Fact]
        public void AttributeMetadataProvider_Works_For_Object_Attribute()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddStrainer<ApplicationStrainerProcessor>();
            services.AddScoped<AttributeMetadataProvider>();
            var serviceProvider = services.BuildServiceProvider();

            // Act
            var attributeMetadataProvider = serviceProvider.GetRequiredService<AttributeMetadataProvider>();
            var metadatas = attributeMetadataProvider.GetPropertyMetadatas<ObjectAttributeTestClass>();

            // Assert
            metadatas.Should().NotBeNullOrEmpty();
            metadatas.Should().HaveCount(1);
            metadatas.First().Name.Should().Be(nameof(ObjectAttributeTestClass.Id));
        }

        [Fact]
        public void AttributeMetadataProvider_Works_For_Property_Attribute()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddStrainer<ApplicationStrainerProcessor>();
            services.AddScoped<AttributeMetadataProvider>();
            var serviceProvider = services.BuildServiceProvider();

            // Act
            var attributeMetadataProvider = serviceProvider.GetRequiredService<AttributeMetadataProvider>();
            var metadatas = attributeMetadataProvider.GetPropertyMetadatas<PropertyAttributeTestClass>();

            // Assert
            metadatas.Should().NotBeNullOrEmpty();
            metadatas.Should().HaveCount(1);
            metadatas.First().Should().BeAssignableTo<StrainerPropertyAttribute>();
            metadatas.First().Name.Should().Be(nameof(PropertyAttributeTestClass.Id));
        }

        [StrainerObject(nameof(Id))]
        private class ObjectAttributeTestClass
        {
            public int Id { get; set; }
        }

        private class PropertyAttributeTestClass
        {
            [StrainerProperty(IsDefaultSorting = true)]
            public int Id { get; set; }
        }
    }
}
