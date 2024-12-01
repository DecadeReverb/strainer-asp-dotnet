using Fluorite.Strainer.Attributes;
using Fluorite.Strainer.Services.Metadata.Attributes;
using System.Reflection;

namespace Fluorite.Strainer.UnitTests.Services.Metadata.Attributes;

public class AttributePropertyMetadataBuilderTests
{
    private readonly AttributePropertyMetadataBuilder _builder;

    public AttributePropertyMetadataBuilderTests()
    {
        _builder = new();
    }

    [Fact]
    public void Should_Return_DefaultPropertyMetadata()
    {
        // Arrange
        var attribute = new StrainerObjectAttribute("Id");
        var propertyName = "foo";
        var propertyInfo = Substitute.For<PropertyInfo>();

        propertyInfo.Name.Returns(propertyName);

        // Act
        var result = _builder.BuildDefaultPropertyMetadata(attribute, propertyInfo);

        // Assert
        result.Should().NotBeNull();
        result.DisplayName.Should().BeNull();
        result.IsDefaultSorting.Should().BeTrue();
        result.IsDefaultSortingDescending.Should().Be(attribute.IsDefaultSortingDescending);
        result.IsFilterable.Should().Be(attribute.IsFilterable);
        result.IsSortable.Should().Be(attribute.IsSortable);
        result.Name.Should().Be(propertyName);
        result.PropertyInfo.Should().BeSameAs(propertyInfo);
    }
}
