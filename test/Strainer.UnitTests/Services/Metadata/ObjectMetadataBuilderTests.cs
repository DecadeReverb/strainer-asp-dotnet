using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Services.Metadata;
using System.Linq.Expressions;

namespace Fluorite.Strainer.UnitTests.Services.Metadata;

public class ObjectMetadataBuilderTests
{
    [Fact]
    public void Should_Save_ObjectMetadata_WhenCreatingBuilder()
    {
        // Arrange
        var objectMetadata = new Dictionary<Type, IObjectMetadata>();
        var defaultSortingPropertyName = nameof(Blog.Title);
        var defaultSortingPropertyInfo = typeof(Blog).GetProperty(defaultSortingPropertyName);
        Expression<Func<Blog, object>> defaultSortingPropertyExpression = b => b.Title;
        var propertyInfoProviderMock = Substitute.For<IPropertyInfoProvider>();
        propertyInfoProviderMock
            .GetPropertyInfoAndFullName(defaultSortingPropertyExpression)
            .Returns((defaultSortingPropertyInfo, defaultSortingPropertyName));

        // Act
        var builder = new ObjectMetadataBuilder<Blog>(
            propertyInfoProviderMock,
            objectMetadata,
            defaultSortingPropertyExpression);

        // Assert
        objectMetadata.Should().NotBeEmpty();
        objectMetadata.Should().HaveCount(1);
        objectMetadata.Keys.Should().BeEquivalentTo(new[] { typeof(Blog) });
        objectMetadata.Values.First().DefaultSortingPropertyName.Should().Be(defaultSortingPropertyName);
        objectMetadata.Values.First().DefaultSortingPropertyInfo.Should().BeSameAs(defaultSortingPropertyInfo);
        objectMetadata.Values.First().IsDefaultSortingDescending.Should().BeFalse();
        objectMetadata.Values.First().IsFilterable.Should().BeFalse();
        objectMetadata.Values.First().IsSortable.Should().BeFalse();
    }

    [Fact]
    public void Should_Save_ObjectMetadata_WhenMarkingAsFilterable()
    {
        // Arrange
        var objectMetadata = new Dictionary<Type, IObjectMetadata>();
        var defaultSortingPropertyName = nameof(Blog.Title);
        var defaultSortingPropertyInfo = typeof(Blog).GetProperty(defaultSortingPropertyName);
        Expression<Func<Blog, object>> defaultSortingPropertyExpression = b => b.Title;
        var propertyInfoProviderMock = Substitute.For<IPropertyInfoProvider>();
        propertyInfoProviderMock
            .GetPropertyInfoAndFullName(defaultSortingPropertyExpression)
            .Returns((defaultSortingPropertyInfo, defaultSortingPropertyName));

        // Act
        var builder = new ObjectMetadataBuilder<Blog>(
            propertyInfoProviderMock,
            objectMetadata,
            defaultSortingPropertyExpression);
        builder.IsFilterable();

        // Assert
        objectMetadata.Should().NotBeEmpty();
        objectMetadata.Should().HaveCount(1);
        objectMetadata.Keys.Should().BeEquivalentTo(new[] { typeof(Blog) });
        objectMetadata.Values.First().DefaultSortingPropertyName.Should().Be(defaultSortingPropertyName);
        objectMetadata.Values.First().DefaultSortingPropertyInfo.Should().BeSameAs(defaultSortingPropertyInfo);
        objectMetadata.Values.First().IsDefaultSortingDescending.Should().BeFalse();
        objectMetadata.Values.First().IsFilterable.Should().BeTrue();
        objectMetadata.Values.First().IsSortable.Should().BeFalse();
    }

    [Fact]
    public void Should_Save_ObjectMetadata_WhenMarkingAsSortable()
    {
        // Arrange
        var objectMetadata = new Dictionary<Type, IObjectMetadata>();
        var defaultSortingPropertyName = nameof(Blog.Title);
        var defaultSortingPropertyInfo = typeof(Blog).GetProperty(defaultSortingPropertyName);
        Expression<Func<Blog, object>> defaultSortingPropertyExpression = b => b.Title;
        var propertyInfoProviderMock = Substitute.For<IPropertyInfoProvider>();
        propertyInfoProviderMock
            .GetPropertyInfoAndFullName(defaultSortingPropertyExpression)
            .Returns((defaultSortingPropertyInfo, defaultSortingPropertyName));

        // Act
        var builder = new ObjectMetadataBuilder<Blog>(
            propertyInfoProviderMock,
            objectMetadata,
            defaultSortingPropertyExpression);
        builder.IsSortable();

        // Assert
        objectMetadata.Should().NotBeEmpty();
        objectMetadata.Should().HaveCount(1);
        objectMetadata.Keys.Should().BeEquivalentTo(new[] { typeof(Blog) });
        objectMetadata.Values.First().DefaultSortingPropertyName.Should().Be(defaultSortingPropertyName);
        objectMetadata.Values.First().DefaultSortingPropertyInfo.Should().BeSameAs(defaultSortingPropertyInfo);
        objectMetadata.Values.First().IsDefaultSortingDescending.Should().BeFalse();
        objectMetadata.Values.First().IsFilterable.Should().BeFalse();
        objectMetadata.Values.First().IsSortable.Should().BeTrue();
    }

    [Fact]
    public void Should_Save_ObjectMetadata_WhenMarkingAsDefaultSortingAscending()
    {
        // Arrange
        var objectMetadata = new Dictionary<Type, IObjectMetadata>();
        var defaultSortingPropertyName = nameof(Blog.Title);
        var defaultSortingPropertyInfo = typeof(Blog).GetProperty(defaultSortingPropertyName);
        Expression<Func<Blog, object>> defaultSortingPropertyExpression = b => b.Title;
        var propertyInfoProviderMock = Substitute.For<IPropertyInfoProvider>();
        propertyInfoProviderMock
            .GetPropertyInfoAndFullName(defaultSortingPropertyExpression)
            .Returns((defaultSortingPropertyInfo, defaultSortingPropertyName));

        // Act
        var builder = new ObjectMetadataBuilder<Blog>(
            propertyInfoProviderMock,
            objectMetadata,
            defaultSortingPropertyExpression);
        builder.IsDefaultSortingAscending();

        // Assert
        objectMetadata.Should().NotBeEmpty();
        objectMetadata.Should().HaveCount(1);
        objectMetadata.Keys.Should().BeEquivalentTo(new[] { typeof(Blog) });
        objectMetadata.Values.First().DefaultSortingPropertyName.Should().Be(defaultSortingPropertyName);
        objectMetadata.Values.First().DefaultSortingPropertyInfo.Should().BeSameAs(defaultSortingPropertyInfo);
        objectMetadata.Values.First().IsDefaultSortingDescending.Should().BeFalse();
        objectMetadata.Values.First().IsFilterable.Should().BeFalse();
        objectMetadata.Values.First().IsSortable.Should().BeFalse();
    }

    [Fact]
    public void Should_Save_ObjectMetadata_WhenMarkingAsDefaultSortingDescending()
    {
        // Arrange
        var objectMetadata = new Dictionary<Type, IObjectMetadata>();
        var defaultSortingPropertyName = nameof(Blog.Title);
        var defaultSortingPropertyInfo = typeof(Blog).GetProperty(defaultSortingPropertyName);
        Expression<Func<Blog, object>> defaultSortingPropertyExpression = b => b.Title;
        var propertyInfoProviderMock = Substitute.For<IPropertyInfoProvider>();
        propertyInfoProviderMock
            .GetPropertyInfoAndFullName(defaultSortingPropertyExpression)
            .Returns((defaultSortingPropertyInfo, defaultSortingPropertyName));

        // Act
        var builder = new ObjectMetadataBuilder<Blog>(
            propertyInfoProviderMock,
            objectMetadata,
            defaultSortingPropertyExpression);
        builder.IsDefaultSortingDescending();

        // Assert
        objectMetadata.Should().NotBeEmpty();
        objectMetadata.Should().HaveCount(1);
        objectMetadata.Keys.Should().BeEquivalentTo(new[] { typeof(Blog) });
        objectMetadata.Values.First().DefaultSortingPropertyName.Should().Be(defaultSortingPropertyName);
        objectMetadata.Values.First().DefaultSortingPropertyInfo.Should().BeSameAs(defaultSortingPropertyInfo);
        objectMetadata.Values.First().IsDefaultSortingDescending.Should().BeTrue();
        objectMetadata.Values.First().IsFilterable.Should().BeFalse();
        objectMetadata.Values.First().IsSortable.Should().BeFalse();
    }

    private class Blog
    {
        public string Title { get; set; }
    }
}
