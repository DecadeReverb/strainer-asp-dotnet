using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Metadata;
using Moq;
using System.Collections;

namespace Fluorite.Strainer.UnitTests.Services.Metadata
{
    public class MetadataSourceCheckerTests
    {
        private readonly Mock<IStrainerOptionsProvider> _strainerOptionsProviderMock = new();
        private readonly MetadataSourceChecker _checker;

        public MetadataSourceCheckerTests()
        {
            _checker = new MetadataSourceChecker(_strainerOptionsProviderMock.Object);
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
                .Setup(x => x.GetStrainerOptions())
                .Returns(strainerOptions);

            // Act
            var result = _checker.IsMetadataSourceEnabled(sourceType);

            // Assert
            result.Should().BeTrue();
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
}
