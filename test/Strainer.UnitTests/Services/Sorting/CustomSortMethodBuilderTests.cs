using Fluorite.Strainer.Models.Sorting;
using Fluorite.Strainer.Services.Sorting;

namespace Fluorite.Strainer.UnitTests.Services.Sorting;

public class CustomSortMethodBuilderTests
{
    [Fact]
    public void Should_Build_CustomSortMethod_UsingDirectExpression()
    {
        // Arrange
        var name = "foo";
        var customMethodsDictionary = new Dictionary<Type, IDictionary<string, ICustomSortMethod>>();
        var builder = new CustomSortMethodBuilder<Post>(customMethodsDictionary, name);

        // Act
        builder.HasFunction(x => x.Name);
        var result = builder.Build();

        // Assert
        result.Should().NotBeNull();
        result.Expression.Should().NotBeNull();
        result.Name.Should().Be(name);
        result.ExpressionProvider.Should().BeNull();
    }

    [Fact]
    public void Should_Build_CustomSortMethod_UsingProviderExpression()
    {
        // Arrange
        var name = "foo";
        var customMethodsDictionary = new Dictionary<Type, IDictionary<string, ICustomSortMethod>>();
        var builder = new CustomSortMethodBuilder<Post>(customMethodsDictionary, name);

        // Act
        builder.HasFunction((sortTerm) => sortTerm.IsDescending ? (x => x.Author) : (x => x.Name));
        var result = builder.Build();

        // Assert
        result.Should().NotBeNull();
        result.Expression.Should().BeNull();
        result.Name.Should().Be(name);
        result.ExpressionProvider.Should().NotBeNull();
    }

    [Fact]
    public void Should_Build_CustomSortMethod_UsingDirectExpression_ErasingPreviousExpression()
    {
        // Arrange
        var name = "foo";
        var customMethodsDictionary = new Dictionary<Type, IDictionary<string, ICustomSortMethod>>();
        var builder = new CustomSortMethodBuilder<Post>(customMethodsDictionary, name);

        // Act
        builder.HasFunction(term => x => x.Name);
        builder.HasFunction(x => x.Name);
        var result = builder.Build();

        // Assert
        result.Should().NotBeNull();
        result.Expression.Should().NotBeNull();
        result.Name.Should().Be(name);
        result.ExpressionProvider.Should().BeNull();
    }

    private class Post
    {
        public string Author { get; set; }

        public string Name { get; set; }
    }
}
