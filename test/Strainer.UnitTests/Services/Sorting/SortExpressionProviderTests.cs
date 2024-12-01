using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Models.Sorting.Terms;
using Fluorite.Strainer.Services.Metadata;
using Fluorite.Strainer.Services.Sorting;
using NSubstitute.ReturnsExtensions;
using System.Reflection;

namespace Fluorite.Strainer.UnitTests.Services.Sorting;

public class SortExpressionProviderTests
{
    private readonly IMetadataFacade _metadataProvidersFacadeMock = Substitute.For<IMetadataFacade>();

    private readonly SortExpressionProvider _provider;

    public SortExpressionProviderTests()
    {
        _provider = new SortExpressionProvider(_metadataProvidersFacadeMock);
    }

    [Fact]
    public void Provider_Returns_ListOfSortExpressions()
    {
        // Arrange
        var propertyInfo = typeof(Comment).GetProperty(nameof(Comment.Text));
        var propertyMetadata = new PropertyMetadata(propertyInfo.Name, propertyInfo)
        {
            IsSortable = true,
        };
        var sortTerm = new SortTerm(propertyInfo.Name)
        {
            IsDescending = false,
        };
        var sortTerms = new Dictionary<PropertyInfo, ISortTerm>
        {
            { propertyInfo, sortTerm },
        };

        _metadataProvidersFacadeMock
            .GetMetadata<Comment>(true, false, sortTerm.Name)
            .Returns(propertyMetadata);

        // Act
        var sortExpressions = _provider.GetExpressions<Comment>(sortTerms).ToList();

        // Assert
        sortExpressions.Should().NotBeEmpty();
        sortExpressions[0].IsDefault.Should().BeFalse();
        sortExpressions[0].IsDescending.Should().BeFalse();
        sortExpressions[0].IsSubsequent.Should().BeFalse();
    }

    [Fact]
    public void Provider_Returns_EmptyListOfSortExpressions_When_NoMatchingPropertyIsFound()
    {
        // Arrange
        var propertyInfo = Substitute.For<PropertyInfo>();
        var sortTerm = Substitute.For<ISortTerm>();
        var sortTerms = new Dictionary<PropertyInfo, ISortTerm>
        {
            { propertyInfo, sortTerm },
        };

        _metadataProvidersFacadeMock
            .GetMetadata<Comment>(Arg.Any<bool>(), Arg.Any<bool>(), Arg.Any<string>())
            .ReturnsNull();

        // Act
        var sortExpressions = _provider.GetExpressions<Comment>(sortTerms);

        // Assert
        sortExpressions.Should().BeEmpty();

        _metadataProvidersFacadeMock
            .Received(1)
            .GetMetadata<Comment>(true, false, sortTerm.Name);
    }

    [Fact]
    public void Provider_Returns_ListOfSortExpressions_For_NestedProperty()
    {
        // Arrange
        var name = $"{nameof(Post.TopComment)}.{nameof(Comment.Text)}.{nameof(string.Length)}";
        var sortTerm = new SortTerm(name)
        {
            Input = $"-{nameof(Post.TopComment)}.{nameof(Comment.Text)}.{nameof(string.Length)}",
            IsDescending = true,
        };
        var propertyInfo = typeof(string).GetProperty(nameof(string.Length));
        var sortTerms = new Dictionary<PropertyInfo, ISortTerm>
        {
            { propertyInfo, sortTerm },
        };
        var propertyMetadata = Substitute.For<IPropertyMetadata>();
        propertyMetadata.Name.Returns(sortTerm.Name);
        propertyMetadata.IsSortable.Returns(true);

        _metadataProvidersFacadeMock
            .GetMetadata<Post>(true, false, sortTerm.Name)
            .Returns(propertyMetadata);

        // Act
        var sortExpressions = _provider.GetExpressions<Post>(sortTerms).ToList();

        // Assert
        sortExpressions.Should().NotBeEmpty();
        sortExpressions[0].IsDefault.Should().BeFalse();
        sortExpressions[0].IsDescending.Should().BeTrue();
        sortExpressions[0].IsSubsequent.Should().BeFalse();
    }

    [Fact]
    public void Provider_Returns_ListOfSortExpressions_For_SubsequentSorting()
    {
        // Arrange
        var termsList = new List<ISortTerm>
        {
            new SortTerm(nameof(Comment.Text))
            {
                Input = $"-{nameof(Comment.Text)},{nameof(Comment.Id)}",
                IsDescending = true,
            },
            new SortTerm(nameof(Comment.Id))
            {
                Input = $"-{nameof(Comment.Text)},{nameof(Comment.Id)}",
                IsDescending = false,
            },
        };
        var properties = new PropertyInfo[]
        {
            typeof(Comment).GetProperty(nameof(Comment.Text)),
            typeof(Comment).GetProperty(nameof(Comment.Id)),
        };
        var sortTerms = new Dictionary<PropertyInfo, ISortTerm>
        {
            { properties[0], termsList[0] },
            { properties[1], termsList[1] },
        };
        var propertyMetadatas = new[]
        {
            new PropertyMetadata(nameof(Comment.Text), properties[0])
            {
                IsSortable = true,
            },
            new PropertyMetadata(nameof(Comment.Id), properties[1])
            {
                IsSortable = true,
            },
        };

        _metadataProvidersFacadeMock
            .GetMetadata<Comment>(true, false, termsList[0].Name)
            .Returns(propertyMetadatas[0]);
        _metadataProvidersFacadeMock
            .GetMetadata<Comment>(true, false, termsList[1].Name)
            .Returns(propertyMetadatas[1]);

        // Act
        var sortExpressions = _provider.GetExpressions<Comment>(sortTerms).ToList();

        // Assert
        sortExpressions.Should().HaveCount(2);
        sortExpressions[0].IsDefault.Should().BeFalse();
        sortExpressions[0].IsDescending.Should().BeTrue();
        sortExpressions[0].IsSubsequent.Should().BeFalse();
        sortExpressions[1].IsDefault.Should().BeFalse();
        sortExpressions[1].IsDescending.Should().BeFalse();
        sortExpressions[1].IsSubsequent.Should().BeTrue();
    }

    private class Comment
    {
        public int Id { get; set; }

        public string Text { get; set; }
    }

    private class Post
    {
        public Comment TopComment { get; set; }
    }
}
