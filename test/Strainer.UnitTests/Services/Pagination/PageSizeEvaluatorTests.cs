using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Pagination;
using Moq;

namespace Fluorite.Strainer.UnitTests.Services.Pagination
{
    public class PageSizeEvaluatorTests
    {
        private readonly Mock<IStrainerOptionsProvider> _strainerOptionsProviderMock = new();
        private readonly PageSizeEvaluator _evaluator;

        public PageSizeEvaluatorTests()
        {
            _evaluator = new PageSizeEvaluator(_strainerOptionsProviderMock.Object);
        }

        [Fact]
        public void Should_Throw_ForNullOptionsProvider()
        {
            // Arrange & Act
            Action act = () => new PageSizeEvaluator(strainerOptionsProvider: null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Should_Throw_ForNullStrainerModel()
        {
            // Arrange & Act
            Action act = () => _evaluator.Evaluate(model: null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(23)]
        [InlineData(50)]
        public void Should_Return_PageSize(int? pageSize)
        {
            // Arrange
            var model = new StrainerModel
            {
                PageSize = pageSize,
            };

            _strainerOptionsProviderMock
                .Setup(x => x.GetStrainerOptions())
                .Returns(new StrainerOptions());

            // Act
            var result = _evaluator.Evaluate(model);

            // Assert
            result.Should().Be(pageSize.Value);
        }

        [Fact]
        public void Should_Return_DefaultPageSize_WhenModelPageSizeIsNull()
        {
            // Arrange
            var model = new StrainerModel
            {
                PageSize = null,
            };
            var defaultPageSize = 23;
            var strainerOptions = new StrainerOptions
            {
                DefaultPageSize = defaultPageSize,
            };

            _strainerOptionsProviderMock
                .Setup(x => x.GetStrainerOptions())
                .Returns(strainerOptions);

            // Act
            var result = _evaluator.Evaluate(model);

            // Assert
            result.Should().Be(defaultPageSize);
        }
    }
}
