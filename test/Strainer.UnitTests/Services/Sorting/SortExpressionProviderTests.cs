using Fluorite.Extensions;
using Fluorite.Strainer.Attributes;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Configuration;
using Fluorite.Strainer.Models.Filtering;
using Fluorite.Strainer.Models.Filtering.Operators;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Models.Sorting;
using Fluorite.Strainer.Models.Sorting.Terms;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Configuration;
using Fluorite.Strainer.Services.Metadata;
using Fluorite.Strainer.Services.Sorting;
using Moq;
using System.Reflection;

namespace Fluorite.Strainer.UnitTests.Services.Sorting
{
    public class SortExpressionProviderTests
    {
        [Fact]
        public void Provider_Returns_ListOfSortExpressions()
        {
            // Arrange
            var sortTerm = new SortTerm
            {
                Input = "Text",
                IsDescending = false,
                Name = "Text"
            };
            var propertyInfo = typeof(Comment).GetProperty(nameof(Comment.Text));
            var sortTerms = new Dictionary<PropertyInfo, ISortTerm>
            {
                { propertyInfo, sortTerm },
            };
            var optionsMock = new Mock<IStrainerOptionsProvider>();
            optionsMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var optionsProvider = optionsMock.Object;
            var propertyInfoProvider = new PropertyInfoProvider();
            var mapper = new MetadataMapper(optionsProvider, propertyInfoProvider);
            mapper.Property<Comment>(c => c.Text).IsSortable();
            var fluentApiMetadataProvider = CreateFluentApiMetadataProvider(optionsProvider, mapper);
            var propertyMetadataProviders = new IMetadataProvider[]
            {
                fluentApiMetadataProvider,
            };
            var mainMetadataProvider = new MetadataFacade(propertyMetadataProviders);
            var expressionProvider = new SortExpressionProvider(mainMetadataProvider);

            // Act
            var sortExpressions = expressionProvider.GetExpressions<Comment>(sortTerms);
            var firstExpression = sortExpressions.FirstOrDefault();

            // Assert
            sortExpressions.Should().NotBeEmpty();
            firstExpression.IsDescending.Should().BeFalse();
            firstExpression.IsSubsequent.Should().BeFalse();
        }

        [Fact]
        public void Provider_Returns_EmptyListOfSortExpressions_When_NoMatchingPropertyIsFound()
        {
            // Arrange
            var sortTerm = new SortTerm
            {
                Input = "Text",
                IsDescending = false,
                Name = "Text"
            };
            var propertyInfo = typeof(Comment).GetProperty(nameof(Comment.Text));
            var sortTerms = new Dictionary<PropertyInfo, ISortTerm>
            {
                { propertyInfo, sortTerm },
            };
            var optionsProviderMock = new Mock<IStrainerOptionsProvider>();
            optionsProviderMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var optionsProvider = optionsProviderMock.Object;
            var propertyInfoProvider = new PropertyInfoProvider();
            var mapper = new MetadataMapper(optionsProvider, propertyInfoProvider);
            var fluentApiMetadataProvider = CreateFluentApiMetadataProvider(optionsProvider, mapper);
            var propertyMetadataProviders = new IMetadataProvider[]
            {
                fluentApiMetadataProvider,
            };
            var mainMetadataProvider = new MetadataFacade(propertyMetadataProviders);
            var expressionProvider = new SortExpressionProvider(mainMetadataProvider);

            // Act
            var sortExpressions = expressionProvider.GetExpressions<Comment>(sortTerms);

            // Assert
            sortExpressions.Should().BeEmpty();
        }

        [Fact]
        public void Provider_Returns_ListOfSortExpressions_For_NestedProperty()
        {
            // Arrange
            var sortTerm = new SortTerm
            {
                Input = "-TopComment.Text.Length",
                IsDescending = true,
                Name = "TopComment.Text.Length"
            };
            var propertyInfo = typeof(string).GetProperty(nameof(string.Length));
            var sortTerms = new Dictionary<PropertyInfo, ISortTerm>
            {
                { propertyInfo, sortTerm },
            };
            var optionsMock = new Mock<IStrainerOptionsProvider>();
            optionsMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var optionsProvider = optionsMock.Object;
            var propertyInfoProvider = new PropertyInfoProvider();
            var mapper = new MetadataMapper(optionsProvider, propertyInfoProvider);
            mapper.Property<Post>(c => c.TopComment.Text.Length).IsSortable();
            var fluentApiMetadataProvider = CreateFluentApiMetadataProvider(optionsProvider, mapper);
            var propertyMetadataProviders = new IMetadataProvider[]
            {
                fluentApiMetadataProvider,
            };
            var mainMetadataProvider = new MetadataFacade(propertyMetadataProviders);
            var expressionProvider = new SortExpressionProvider(mainMetadataProvider);

            // Act
            var sortExpressions = expressionProvider.GetExpressions<Post>(sortTerms);
            var firstExpression = sortExpressions.FirstOrDefault();

            // Assert
            sortExpressions.Should().NotBeEmpty();
            firstExpression.IsDescending.Should().BeTrue();
            firstExpression.IsSubsequent.Should().BeFalse();
        }

        [Fact]
        public void Provider_Returns_ListOfSortExpressions_For_SubsequentSorting()
        {
            // Arrange
            var termsList = new List<ISortTerm>
            {
                new SortTerm
                {
                    Input = "-Text,Id",
                    IsDescending = true,
                    Name = "Text"
                },
                new SortTerm
                {
                    Input = "-Text,Id",
                    IsDescending = false,
                    Name = "Id"
                },
            };
            var properties = new PropertyInfo[]
            {
                typeof(Comment).GetProperty(nameof(Comment.Text)),
                typeof(Comment).GetProperty(nameof(Comment.Id)),
            };
            var sortTerms = properties.Zip(termsList, (prop, term) => new { prop, term })
                .ToDictionary(x => x.prop, x => x.term);
            var optionsMock = new Mock<IStrainerOptionsProvider>();
            optionsMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var optionsProvider = optionsMock.Object;
            var propertyInfoProvider = new PropertyInfoProvider();
            var mapper = new MetadataMapper(optionsProvider, propertyInfoProvider);
            mapper.Property<Comment>(c => c.Text).IsSortable();
            mapper.Property<Comment>(c => c.Id).IsSortable();
            mapper.Property<Comment>(c => c.DateCreated).IsSortable();
            var metadataSourceTypeProviderMock = new Mock<IMetadataSourceTypeProvider>();
            var metadataAssemblySourceProviderMock = new Mock<IMetadataAssemblySourceProvider>();
            var objectMetadataProviderMock = new Mock<IObjectMetadataProvider>();
            var fluentApiMetadataProvider = CreateFluentApiMetadataProvider(optionsProvider, mapper);
            var attributeMetadataProvider = new AttributeMetadataProvider(
                optionsProvider,
                metadataSourceTypeProviderMock.Object,
                metadataAssemblySourceProviderMock.Object,
                objectMetadataProviderMock.Object);
            var propertyMetadataProviders = new IMetadataProvider[]
            {
                attributeMetadataProvider,
                fluentApiMetadataProvider,
            };
            var mainMetadataProvider = new MetadataFacade(propertyMetadataProviders);
            var expressionProvider = new SortExpressionProvider(mainMetadataProvider);

            // Act
            var sortExpressions = expressionProvider.GetExpressions<Comment>(sortTerms);
            var firstExpression = sortExpressions.FirstOrDefault();
            var secondExpression = sortExpressions.LastOrDefault();

            // Assert
            sortExpressions.Should().HaveCount(2);
            firstExpression.IsDescending.Should().BeTrue();
            firstExpression.IsSubsequent.Should().BeFalse();
            secondExpression.IsDescending.Should().BeFalse();
            secondExpression.IsSubsequent.Should().BeTrue();
        }

        private FluentApiMetadataProvider CreateFluentApiMetadataProvider(
            IStrainerOptionsProvider optionsProvider,
            MetadataMapper mapper)
        {
            var defaultMetadata = mapper.DefaultMetadata.ToReadOnly();
            var objectMetadata = mapper.ObjectMetadata.ToReadOnly();
            var propertyMetadata = mapper
                .PropertyMetadata
                .Select(pair =>
                    new KeyValuePair<Type, IReadOnlyDictionary<string, IPropertyMetadata>>(
                        pair.Key, pair.Value.ToReadOnly()))
                .ToReadOnlyDictionary();
            var strainerConfiguration = new StrainerConfiguration(
                new Dictionary<Type, IReadOnlyDictionary<string, ICustomFilterMethod>>(),
                new Dictionary<Type, IReadOnlyDictionary<string, ICustomSortMethod>>(),
                defaultMetadata,
                new Dictionary<string, IFilterOperator>(),
                objectMetadata,
                propertyMetadata);
            var strainerConfigurationProvider = new StrainerConfigurationProvider(strainerConfiguration);
            var configurationMetadataProvider = new ConfigurationMetadataProvider(strainerConfigurationProvider);
            var fluentApiMetadataProvider = new FluentApiMetadataProvider(
                optionsProvider,
                configurationMetadataProvider);

            return fluentApiMetadataProvider;
        }

        private class Comment
        {
            [StrainerProperty]
            public DateTimeOffset DateCreated { get; set; }

            public int Id { get; set; }

            [StrainerProperty]
            public string Text { get; set; }
        }

        private class Post
        {
            public Comment TopComment { get; set; }
        }
    }
}
