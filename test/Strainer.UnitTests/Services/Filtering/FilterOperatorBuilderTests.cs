using Fluorite.Strainer.Services.Filter;
using Xunit;

namespace Fluorite.Strainer.UnitTests.Services.Filtering
{
    public class FilterOperatorBuilderTests
    {
        [Fact]
        public void Builder_works()
        {
            // Arrange
            var validator = new FilterOperatorValidator();
            var mapper = new FilterOperatorMapper(validator);

            // Act
            var builder = new FilterOperatorBuilder(mapper, symbol: "!=");

            // Assert
        }
    }
}
