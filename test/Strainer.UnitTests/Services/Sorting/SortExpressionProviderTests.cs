using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Models.Sorting.Terms;
using Fluorite.Strainer.Services.Metadata;
using Fluorite.Strainer.Services.Sorting;
using NSubstitute.ReturnsExtensions;
using System.Reflection;

namespace Fluorite.Strainer.UnitTests.Services.Sorting
{
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
            var propertyMetadata = new PropertyMetadata
            {
                IsSortable = true,
                Name = propertyInfo.Name,
                PropertyInfo = propertyInfo,
            };
            var sortTerm = new SortTerm
            {
                IsDescending = false,
                Name = propertyInfo.Name,
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
            var sortTerm = new SortTerm
            {
                Input = $"-{nameof(Post.TopComment)}.{nameof(Comment.Text)}.{nameof(string.Length)}",
                IsDescending = true,
                Name = $"{nameof(Post.TopComment)}.{nameof(Comment.Text)}.{nameof(string.Length)}"
            };
            var propertyInfo = typeof(string).GetProperty(nameof(string.Length));
            var sortTerms = new Dictionary<PropertyInfo, ISortTerm>
            {
                { propertyInfo, sortTerm },
            };
            var propertyMetadata = new PropertyMetadata
            {
                IsSortable = true,
                Name = sortTerm.Name,
                PropertyInfo = propertyInfo,
            };

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
                new SortTerm
                {
                    Input = $"-{nameof(Comment.Text)},{nameof(Comment.Id)}",
                    IsDescending = true,
                    Name = nameof(Comment.Text),
                },
                new SortTerm
                {
                    Input = $"-{nameof(Comment.Text)},{nameof(Comment.Id)}",
                    IsDescending = false,
                    Name = nameof(Comment.Id),
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
            var propertyMetadatas = new []
            {
                new PropertyMetadata
                {
                    Name = nameof(Comment.Text),
                    IsSortable = true,
                    PropertyInfo = properties[0],
                },
                new PropertyMetadata
                {
                    Name = nameof(Comment.Id),
                    IsSortable = true,
                    PropertyInfo = properties[1],
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
}
