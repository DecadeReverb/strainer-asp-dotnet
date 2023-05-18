using Fluorite.Strainer.Models.Filtering;
using Fluorite.Strainer.Services.Filtering;

namespace Fluorite.Strainer.UnitTests.Services.Filtering
{
    public class CustomFilterMethodMapperTests
    {
        [Fact]
        public void Mapper_Adds_NewCustomMethod()
        {
            // Arrange
            var customFilterMethod = new CustomFilterMethod<Uri>
            {
                Expression = (uri) => uri.Port == 443,
                Name = "HTTPS",
            };
            var mapper = new CustomFilterMethodMapper();

            // Act
            mapper.AddMap(customFilterMethod);

            // Assert
            mapper.Methods.ContainsKey(typeof(Uri)).Should().BeTrue();
            mapper.Methods[typeof(Uri)].Should().NotBeEmpty();
            mapper.Methods[typeof(Uri)].ContainsKey(customFilterMethod.Name).Should().BeTrue();
            mapper.Methods[typeof(Uri)][customFilterMethod.Name].Should().Be(customFilterMethod);
        }
    }
}
