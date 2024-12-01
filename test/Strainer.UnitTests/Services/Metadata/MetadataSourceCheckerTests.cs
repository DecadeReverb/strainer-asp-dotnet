using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Metadata;
using System.Collections;

namespace Fluorite.Strainer.UnitTests.Services.Metadata;

public class MetadataSourceCheckerTests
{
    private readonly IStrainerOptionsProvider _strainerOptionsProviderMock = Substitute.For<IStrainerOptionsProvider>();
    private readonly MetadataSourceChecker _checker;

    public MetadataSourceCheckerTests()
    {
        _checker = new MetadataSourceChecker(_strainerOptionsProviderMock);
    }

    [Theory]
    [ClassData(typeof(MetadataSourceTypeTestData))]
    public void Should_Check_IfMetadataSourceIsEnabled(MetadataSourceType sourceType)
    {
        // Arrange
        var strainerOptions = new StrainerOptions
        {
            MetadataSourceType = sourceType,
        };
        _strainerOptionsProviderMock
            .GetStrainerOptions()
            .Returns(strainerOptions);

        // Act
        var result = _checker.IsMetadataSourceEnabled(sourceType);

        // Assert
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData(MetadataSourceType.None, MetadataSourceType.Attributes, false)]
    [InlineData(MetadataSourceType.None, MetadataSourceType.FluentApi, false)]
    [InlineData(MetadataSourceType.None, MetadataSourceType.All, false)]
    [InlineData(MetadataSourceType.FluentApi, MetadataSourceType.FluentApi, true)]
    [InlineData(MetadataSourceType.Attributes, MetadataSourceType.FluentApi, false)]
    [InlineData(MetadataSourceType.Attributes, MetadataSourceType.Attributes, true)]
    [InlineData(MetadataSourceType.Attributes, MetadataSourceType.PropertyAttributes, true)]
    [InlineData(MetadataSourceType.Attributes, MetadataSourceType.ObjectAttributes, true)]
    [InlineData(MetadataSourceType.All, MetadataSourceType.PropertyAttributes, true)]
    [InlineData(MetadataSourceType.All, MetadataSourceType.ObjectAttributes, true)]
    [InlineData(MetadataSourceType.All, MetadataSourceType.Attributes, true)]
    [InlineData(MetadataSourceType.All, MetadataSourceType.FluentApi, true)]
    [InlineData(MetadataSourceType.All, MetadataSourceType.All, true)]
    public void Should_Check_IfMetadataSourceIsEnabled_AgaintScenarios(
        MetadataSourceType sourceTypeInOptions,
        MetadataSourceType sourceTypeToCheck,
        bool expectedResult)
    {
        // Arrange
        var strainerOptions = new StrainerOptions
        {
            MetadataSourceType = sourceTypeInOptions,
        };
        _strainerOptionsProviderMock
            .GetStrainerOptions()
            .Returns(strainerOptions);

        // Act
        var result = _checker.IsMetadataSourceEnabled(sourceTypeToCheck);

        // Assert
        result.Should().Be(expectedResult);
    }

    public class MetadataSourceTypeTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            return Enum.GetValues<MetadataSourceType>().Cast<object>().Chunk(1).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
