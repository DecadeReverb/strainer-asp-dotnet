using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Services.Metadata;

namespace Fluorite.Strainer.UnitTests.Services.Metadata;

public class PropertyMetadataBuilderTests
{
    [Fact]
    public void Should_Save_PropertyMetadata_WhenCreatingBuilder()
    {
        // Arrange
        var propertyMetadata = new Dictionary<Type, IDictionary<string, IPropertyMetadata>>();
        var defaultMetadata = new Dictionary<Type, IPropertyMetadata>();
        var type = typeof(Blog);
        var propertyInfo = type.GetProperty(nameof(Blog.Title));
        var propertyName = nameof(Blog.Title);

        // Act
        _ = new PropertyMetadataBuilder<Blog>(
            propertyMetadata,
            defaultMetadata,
            propertyInfo,
            propertyName);

        // Assert
        propertyMetadata.Should().NotBeEmpty();
        propertyMetadata.Keys.Should().BeEquivalentTo([type]);
        propertyMetadata[type].Should().NotBeNullOrEmpty();
        propertyMetadata[type].Keys.Should().BeEquivalentTo(propertyName);
        propertyMetadata[type][propertyName].DisplayName.Should().BeNull();
        propertyMetadata[type][propertyName].IsDefaultSorting.Should().BeFalse();
        propertyMetadata[type][propertyName].IsDefaultSortingDescending.Should().BeFalse();
        propertyMetadata[type][propertyName].IsFilterable.Should().BeFalse();
        propertyMetadata[type][propertyName].IsSortable.Should().BeFalse();
        propertyMetadata[type][propertyName].Name.Should().Be(propertyName);
        propertyMetadata[type][propertyName].PropertyInfo.Should().BeSameAs(propertyInfo);
    }

    [Fact]
    public void Should_Save_PropertyMetadata_WhenMarkingAsFilterable()
    {
        // Arrange
        var propertyMetadata = new Dictionary<Type, IDictionary<string, IPropertyMetadata>>();
        var defaultMetadata = new Dictionary<Type, IPropertyMetadata>();
        var type = typeof(Blog);
        var propertyInfo = type.GetProperty(nameof(Blog.Title));
        var propertyName = nameof(Blog.Title);

        // Act
        var builder = new PropertyMetadataBuilder<Blog>(
            propertyMetadata,
            defaultMetadata,
            propertyInfo,
            propertyName);
        builder.IsFilterable();

        // Assert
        propertyMetadata.Should().NotBeEmpty();
        propertyMetadata.Keys.Should().BeEquivalentTo([type]);
        propertyMetadata[type].Should().NotBeNullOrEmpty();
        propertyMetadata[type].Keys.Should().BeEquivalentTo(propertyName);
        propertyMetadata[type][propertyName].DisplayName.Should().BeNull();
        propertyMetadata[type][propertyName].IsDefaultSorting.Should().BeFalse();
        propertyMetadata[type][propertyName].IsDefaultSortingDescending.Should().BeFalse();
        propertyMetadata[type][propertyName].IsFilterable.Should().BeTrue();
        propertyMetadata[type][propertyName].IsSortable.Should().BeFalse();
        propertyMetadata[type][propertyName].Name.Should().Be(propertyName);
        propertyMetadata[type][propertyName].PropertyInfo.Should().BeSameAs(propertyInfo);
    }

    [Fact]
    public void Should_Save_PropertyMetadata_WhenMarkingAsSortable()
    {
        // Arrange
        var propertyMetadata = new Dictionary<Type, IDictionary<string, IPropertyMetadata>>();
        var defaultMetadata = new Dictionary<Type, IPropertyMetadata>();
        var type = typeof(Blog);
        var propertyInfo = type.GetProperty(nameof(Blog.Title));
        var propertyName = nameof(Blog.Title);

        // Act
        var builder = new PropertyMetadataBuilder<Blog>(
            propertyMetadata,
            defaultMetadata,
            propertyInfo,
            propertyName);
        var sortableBuilder = builder.IsSortable();

        // Assert
        propertyMetadata.Should().NotBeEmpty();
        propertyMetadata.Keys.Should().BeEquivalentTo([type]);
        propertyMetadata[type].Should().NotBeNullOrEmpty();
        propertyMetadata[type].Keys.Should().BeEquivalentTo(propertyName);
        propertyMetadata[type][propertyName].DisplayName.Should().BeNull();
        propertyMetadata[type][propertyName].IsDefaultSorting.Should().BeFalse();
        propertyMetadata[type][propertyName].IsDefaultSortingDescending.Should().BeFalse();
        propertyMetadata[type][propertyName].IsFilterable.Should().BeFalse();
        propertyMetadata[type][propertyName].IsSortable.Should().BeTrue();
        propertyMetadata[type][propertyName].Name.Should().Be(propertyName);
        propertyMetadata[type][propertyName].PropertyInfo.Should().BeSameAs(propertyInfo);

        sortableBuilder.Should().NotBeNull();
    }

    [Fact]
    public void Should_Save_PropertyMetadata_WhenSettingDisplayName()
    {
        // Arrange
        var propertyMetadata = new Dictionary<Type, IDictionary<string, IPropertyMetadata>>();
        var defaultMetadata = new Dictionary<Type, IPropertyMetadata>();
        var type = typeof(Blog);
        var propertyInfo = type.GetProperty(nameof(Blog.Title));
        var propertyName = nameof(Blog.Title);
        var displayName = "SuperTitle";

        // Act
        var builder = new PropertyMetadataBuilder<Blog>(
            propertyMetadata,
            defaultMetadata,
            propertyInfo,
            propertyName);
        builder.HasDisplayName(displayName);

        // Assert
        propertyMetadata.Should().NotBeEmpty();
        propertyMetadata.Keys.Should().BeEquivalentTo([type]);
        propertyMetadata[type].Should().NotBeNullOrEmpty();
        propertyMetadata[type].Keys.Should().BeEquivalentTo(displayName);
        propertyMetadata[type][displayName].DisplayName.Should().Be(displayName);
        propertyMetadata[type][displayName].IsDefaultSorting.Should().BeFalse();
        propertyMetadata[type][displayName].IsDefaultSortingDescending.Should().BeFalse();
        propertyMetadata[type][displayName].IsFilterable.Should().BeFalse();
        propertyMetadata[type][displayName].IsSortable.Should().BeFalse();
        propertyMetadata[type][displayName].Name.Should().Be(propertyName);
        propertyMetadata[type][displayName].PropertyInfo.Should().BeSameAs(propertyInfo);
    }

    private class Blog
    {
        public string Title { get; set; }
    }
}
